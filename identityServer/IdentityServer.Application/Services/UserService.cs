using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityServer.Application.Entities;
using IdentityServer.Application.Exceptions;
using IdentityServer.Application.Repositories.Abstractions;
using IdentityServer.Application.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly JwtOptions _jwtOptions;

    public UserService(IUserRepository userRepository, IHashService hashService, IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<TokenModel> Login(UserIdentity user, CancellationToken cancellationToken)
    {
        var foundUser = await AuthenticateAsync(user, cancellationToken);

        var model = new TokenModel
        {
            AccessToken = GenerateToken(foundUser),
            ExpiresIn = 2629800,
            TokenType = "Bearer"
        };

        return model;
    }

    public async Task RegisterAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        user.Password = _hashService.ComputeHash(user.Password);
        await _userRepository.CreateUserAsync(user, cancellationToken);
    }

    private async Task<UserIdentity> AuthenticateAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        var foundUser = await _userRepository.FindUserAsync(user.Email, cancellationToken);
        if (foundUser == null)
        {
            throw new IncorrectLoginDataException("Email");
        }

        if (!_hashService.IsMatch(foundUser.Password, user.Password))
        {
            throw new IncorrectLoginDataException("Password");
        }

        return foundUser;
    }

    private string GenerateToken(UserIdentity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Email),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Sid,user.Id),
            new Claim(ClaimTypes.Role,"user")
        };
        var token = new JwtSecurityToken("",
            "",
            claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}