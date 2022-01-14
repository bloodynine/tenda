﻿using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Shared;

public abstract class FinancialTransactionRequestBase : RequestBase
{
    public string Name { get; set; } = "";
    public decimal Amount { get; set; } = 0;
    public DateTime Date { get; set; } = new();

    public List<string> Tags { get; set; } = new();
    public abstract FinancialTransaction ToTransaction();
}

public class FinancialTransactionRequestBaseValidator : Validator<FinancialTransactionRequestBase>
{
    public FinancialTransactionRequestBaseValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        Include(new RequestBaseValidator());
    }
}