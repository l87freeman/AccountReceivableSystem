using AutoMapper;
using IdentityServer.Application.Entities;
using IdentityServer.Application.Services.Abstractions;
using IdentityServer.Web.Models.Request;
using IdentityServer.Web.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthController(ILogger<AuthController> logger, IUserService userService, IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var tokenModel = await _userService.Login(_mapper.Map<UserIdentity>(request), HttpContext.RequestAborted);
        return Ok(_mapper.Map<LoginResponse>(tokenModel));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _userService.RegisterAsync(_mapper.Map<UserIdentity>(request), HttpContext.RequestAborted);
        return Ok();
    }
}