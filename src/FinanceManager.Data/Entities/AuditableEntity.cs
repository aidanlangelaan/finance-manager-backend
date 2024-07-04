using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public abstract class AuditableEntity : EntityBase
{
    [Required]
    [Column(TypeName = "datetime")]
    public DateTime CreatedOnAt { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime UpdatedOnAt { get; set; }
}