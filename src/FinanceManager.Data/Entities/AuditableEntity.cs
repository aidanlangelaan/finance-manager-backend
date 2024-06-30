using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public abstract class AuditableEntity : EntityBase
{
    [Required]
    [Column(TypeName = "datetime2(7)")]
    public DateTime CreatedOnAt { get; set; }

    [Required]
    [Column(TypeName = "datetime2(7)")]
    public DateTime UpdatedOnAt { get; set; }
}