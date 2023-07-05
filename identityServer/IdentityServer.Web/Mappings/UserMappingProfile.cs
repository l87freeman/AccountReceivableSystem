using AutoMapper;
using IdentityServer.Application.Entities;
using IdentityServer.Web.Models.Request;
using IdentityServer.Web.Models.Response;
using MongoDB.Bson;

namespace IdentityServer.Web.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<LoginRequest, UserIdentity>()
            .ForMember(x => x.Password, opt => opt.MapFrom(s => s.Password))
            .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.UserName, opt => opt.Ignore())
            .ForMember(x => x.RegisterDate, opt => opt.Ignore());

        CreateMap<TokenModel, LoginResponse>()
            .ForMember(x => x.TokenType, opt => opt.MapFrom(s => s.TokenType))
            .ForMember(x => x.ExpiresIn, opt => opt.MapFrom(s => s.ExpiresIn))
            .ForMember(x => x.AccessToken, opt => opt.MapFrom(s => s.AccessToken));

        CreateMap<RegisterRequest, UserIdentity>()
            .ForMember(x => x.Password, opt => opt.MapFrom(s => s.Password))
            .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email))
            .ForMember(x => x.Id, opt => opt.MapFrom(_ => ObjectId.GenerateNewId()))
            .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.UserName))
            .ForMember(x => x.RegisterDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}