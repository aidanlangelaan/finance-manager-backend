﻿namespace FinanceManager.Api.Models;

public class GetTransactionViewModel
{
    public int Id { get; set; }

    public double Amount { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public int CategoryId { get; set; }
}