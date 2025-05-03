using CFAIProcessor.Enums;
using CFAIProcessor.Interfaces;
using CFAIProcessor.Models;
using CFAIProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Common.Services
{
    public class XmlUserService : XmlEntityWithIdService<User, string>, IUserService
    {
        private readonly IPasswordService _passwordService;

        public XmlUserService(string folder, IPasswordService passwordService) : base(folder,
                                                "User.*.xml",
                                              (user) => $"User.{user.Id}.xml",
                                                (userId) => $"User.{userId}.xml")
        {
            _passwordService = passwordService;
        }

        public User? ValidateCredentials(string username, string password)
        {
            var user = GetAll().FirstOrDefault(u => u.Email == username);
            if (user != null &&
                user.Active &&
                user.GetUserType() == UserTypes.Normal &&
                _passwordService.IsValid(user.Password, password, user.Salt))
            {
                return user;
            }
            return null;
        }
    }
}
