using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Kata.Wallet.Domain.IServices;
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
    public sealed class TransactionServiceTests
    {
        private Mock<ITransactionRepository> transactionRepoMock = null!;
        private Mock<ILogger<TransactionService>> loggerTransactionMock = null!;
        private Mock<IWalletService> walletServiceMock = null!;
        private TransactionService transactionService = null!;

        [TestInitialize]
        public void Setup()
        {
            transactionRepoMock = new Mock<ITransactionRepository>();
            loggerTransactionMock = new Mock<ILogger<TransactionService>>();
            walletServiceMock = new Mock<IWalletService>();

            transactionService = new TransactionService(
                transactionRepoMock.Object,
                loggerTransactionMock.Object,
                walletServiceMock.Object
            );
        }

        [TestMethod]
        public async Task Create_ShouldThrowException_WhenOriginWalletNotFound()
        {
            var request = new TransactionRequest
            {
                OriginWalletId = 1,
                DestinationWalletId = 2,
                Amount = 100,
                Description = "Test"
            };

            walletServiceMock.Setup(w => w.GetById(1)).ReturnsAsync((Domain.Wallet?)null);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => await transactionService.Create(request));

            Assert.AreEqual("Origin wallet not found.", ex.Message);
        }

        [TestMethod]
        public async Task Create_ShouldThrowException_WhenDestinationWalletNotFound()
        {
            var origin = new Domain.Wallet { Id = 1, Currency = Currency.ARS, Balance = 1000 };

            walletServiceMock.Setup(w => w.GetById(1)).ReturnsAsync(origin);
            walletServiceMock.Setup(w => w.GetById(2)).ReturnsAsync((Domain.Wallet?)null);

            var request = new TransactionRequest
            {
                OriginWalletId = 1,
                DestinationWalletId = 2,
                Amount = 100,
                Description = "Test"
            };

            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => await transactionService.Create(request));

            Assert.AreEqual("Destination wallet not found.", ex.Message);
        }

        [TestMethod]
        public async Task Create_ShouldThrowException_WhenCurrenciesAreDifferent()
        {
            var origin = new Domain.Wallet { Id = 1, Currency = Currency.ARS, Balance = 1000 };
            var destination = new Domain.Wallet { Id = 2, Currency = Currency.USD };

            walletServiceMock.Setup(w => w.GetById(1)).ReturnsAsync(origin);
            walletServiceMock.Setup(w => w.GetById(2)).ReturnsAsync(destination);

            var request = new TransactionRequest
            {
                OriginWalletId = 1,
                DestinationWalletId = 2,
                Amount = 100,
                Description = "Test"
            };

            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => await transactionService.Create(request));

            Assert.AreEqual("Both wallets must have the same currency.", ex.Message);
        }

        [TestMethod]
        public async Task Create_ShouldThrowException_WhenOriginHasInsufficientBalance()
        {
            var origin = new Domain.Wallet { Id = 1, Currency = Currency.ARS, Balance = 50 };
            var destination = new Domain.Wallet { Id = 2, Currency = Currency.ARS };

            walletServiceMock.Setup(w => w.GetById(1)).ReturnsAsync(origin);
            walletServiceMock.Setup(w => w.GetById(2)).ReturnsAsync(destination);

            var request = new TransactionRequest
            {
                OriginWalletId = 1,
                DestinationWalletId = 2,
                Amount = 100,
                Description = "Test"
            };

            var ex = await Assert.ThrowsExceptionAsync<Exception>(
                async () => await transactionService.Create(request));

            Assert.AreEqual("Insufficient balance in the origin wallet.", ex.Message);
        }

        [TestMethod]
        public async Task Create_ShouldSucceed_WhenTransactionIsValid()
        {
            var origin = new Domain.Wallet { Id = 1, Currency = Currency.ARS, Balance = 500 };
            var destination = new Domain.Wallet { Id = 2, Currency = Currency.ARS, Balance = 100 };

            walletServiceMock.Setup(w => w.GetById(1)).ReturnsAsync(origin);
            walletServiceMock.Setup(w => w.GetById(2)).ReturnsAsync(destination);

            var request = new TransactionRequest
            {
                OriginWalletId = 1,
                DestinationWalletId = 2,
                Amount = 200,
                Description = "Pago"
            };

            await transactionService.Create(request);

            transactionRepoMock.Verify(r => r.Create(It.Is<Transaction>(t =>
                t.Amount == 200 &&
                t.Description == "Pago" &&
                t.WalletOutgoing == origin &&
                t.WalletIncoming == destination
            )), Times.Once);

            walletServiceMock.Verify(w => w.SaveChangesAsync(), Times.Once);

            Assert.AreEqual(300, origin.Balance);
            Assert.AreEqual(300, destination.Balance);
        }

    }
}
