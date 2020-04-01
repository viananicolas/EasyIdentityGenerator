using EasyIdentityGenerator.Data.Models;

namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IDataGenerator
    {
        string GenerateCoreSql(ApplicationUser applicationUser);
        string GenerateMvcSql(ApplicationUser applicationUser);
        string GenerateJson(ApplicationUser applicationUser);
    }
}