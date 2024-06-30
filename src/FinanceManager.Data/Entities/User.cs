using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public class User : IdentityUser<Guid>
{
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; }

    [Required]
    [Column(TypeName = "datetime2(7)")]
    public DateTime RegisteredOnAt { get; set; } = DateTime.UtcNow;
}