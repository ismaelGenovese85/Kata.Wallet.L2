using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Kata.Wallet.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Services.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IWalletService walletService;

        public TransactionService(
            ITransactionRepository transactionRepository, 
            IWalletService walletService)
        {
            this.transactionRepository = transactionRepository;
            this.walletService = walletService;
        }

        public async Task<Domain.Transaction?> GetById(int id)
        {
            return await transactionRepository.GetById(id);
        }
        public async Task Create(TransactionRequest transactionRequest)
        {
            var origin = await walletService.GetById(transactionRequest.OriginWalletId)
                ?? throw new Exception("Origin wallet not found.");

            var destination = await walletService.GetById(transactionRequest.DestinationWalletId)
                ?? throw new Exception("Destination wallet not found.");

            if (origin.Currency != destination.Currency)
                throw new Exception("Both wallets must have the same currency.");

            if (origin.Balance < transactionRequest.Amount)
                throw new Exception("Insufficient balance in the origin wallet.");

            var transaction = new Transaction
            {
                Amount = transactionRequest.Amount,
                Description = transactionRequest.Description,
                Date = DateTime.UtcNow,
                WalletOutgoing = origin,
                WalletIncoming = destination
            };

            origin.Balance -= transactionRequest.Amount;
            destination.Balance += transactionRequest.Amount;

            await transactionRepository.Create(transaction);
            await walletService.SaveChangesAsync();
        }

        public async Task<(List<Transaction> Sent, List<Transaction> Received)> GetGroupedByWalletId(int walletId)
        {
            var transactions = await transactionRepository.GetAllByWalletId(walletId);

            var sent = transactions.Where(t => t.WalletOutgoing.Id == walletId).ToList();
            var received = transactions.Where(t => t.WalletIncoming.Id == walletId).ToList();

            return (sent, received);
        }
    }
}
