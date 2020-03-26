using System.Linq;
using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class IdentityGenerator : IIdentityGenerator
    {
        private readonly IHttpService<RandomUser> _randomUserHttpService;
        private readonly IHttpService<RandomPassword> _randomPasswordHttpService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasherMvc _passwordHasherMvc;
        public IdentityGenerator(IHttpService<RandomUser> randomUserHttpService,
            UserManager<ApplicationUser> userManager,
            IPasswordHasherMvc passwordHasherMvc,
            IHttpService<RandomPassword> randomPasswordHttpService)
        {
            _randomUserHttpService = randomUserHttpService;
            _userManager = userManager;
            _passwordHasherMvc = passwordHasherMvc;
            _randomPasswordHttpService = randomPasswordHttpService;
        }

        public async Task<ApplicationUser> GenerateUser()
        {
            var randomUser = await _randomUserHttpService.Get();
            var randomPassword = await _randomPasswordHttpService.Get();
            var user = randomUser.Results.FirstOrDefault();
            var password = randomPassword.Char.FirstOrDefault();
            if (user == null || password == null) return null;
            var applicationUser = new ApplicationUser
            {
                Password = password,
                Email = user.Email,
                UserName = user.Email
            };
            await _userManager.CreateAsync(applicationUser, user.Login.Password);
            applicationUser.PasswordHashMvc = _passwordHasherMvc.HashPassword(applicationUser.Password);
            applicationUser.Json = JsonConvert.SerializeObject(applicationUser, Formatting.Indented).Trim();
            return applicationUser;
        }
    }
}