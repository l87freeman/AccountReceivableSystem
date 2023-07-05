using FluentValidation;
using IdentityServer.Web.Models.Request;

namespace IdentityServer.Web.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
    }
}