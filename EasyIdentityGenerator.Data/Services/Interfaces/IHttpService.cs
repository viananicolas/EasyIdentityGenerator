using System.Net.Http;

namespace EasyIdentityGenerator.Data.Services.Interfaces
{
    public interface IHttpService
    {
        HttpResponseMessage ResponseMessage(string uri);
    }
}