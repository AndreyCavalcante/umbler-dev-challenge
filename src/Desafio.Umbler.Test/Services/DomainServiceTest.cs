using Desafio.Umbler.Models;
using Desafio.Umbler.Services;
using Desafio.Umbler.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desafio.Umbler.Dtos;

namespace Desafio.Umbler.Test.Services
{
    [TestClass]
    public class DomainServiceTests
    {
        private DatabaseContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options);
        }

        [TestMethod]
        public async Task GetDomainAsync_WhenDomainIsNotInDatabase_ShouldResolveAndPersist()
        {
            var db = CreateDbContext();

            var dnsMock = new Mock<IDnsService>();
            dnsMock.Setup(d => d.ResolveAsync("test.com"))
                .ReturnsAsync(new DnsResult
                {
                    Ip = "1.1.1.1",
                    Ttl = 60
                });

            dnsMock.Setup(d => d.GetNameServersAsync("test.com"))
                .ReturnsAsync(new List<string> { "ns1.test.com", "ns2.test.com" });

            var whoisMock = new Mock<IWhoisService>();
            whoisMock.Setup(w => w.QueryAsync("test.com"))
                .ReturnsAsync(new WhoisResult { Raw = "DOMAIN WHOIS" });

            whoisMock.Setup(w => w.QueryAsync("1.1.1.1"))
                .ReturnsAsync(new WhoisResult { OrganizationName = "Test Hosting" });

            var service = new DomainService(db, dnsMock.Object, whoisMock.Object);

            var result = await service.GetDomainAsync("test.com");

            Assert.IsNotNull(result);
            Assert.AreEqual("test.com", result.Domain);
            Assert.AreEqual("1.1.1.1", result.Ip);
            Assert.AreEqual("Test Hosting", result.HostedAt);

            var savedDomain = await db.Domains.FirstOrDefaultAsync();
            Assert.IsNotNull(savedDomain);
        }
    }
}
