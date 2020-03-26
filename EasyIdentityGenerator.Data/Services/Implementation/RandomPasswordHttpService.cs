using System.Net.Http;
using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class RandomPasswordHttpService : IHttpService<RandomPassword>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public RandomPasswordHttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<RandomPassword> Get()
        {
            var responseString = await _httpClient.GetStringAsync(_configuration["RandomPasswordUri"]);
            var randomPassword = JsonConvert.DeserializeObject<RandomPassword>(responseString);
            return randomPassword;
        }
    }
}