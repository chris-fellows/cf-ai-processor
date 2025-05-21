using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Constants
{
    public static class AuditEventTypeNames
    {
        public const string DataSetInfoAdded = "Data Set Info Added";
        public const string Error = "Error";
        public const string PasswordUpdated = "Password Updated";
        public const string UserAdded = "User Added";        
        public const string UserLogInError = "User Log In Error";
        public const string UserLogInSuccess = "User Log In Success";
        public const string UserLogOut = "User Log Out";
    }
}
