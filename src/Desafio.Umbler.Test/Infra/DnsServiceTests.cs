using Desafio.Umbler.Dtos;
using Desafio.Umbler.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Umbler.Test.Infra
{
    [TestClass]
    public class DnsServiceTests
    {
        private DnsService _dnsService;

        [TestInitialize]
        public void Setup()
        {
            _dnsService = new DnsService();
        }

        [TestMethod]
        public async Task ResolveAsync_ShouldReturnIpAndTtl()
        {
            DnsResult result = await _dnsService.ResolveAsync("test.com");

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Ip), "IP should not be null or empty");
            Assert.IsTrue(result.Ttl > 0, "TTL should be greater than 0");
        }

        [TestMethod]
        public async Task GetNameServersAsync_ShouldReturnListOfNs()
        {
            List<string> nsList = await _dnsService.GetNameServersAsync("test.com");

            Assert.IsNotNull(nsList);
            Assert.IsTrue(nsList.Count > 0, "There should be at least one NS record");
        }
    }
}
