using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Kata.Wallet.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Tests.ServicesTests
{
    [TestClass]
    public sealed class WalletServiceTests
    {
        private Mock<IWalletRepository> walletRepoMock = null!;
        private Mock<ILogger<WalletService>> loggerMock = null!;
        private WalletService walletService = null!;

        [TestInitialize]
        public void Setup()
        {
            walletRepoMock = new Mock<IWalletRepository>();
            loggerMock = new Mock<ILogger<WalletService>>();
            walletService = new WalletService(walletRepoMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnWallet_WhenExists()
        {
            var wallet = new Domain.Wallet { Id = 1, UserDocument = "123", Currency = Currency.ARS };
            walletRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(wallet);

            var result = await walletService.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnNull_WhenWalletDoesNotExist()
        {
            int walletId = 999;

            walletRepoMock.Setup(repo => repo.GetById(walletId)).ReturnsAsync((Domain.Wallet?)null);

            var result = await walletService.GetById(walletId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Filter_ShouldReturnWallets_WhenFilterMatches()
        {
            var filter = new WalletFilter
            {
                UserDocument = "123",
                Currency = Currency.ARS
            };

            var expected = new List<Domain.Wallet>
            {
                new Domain.Wallet { Id = 1, UserDocument = "123", Currency = Currency.ARS }
            };

            walletRepoMock.Setup(r => r.Filter("123", Currency.ARS))
                .ReturnsAsync(expected);

            var result = await walletService.Filter(filter);


            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("123", result[0].UserDocument);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Filter_ShouldThrow_WhenFilterIsNull()
        {
            await walletService.Filter(null!);
        }
    }
}
