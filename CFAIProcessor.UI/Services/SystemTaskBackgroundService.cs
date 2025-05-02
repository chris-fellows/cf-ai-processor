using CFAIProcessor.SystemTask;

namespace CFAIProcessor.UI.Services
{
    public class SystemTaskBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISystemTaskList _systemTaskList;

        public SystemTaskBackgroundService(IServiceProvider serviceProvider,
                                        ISystemTaskList systemTaskList)
        {
            _serviceProvider = serviceProvider;
            _systemTaskList = systemTaskList;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            System.Diagnostics.Debug.WriteLine($"System Task background service running");

            while (!stoppingToken.IsCancellationRequested ||
                _systemTaskList.SystemTaskActives.Any())
            {
                // Start next regular system task
                if (!stoppingToken.IsCancellationRequested)
                {
                    StartNextRegularSystemTask();
                }

                await Task.Delay(1);

                // Start next requested system task
                if (!stoppingToken.IsCancellationRequested)
                {
                    StartNextRequestedSystemTask();
                }

                // Check completed
                CheckCompleteSystemTasks();

                await Task.Delay(5000);
            }

            System.Diagnostics.Debug.WriteLine($"System Task background service terminated");
        }

        /// <summary>
        /// Starts next regular system task that is overdue
        /// </summary>
        private void StartNextRegularSystemTask()
        {
            var systemTaskConfig = _systemTaskList.GetNextRegularOverdue();
            if (systemTaskConfig != null)
            {
                // Start system task                
                StartExecuteSystemTask(systemTaskConfig.SystemTaskName, new Dictionary<string, object>());

                // Set next execute time
                do
                {
                    systemTaskConfig.NextExecuteTime = systemTaskConfig.NextExecuteTime.Add(systemTaskConfig.ExecuteFrequency);
                } while (systemTaskConfig.NextExecuteTime < DateTimeOffset.UtcNow);
            }
        }

        /// <summary>
        /// Starts next requested system task that is overdue
        /// </summary>
        private void StartNextRequestedSystemTask()
        {
            var systemTaskRequest = _systemTaskList.GetNextRequestedOverdue();
            if (systemTaskRequest != null)
            {
                StartExecuteSystemTask(systemTaskRequest.SystemTaskName, systemTaskRequest.Parameters);
            }
        }

        /// <summary>
        /// Starts executing system task
        /// </summary>
        /// <param name="systemTask"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Task StartExecuteSystemTask(string systemTaskName, Dictionary<string, object> parameters)
        {
            ISystemTask? systemTask = null;

            // Start task
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Factory.StartNew(() =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    systemTask = scope.ServiceProvider.GetServices<ISystemTask>().First(st => st.Name == systemTaskName);

                    System.Diagnostics.Debug.WriteLine($"Executing system task {systemTask.Name}");

                    systemTask.ExecuteAsync(parameters, scope.ServiceProvider, cancellationTokenSource.Token).Wait();

                    System.Diagnostics.Debug.WriteLine($"Executed system task {systemTask.Name}");
                }
            });

            // Wait for ISystemTask to be initialized because we need it below. 
            // This code is messy
            while (systemTask == null)
            {
                System.Threading.Thread.Sleep(100);
            }

            // Set active
            _systemTaskList.SetActive(systemTask, task, cancellationTokenSource);

            return task;
        }

        /// <summary>
        /// Checks completed system tasks
        /// </summary>
        private void CheckCompleteSystemTasks()
        {
            // Get completed system tasks
            var systemTaskActives = _systemTaskList.SystemTaskActives.Where(st => st.Task.IsCompleted).ToList();

            // Check results
            while (systemTaskActives.Any())
            {
                var systemTaskActive = systemTaskActives.First();
                systemTaskActives.Remove(systemTaskActive);

                _systemTaskList.SetComplete(systemTaskActive.SystemTask);

                // Log status
                if (systemTaskActive.Task.Exception != null)    // Error
                {
                    System.Diagnostics.Debug.WriteLine($"System task error for {systemTaskActive.SystemTask.Name}: {systemTaskActive.Task.Exception.Message}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Executed system task {systemTaskActive.SystemTask.Name}");
                }
            }
        }
    }
}
