﻿using MongoDB.Entities;
using Tenda.Shared.BaseModels;

namespace Tenda.Shared.Models;

public class FinancialTransaction : Entity
{
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public bool IsResolved { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; }
    public string? AssociatedRepeatId { get; set; }
    public bool IsRepeating => !string.IsNullOrEmpty(AssociatedRepeatId);
    public List<string> Tags { get; set; } 

    public FinancialTransaction(string name, decimal amount, DateTime date, bool isResolved, TransactionType type,
        string userId, List<string>? tags)
    {
        Name = name;
        Amount = amount;
        IsResolved = isResolved;
        Type = type;
        UserId = userId;
        Date = date;
        Tags = tags ?? new List<string>();
    }

    public FinancialTransaction(FinancialTransactionRequestBase request, bool isResolved, TransactionType type)
    {
        Name = request.Name;
        Amount = request.Amount.GetValueOrDefault();
        IsResolved = isResolved;
        Date = request.Date;
        UserId = request.UserId;
        Type = type;
        Tags = request.Tags;
    }

    public FinancialTransaction(RepeatContracts contract, DateTime date, string associatedRepeatId)
    {
        Name = contract.Name;
        Amount = contract.Amount;
        IsResolved = false;
        Date = date;
        Type = contract.Type;
        UserId = contract.UserId;
        AssociatedRepeatId = associatedRepeatId;
        Updated = DateTime.Now;
        Created = DateTime.Now;
        Tags = contract.Tags.ToList();
    }

    public FinancialTransaction()
    {}
}

public enum TransactionType
{
    OneOff = 1,
    Bill = 2,
    Income = 3
}