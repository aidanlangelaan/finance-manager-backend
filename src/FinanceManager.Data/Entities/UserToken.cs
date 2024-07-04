using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public class UserToken : IdentityUserToken<Guid>
{
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string AccessTokenExpiresOnAt { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime RegisteredOnAt { get; set; } = DateTime.UtcNow;
}