using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Application
{
    public class UserFactory : IUserFactory
    {
        public User Create(RegisterRequest request, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public LoginResponse CreateLoginResponse(long userId, string token, string email)
        {
            throw new NotImplementedException();
        }
    }
}
