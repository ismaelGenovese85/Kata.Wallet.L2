using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Dtos
{
    public class TransactionGroupedByWalletDto
    {
        public List<TransactionDto> Sent { get; set; } = new();
        public List<TransactionDto> Received { get; set; } = new();
    }
}
