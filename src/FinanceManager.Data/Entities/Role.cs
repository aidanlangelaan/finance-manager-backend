﻿using Microsoft.AspNetCore.Identity;
using System;

namespace FinanceManager.Data.Entities;

public class Role : IdentityRole<Guid>
{
    public Role() : base()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}