using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class AuditableEntity : EntityBase
{
    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedOnAt { get; set; }

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedOnAt { get; set; }
    
    [Column(TypeName = "uuid")]
    public Guid? CreatedById { get; set; }
    
    [Column(TypeName = "uuid")]
    public Guid? UpdatedById { get; set; }
    
    // Foreign keys
    public User? CreatedBy { get; set; }
    
    public User? UpdatedBy { get; set; }
}