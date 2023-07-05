using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Domain.Exceptions;
using AccountReceivableSystem.Domain.Ports;
using Microsoft.AspNetCore.Http;

namespace AccountReceivableSystem.Infrastructure.Adapters;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Task<UserIdentity> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var user = _contextAccessor.HttpContext?.User;
        ThrowOnNull(user);

        var userId = GetClaimOrDefault(user, ClaimTypes.Sid);

        ThrowOnNull(userId);

        var userIdentity = new UserIdentity
        {
            Email = GetClaimOrDefault(user, ClaimTypes.NameIdentifier),
            Id = GetClaimOrDefault(user, ClaimTypes.Sid),
            UserName = GetClaimOrDefault(user, ClaimTypes.Name),
        };

        return Task.FromResult(userIdentity);

        static void ThrowOnNull<T>(T value) where T : class
        {
            if (value == null)
            {
                throw new UserIdentityNotFound();
            }
        }
    }

    private string GetClaimOrDefault(ClaimsPrincipal user, string claimType) 
        => user.Claims.FirstOrDefault(cl => cl.Type.Equals(claimType, StringComparison.InvariantCultureIgnoreCase))?.Value;
}