using CFAIProcessor.Models;

namespace CFAIProcessor.SystemTask
{
    public class SystemTaskList : ISystemTaskList
    {
        private readonly int _maxConcurrentTasks;
        //private readonly List<ISystemTask> _systemTasks;
        private readonly List<SystemTaskConfig> _systemTaskConfigs;
        private readonly List<SystemTaskActive> _systemTaskActives = new List<SystemTaskActive>();
        private readonly List<SystemTaskRequest> _systemTaskRequests = new List<SystemTaskRequest>();

        public SystemTaskList(int maxConcurrentTasks,
                    List<SystemTaskConfig> systemTaskConfigs)
        {
            _maxConcurrentTasks = maxConcurrentTasks;
            _systemTaskConfigs = systemTaskConfigs;
        }

        public int MaxConcurrentTasks => _maxConcurrentTasks;

        //public IReadOnlyList<ISystemTask> SystemTasks => _systemTasks;

        public IReadOnlyList<SystemTaskActive> SystemTaskActives => _systemTaskActives;

        public IReadOnlyList<SystemTaskRequest> SystemTaskRequests => _systemTaskRequests;

        public void AddRequest(SystemTaskRequest systemTaskRequest)
        {
            _systemTaskRequests.Add(systemTaskRequest);
        }

        public SystemTaskConfig? GetNextRegularOverdue()
        {
            if (_systemTaskActives.Count < _maxConcurrentTasks)
            {
                foreach (var systemTaskConfig in _systemTaskConfigs.Where(c => c.NextExecuteTime <= DateTimeOffset.UtcNow))
                {
                    if (!_systemTaskActives.Any(st => st.SystemTask.Name == systemTaskConfig.SystemTaskName)) // Not already executing
                    {
                        return systemTaskConfig;
                    }
                }
            }

            return null;
        }

        public SystemTaskRequest? GetNextRequestedOverdue()
        {
            if (_systemTaskActives.Count < _maxConcurrentTasks)
            {
                var systemTaskRequest = _systemTaskRequests.Where(st => st.ExecuteTime <= DateTimeOffset.UtcNow).FirstOrDefault();
                if (systemTaskRequest != null)
                {
                    _systemTaskRequests.Remove(systemTaskRequest);
                }
                return systemTaskRequest;
            }

            return null;
        }

        public void SetActive(ISystemTask systemTask, Task task, CancellationTokenSource cancellationTokenSource)
        {
            var systemTaskActive = new SystemTaskActive(systemTask, task, cancellationTokenSource);
            _systemTaskActives.Add(systemTaskActive);
        }

        public void SetComplete(ISystemTask systemTask)
        {
            var systemTaskActive = _systemTaskActives.First(st => st.SystemTask.Name == systemTask.Name);
            _systemTaskActives.Remove(systemTaskActive);
        }

        public void Cancel(ISystemTask systemTask)
        {
            var systemTaskActive = _systemTaskActives.FirstOrDefault(st => st.SystemTask.Name == systemTask.Name);
            if (systemTaskActive != null)
            {
                // Request cancel
                systemTaskActive.CancellationTokenSource.Cancel();

                // Wait for task to abort
                while (!systemTaskActive.Task.IsCompleted)
                {
                    Thread.Sleep(50);
                }

                SetComplete(systemTask);
            }
        }
    }
}
