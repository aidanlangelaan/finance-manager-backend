﻿namespace FinanceManager.Api.Models;

public class LoginUserViewModel
{
    public string EmailAddress { get; init; } = null!;

    public string Password { get; init; } = null!;
}