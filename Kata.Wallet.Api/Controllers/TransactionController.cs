using AutoMapper;
using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
using Kata.Wallet.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Wallet.Api.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IMapper mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            this.transactionService = transactionService;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetTransactionById")]
        [AllowAnonymous]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            try
            {
                var transaction = await transactionService.GetById(id);
                if (transaction is null)
                {
                    return NotFound();
                }

                var dto = mapper.Map<TransactionDto>(transaction);
                return dto;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{walletId}")]
        [AllowAnonymous]
        public async Task<ActionResult<TransactionGroupedByWalletDto>> GetByWalletId(int walletId)
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

        [HttpPost("create")]
        [AllowAnonymous]
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
