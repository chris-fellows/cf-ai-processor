using CFAIProcessor.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Services
{
    public class ToastService : IToastService
    {
        private Dictionary<string, Action<string>> _informationById = new Dictionary<string, Action<string>>();

        public string RegisterInformation(Action<string> action)
        {
            var id = Guid.NewGuid().ToString();
            _informationById.Add(id, action);
            return id;
        }

        public void UnregisterInformation(string id)
        {
            if (_informationById.ContainsKey(id)) _informationById.Remove(id);
        }

        public void Information(string text)
        {
            foreach (var id in _informationById.Keys)
            {
                _informationById[id].Invoke(text);
            }
        }
    }
}
