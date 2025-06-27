using AutoMapper;
using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IMapper mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            this.transactionService = transactionService;
            this.mapper = mapper;
        }

        [HttpGet("{walletId}")]
        public async Task<ActionResult<List<TransactionDto>>> GetByWalletId(int walletId)
        {
            try
            {
                var (sent, received) = await transactionService.GetGroupedByWalletId(walletId);

                var result = new TransactionGroupedByWalletDto
                {
                    Sent = mapper.Map<List<TransactionDto>>(sent),
                    Received = mapper.Map<List<TransactionDto>>(received)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("wallet/{walletId}")]
        public async Task<ActionResult<WalletTransactionListDto>> GetGrouped(int walletId)
        {
            var result = await transactionService.GetGroupedByWalletId(walletId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TransactionCreationDto transactionCreationDto)
        {
            try
            {
                var transactionRequest = mapper.Map<TransactionRequest>(transactionCreationDto);
                await transactionService.Create(transactionRequest);
                return Ok(transactionRequest);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
