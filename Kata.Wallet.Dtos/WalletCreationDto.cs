using Kata.Wallet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Dtos
{
    public class WalletCreationDto
    {
        public decimal Balance { get; set; }
        public string? UserDocument { get; set; }
        public string? UserName { get; set; }
        public Currency Currency { get; set; }
    }
}
