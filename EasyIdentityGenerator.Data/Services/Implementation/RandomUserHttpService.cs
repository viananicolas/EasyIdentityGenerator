using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class RandomUserHttpService : IHttpService<RandomUser>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public RandomUserHttpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task<RandomUser> Get()
        {
            var responseString = await _httpClient.GetStringAsync(_configuration["RandomUserUri"]);
            var randomUser = JsonConvert.DeserializeObject<RandomUser>(responseString);
            return randomUser;
        }
    }
}
