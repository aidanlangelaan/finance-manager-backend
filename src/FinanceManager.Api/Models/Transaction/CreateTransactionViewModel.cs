﻿namespace FinanceManager.Api.Models;

public class CreateTransactionViewModel
{
    public double Amount { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }
}