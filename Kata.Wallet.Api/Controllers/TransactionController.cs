using AutoMapper;
using Kata.Wallet.API.Controllers;
using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kata.Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ITransactionService transactionService, IMapper mapper,
            ILogger<TransactionController> logger)
        {
            this.transactionService = transactionService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("{id:int}", Name = "GetTransactionById")]
        [AllowAnonymous]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            try
            {
                logger.LogInformation("GetTransactionById => ID: {Id}", id);
                var transaction = await transactionService.GetById(id);
                if (transaction is null)
                {
                    logger.LogInformation("Transaction NotFound ID: {Id}", id);
                    return NotFound();
                }

                var dto = mapper.Map<TransactionDto>(transaction);
                return dto;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetTransactionById => ID: {Id}", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{walletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<TransactionGroupedByWalletDto>> GetByWalletId(int walletId)
        {
            try
            {
                logger.LogInformation("Init GetByWalletId => WalletId: {walletId}", walletId);
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
                logger.LogError(ex, "Error in GetByWalletId => WalletId: {walletId}", walletId);
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] TransactionCreationDto transactionCreationDto)
        {
            try
            {
                logger.LogInformation("Create Transaction: {Transaction}", transactionCreationDto);
                var transactionRequest = mapper.Map<TransactionRequest>(transactionCreationDto);
                await transactionService.Create(transactionRequest);
                return Ok(transactionRequest);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Create Transaction: {Transaction}", transactionCreationDto);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
