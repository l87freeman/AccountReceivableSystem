﻿namespace IdentityServer.Web.Models.Request;

public class RegisterRequest
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
}