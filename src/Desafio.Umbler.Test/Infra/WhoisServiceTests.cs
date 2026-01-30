using Desafio.Umbler.Dtos;
using Desafio.Umbler.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Desafio.Umbler.Test.Infra
{
    [TestClass]
    public class WhoisServiceTests
    {
        private WhoisService _whoisService;

        [TestInitialize]
        public void Setup()
        {
            _whoisService = new WhoisService();
        }

        [TestMethod]
        public async Task QueryAsync_ShouldReturnRawAndOrganization()
        {
            WhoisResult result = await _whoisService.QueryAsync("google.com");

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Raw), "Raw response should not be null or empty");
        }
    }
}
