using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class AuditableEntity : EntityBase
{
    [Required]
    public DateTime CreatedOnAt { get; set; }

    [Required]
    public DateTime UpdatedOnAt { get; set; }
    
    
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}