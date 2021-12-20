﻿using Microsoft.AspNetCore.Mvc;
using Tenda.Shared.BaseModels;

namespace Tenda.Shared;

public class DeleteRequest : RequestBase
{
    public string Id { get; set; }
    
    public DateTime ViewDate { get; set; }
}