﻿namespace FinanceManager.Business.Services.Models;

public class LoginUserDTO
{
    public string EmailAddress { get; init; } = null!;

    public string Password { get; init; } = null!;
}