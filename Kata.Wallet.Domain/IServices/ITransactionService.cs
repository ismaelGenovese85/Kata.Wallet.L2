using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain.IServices
{
    public interface ITransactionService
    {
        Task Create(TransactionRequest transactionRequest);
        Task<(List<Transaction> Sent, List<Transaction> Received)> GetGroupedByWalletId(int walletId);
    }
}
