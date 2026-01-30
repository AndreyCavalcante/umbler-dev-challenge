using Desafio.Umbler.Controllers;
using Desafio.Umbler.Dtos;
using Desafio.Umbler.Models;
using Desafio.Umbler.Services;
using DnsClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Desafio.Umbler.Test.Controllers
{
    [TestClass]
    public class ControllersTest
    {
        [TestMethod]
        public async Task Get_Returns_Ok()
        {
            var serviceMock = new Mock<IDomainService>();

            serviceMock
                .Setup(s => s.GetDomainAsync(It.IsAny<string>()))
                .ReturnsAsync(new DomainResultDto
                {
                    Domain = "test.com",
                    Ip = "127.0.0.1",
                    HostedAt = "Umbler"
                });

            var controller = new DomainController(serviceMock.Object);

            var result = await controller.Get("test.com");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}