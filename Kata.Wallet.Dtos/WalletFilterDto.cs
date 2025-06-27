using Kata.Wallet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Dtos
{
    public class WalletFilterDto
    {
        public Currency? Currency { get; set; }
        public string? UserDocument { get; set; }
    }
}
