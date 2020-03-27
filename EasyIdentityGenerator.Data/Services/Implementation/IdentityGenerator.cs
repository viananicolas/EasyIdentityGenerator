using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Helpers;
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
        private readonly IFileReader _fileReader;

        public IdentityGenerator(IHttpService<RandomUser> randomUserHttpService,
            UserManager<ApplicationUser> userManager,
            IPasswordHasherMvc passwordHasherMvc,
            IHttpService<RandomPassword> randomPasswordHttpService,
            IFileReader fileReader)
        {
            _randomUserHttpService = randomUserHttpService;
            _userManager = userManager;
            _passwordHasherMvc = passwordHasherMvc;
            _randomPasswordHttpService = randomPasswordHttpService;
            _fileReader = fileReader;
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
            applicationUser.Json = GenerateJson(applicationUser);
            applicationUser.CoreSql = GenerateCoreSql(applicationUser);
            applicationUser.MvcSql = GenerateMvcSql(applicationUser);
            return applicationUser;
        }

        private string GenerateCoreSql(ApplicationUser applicationUser)
        {
            var sql = _fileReader.GetFile("aspnetusersinsert_dotnetcore.sql");
            var generatedSql = sql
                .ReplaceWholeWord("PlaceholderId", $"'{applicationUser.Id}'")
                .ReplaceWholeWord("PlaceholderAccessFailedCount", $"'{applicationUser.AccessFailedCount}'")
                .ReplaceWholeWord("PlaceholderConcurrencyStamp", $"'{applicationUser.ConcurrencyStamp}'")
                .ReplaceWholeWord("PlaceholderEmail", $"'{applicationUser.Email}'")
                .ReplaceWholeWord("PlaceholderEmailConfirmed", $"{applicationUser.EmailConfirmed.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderLockoutEnabled", $"{applicationUser.LockoutEnabled.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderLockoutEnd", $"'{applicationUser.LockoutEnd.GetValueOrDefault().Date:yyyy-MM-dd HH:mm:ss.fff}'")
                .ReplaceWholeWord("PlaceholderNormalizedEmail", $"'{applicationUser.NormalizedEmail}'")
                .ReplaceWholeWord("PlaceholderNormalizedUserName", $"'{applicationUser.NormalizedUserName}'")
                .ReplaceWholeWord("PlaceholderPasswordHash", $"'{applicationUser.PasswordHash}'")
                .ReplaceWholeWord("PlaceholderPhoneNumber", $"'{applicationUser.PhoneNumber}'")
                .ReplaceWholeWord("PlaceholderPhoneNumberConfirmed", $"{applicationUser.PhoneNumberConfirmed.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderSecurityStamp", $"'{applicationUser.SecurityStamp}'")
                .ReplaceWholeWord("PlaceholderTwoFactorEnabled", $"{applicationUser.TwoFactorEnabled.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderUserName", $"'{applicationUser.UserName}'");
            return generatedSql;
        }        
        private string GenerateMvcSql(ApplicationUser applicationUser)
        {
            var sql = _fileReader.GetFile("aspnetusersinsert_dotnetframework.sql");
            var generatedSql = sql
                .ReplaceWholeWord("PlaceholderId", $"'{applicationUser.Id}'")
                .ReplaceWholeWord("PlaceholderEmail", $"'{applicationUser.Email}'")
                .ReplaceWholeWord("PlaceholderEmailConfirmed", $"{applicationUser.EmailConfirmed.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderPasswordHash", $"'{applicationUser.PasswordHashMvc}'")
                .ReplaceWholeWord("PlaceholderSecurityStamp", $"'{applicationUser.SecurityStamp}'")
                .ReplaceWholeWord("PlaceholderPhoneNumber", $"'{applicationUser.PhoneNumber}'")
                .ReplaceWholeWord("PlaceholderPhoneNumberConfirmed", $"{applicationUser.PhoneNumberConfirmed.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderTwoFactorEnabled", $"{applicationUser.TwoFactorEnabled.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderLockoutEnabled", $"{applicationUser.LockoutEnabled.GetHashCode()}")
                .ReplaceWholeWord("PlaceholderLockoutEndDateUtc", $"'{applicationUser.LockoutEnd.GetValueOrDefault().Date:yyyy-MM-dd HH:mm:ss.fff}'")
                .ReplaceWholeWord("PlaceholderAccessFailedCount", $"'{applicationUser.AccessFailedCount}'")
                .ReplaceWholeWord("PlaceholderUserName", $"'{applicationUser.UserName}'");

            return generatedSql;
        }

        private static string GenerateJson(ApplicationUser applicationUser)
        {
            return JsonConvert.SerializeObject(applicationUser, Formatting.Indented).Trim();
        }
    }
}