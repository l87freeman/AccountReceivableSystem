using System.Net;
using AccountReceivableSystem.Web.Models;

namespace AccountReceivableSystem.IntegrationTests.Model;

public record ErrorResponseModel(ErrorModel Error, HttpStatusCode StatusCode);