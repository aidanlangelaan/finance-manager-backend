using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class AuditableEntity : EntityBase
{
    public DateTime CreatedOnAt { get; set; }

    public DateTime UpdatedOnAt { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    // Foreign keys
    public User? CreatedBy { get; set; }

    public User? UpdatedBy { get; set; }
}
