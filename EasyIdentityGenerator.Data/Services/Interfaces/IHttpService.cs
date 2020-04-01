using System.Threading.Tasks;

namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IHttpService<T> where T : class
    {
        Task<T> Get();
    }
}