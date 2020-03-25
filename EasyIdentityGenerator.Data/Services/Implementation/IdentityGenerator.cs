using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class IdentityGenerator : IIdentityGenerator
    {
        public ApplicationUser GenerateUser()
        {
            var applicationUser = new ApplicationUser();
            applicationUser.Password = "oi";
            applicationUser.PasswordHash = "fsdf";
            return applicationUser;
        }
    }
}