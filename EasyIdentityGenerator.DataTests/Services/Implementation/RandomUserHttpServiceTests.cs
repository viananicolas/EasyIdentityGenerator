using EasyIdentityGenerator.Data.Models;
using EasyIdentityGenerator.Data.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EasyIdentityGenerator.DataTests.Services.Implementation
{
    [TestClass]
    public class RandomUserHttpServiceTests
    {
        [TestMethod]
        public async void GetTest()
        {
            var mockHttpService = new Mock<IHttpService<RandomUser>>();
            mockHttpService.Setup(e => e.Get()).ReturnsAsync(new RandomUser());
            var httpService = mockHttpService.Object;
            Assert.IsNotNull(await httpService.Get());
        }
    }
}