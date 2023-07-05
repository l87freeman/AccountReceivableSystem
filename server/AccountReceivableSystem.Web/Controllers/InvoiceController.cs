using System.Collections.Generic;
using System.Threading.Tasks;
using AccountReceivableSystem.Application.Services.Abstractions;
using AccountReceivableSystem.Domain.Entities;
using AccountReceivableSystem.Infrastructure.Constants;
using AccountReceivableSystem.Web.Models.Request;
using AccountReceivableSystem.Web.Models.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountReceivableSystem.Web.Controllers;

[ApiController]
[Authorize(AuthConstants.AuthenticatedUserPolicyName)]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly IMapper _mapper;

    public InvoicesController(IInvoiceService invoiceService, IMapper mapper)
    {
        _invoiceService = invoiceService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest createInvoiceRequest)
    {
        var invoice = await _invoiceService.CreateInvoiceAsync(_mapper.Map<Invoice>(createInvoiceRequest), HttpContext.RequestAborted);
        return Created("", _mapper.Map<InvoiceResponse>(invoice));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice([FromRoute] string id, [FromBody] UpdateInvoiceRequest updateInvoiceRequest)
    {
        updateInvoiceRequest = updateInvoiceRequest with { Id = id };
        await _invoiceService.UpdateInvoiceAsync(_mapper.Map<UpdateInvoice>(updateInvoiceRequest), HttpContext.RequestAborted);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _invoiceService.GetInvoicesAsync(HttpContext.RequestAborted);
        return Ok(_mapper.Map<ICollection<InvoiceResponse>>(invoices));
    }
}