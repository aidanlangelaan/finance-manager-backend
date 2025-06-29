using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public abstract class EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ConcurrencyCheck]
    [Column("xmin")]
    public uint RowVersion { get; set; }
}
