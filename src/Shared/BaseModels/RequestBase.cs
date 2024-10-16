﻿using System.Text.Json.Serialization;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace Tenda.Shared.BaseModels;

public abstract class RequestBase
{
    [FromClaim("UserId")]
    [JsonIgnore]
    public string UserId { get; set; } = "";

    [JsonIgnore]
    [FromClaim("SeedId")]
    public string SeedId { get; set; } = "";
}

public class RequestBaseValidator : Validator<RequestBase>{
    public RequestBaseValidator()
    {
        RuleFor(x => x.SeedId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}