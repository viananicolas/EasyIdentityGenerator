using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Models;

namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IIdentityGenerator
    {
        Task<ApplicationUser> GenerateUser();
    }
}