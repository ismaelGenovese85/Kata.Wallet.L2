using AutoMapper;
using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kata.Wallet.API.Controllers;

[ApiController]
[Route("api/wallet")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WalletController : ControllerBase
{
    private readonly IWalletService walletService;
    private readonly IMapper mapper;
    private readonly ILogger<WalletController> logger;

    public WalletController(IWalletService walletService, IMapper mapper,
        ILogger<WalletController> logger) 
    {
        this.walletService = walletService;
        this.mapper = mapper;
        this.logger = logger;
    }

    [HttpGet("{id:int}", Name = "GetWalletById")]
    [AllowAnonymous]
    public async Task<ActionResult<WalletDto>> Get(int id)
    {
        try
        {
            logger.LogInformation("GetWalletById => ID: {Id}", id);
            var wallet = await walletService.GetById(id);
            if (wallet is null)
            {
                logger.LogInformation("Wallet NotFound ID: {Id}", id);
                return NotFound();
            }

            var dto = mapper.Map<WalletDto>(wallet);
            return dto;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetWalletById => id {WalletId}", id);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("filter")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WalletDto>>> Filter([FromQuery] WalletFilterDto walletFilterDto)
    {
        try
        {
            logger.LogInformation("Filter wallets => paramas: {@Filter}", walletFilterDto);
            var filter = mapper.Map<WalletFilter>(walletFilterDto);
            var list = mapper.Map<List<WalletDto>>(await walletService.Filter(filter)); 
            return Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in filter");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<ActionResult> Create([FromBody] WalletCreationDto walletDto)
    {
        try
        {
            logger.LogInformation("Create wallet: {wallet}", walletDto);
            var wallet = mapper.Map<Domain.Wallet>(walletDto);
            await walletService.Create(wallet);
            return CreatedAtRoute("GetWalletById", new { id = wallet.Id }, wallet);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in create wallet");
            return StatusCode(500, ex.Message);
        }
    }
}
