using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class AuditableEntity : EntityBase
{
    [Required]
    [Column(TypeName = "datetime")]
    public DateTime CreatedOnAt { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime UpdatedOnAt { get; set; }
    
    [Column(TypeName = "int")]
    public int? CreatedById { get; set; }
    
    [Column(TypeName = "int")]
    public int? UpdatedById { get; set; }
    
    // Foreign keys
    [ForeignKey("CreatedById")]
    public User? CreatedBy { get; set; }
    
    [ForeignKey("UpdatedById")]
    public User? UpdatedBy { get; set; }
}