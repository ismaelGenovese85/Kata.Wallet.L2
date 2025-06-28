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

    public WalletController(IWalletService walletService, IMapper mapper) 
    {
        this.walletService = walletService;
        this.mapper = mapper;
    }

    [HttpGet("{id:int}", Name = "GetWalletById")]
    [AllowAnonymous]
    public async Task<ActionResult<WalletDto>> Get(int id)
    {
        try
        {
            var wallet = await walletService.GetById(id);
            if (wallet is null)
            {
                return NotFound();
            }

            var dto = mapper.Map<WalletDto>(wallet);
            return dto;

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("filter")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WalletDto>>> Filter([FromQuery] WalletFilterDto walletFilterDto)
    {
        try
        {
            var filter = mapper.Map<WalletFilter>(walletFilterDto);
            var list = mapper.Map<List<WalletDto>>(await walletService.Filter(filter)); 
            return Ok(list);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<ActionResult> Create([FromBody] WalletCreationDto walletDto)
    {
        try
        {
            var wallet = mapper.Map<Domain.Wallet>(walletDto);
            await walletService.Create(wallet);
            return CreatedAtRoute("GetWalletById", new { id = wallet.Id }, wallet);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
