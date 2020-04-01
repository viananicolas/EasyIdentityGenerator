using EasyIdentityGenerator.Data.Helpers;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Newtonsoft.Json;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class DataGenerator : IDataGenerator
    {
        private readonly IFileReader _fileReader;

        public DataGenerator(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public string GenerateCoreSql(ApplicationUser applicationUser)
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

        public string GenerateMvcSql(ApplicationUser applicationUser)
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

        public string GenerateJson(ApplicationUser applicationUser)
        {
            return JsonConvert.SerializeObject(applicationUser, Formatting.Indented).Trim();
        }
    }
}