using EasyIdentityGenerator.Data.Models;

namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IIdentityGenerator
    {
        ApplicationUser GenerateUser();
    }
}