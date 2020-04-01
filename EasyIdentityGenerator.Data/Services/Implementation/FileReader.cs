using System.IO;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace EasyIdentityGenerator.Data.Services.Implementation
{
    public class FileReader : IFileReader
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileReader(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        public string GetFile(string fileName)
        {
            var sql = File.ReadAllText($"Assets/{fileName}");
            return sql;
        }
    }
}