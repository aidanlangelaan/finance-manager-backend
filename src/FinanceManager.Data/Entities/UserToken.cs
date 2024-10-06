using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public class UserToken : IdentityUserToken<Guid>
{
    [Required]
    [Column(TypeName = "longtext")]
    [ProtectedPersonalData]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? RefreshToken { get; set; }
    
    [Required]
    [Column(TypeName = "datetime")]
    public DateTime RefreshTokenExpiresOnAt { get; set; }
}