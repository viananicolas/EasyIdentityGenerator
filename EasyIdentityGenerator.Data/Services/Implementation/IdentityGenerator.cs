using System.Linq;
using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class IdentityGenerator : IIdentityGenerator
    {
        private readonly IHttpService<RandomUser> _httpService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityGenerator(IHttpService<RandomUser> httpService, UserManager<ApplicationUser> userManager)
        {
            _httpService = httpService;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GenerateUser()
        {
            var randomUser = await _httpService.Get();
            var user = randomUser.Results.FirstOrDefault();
            if (user == null) return null;
            var applicationUser = new ApplicationUser { Password = user.Login.Password, Email = user.Email, UserName = user.Email };
            await _userManager.CreateAsync(applicationUser, user.Login.Password);
            return applicationUser;
        }
    }
}