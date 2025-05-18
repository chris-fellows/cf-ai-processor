using CFAIProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Interfaces
{
    public interface IRequestContextService
    {
        /// <summary>
        /// User Id
        /// </summary>
        public string? UserId { get; }

        /// <summary>
        /// User
        /// </summary>
        public User? User { get; }

        /// <summary>
        /// System User
        /// </summary>
        public User SystemUser { get; }
    }
}
