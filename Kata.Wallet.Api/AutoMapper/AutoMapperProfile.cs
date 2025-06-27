using AutoMapper;
using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        ConfigMapTransaction();
        ConfigMapWallet();
        ConfigMapWalletFilter();
    }

    private void ConfigMapTransaction()
    {
        CreateMap<Domain.Transaction, TransactionDto>();
        CreateMap<TransactionDto, Domain.Transaction>();
        CreateMap<TransactionCreationDto, TransactionRequest>();
    }
    private void ConfigMapWallet()
    {
        CreateMap<Domain.Wallet, WalletDto>();
        CreateMap<WalletDto, Domain.Wallet>();
        CreateMap<WalletCreationDto, Domain.Wallet>();
    }

    private void ConfigMapWalletFilter()
    {
        CreateMap<WalletFilterDto, Domain.WalletFilter>();
    }
}
