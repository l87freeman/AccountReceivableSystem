using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Web.Models.Request;
using AccountReceivableSystem.Web.Models.Response;
using AutoMapper;
using MongoDB.Bson;

namespace AccountReceivableSystem.Web.Mappings;

public class InvoiceMappingProfile : Profile
{
    public InvoiceMappingProfile()
    {
        CreateMap<CreateInvoiceRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => ObjectId.GenerateNewId()))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.CustomerName))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(s => s.DueDate))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(s => s.InvoiceDate))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(s => s.InvoiceNumber))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.PayDate, opt => opt.Ignore())
            .ForMember(dest => dest.LineItems, opt => opt.MapFrom(s => s.LineItems))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Models.LineItem, Domain.Entities.LineItem>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.Description))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(s => s.Quantity))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(s => s.TotalPrice))
            .ReverseMap();

        CreateMap<Invoice, InvoiceResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.CustomerName))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(s => s.DueDate))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(s => s.IsPaid))
            .ForMember(dest => dest.PayDate, opt => opt.MapFrom(s => s.PayDate))
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(s => s.InvoiceDate))
            .ForMember(dest => dest.InvoiceNumber, opt => opt.MapFrom(s => s.InvoiceNumber))
            .ForMember(dest => dest.LineItems, opt => opt.MapFrom(s => s.LineItems));

        CreateMap<UpdateInvoiceRequest, UpdateInvoice>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(dest => dest.IsPaid, opt => opt.MapFrom(s => s.IsPaid))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
    }
}