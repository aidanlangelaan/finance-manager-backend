namespace FinanceManager.Domain.Entities;

public abstract class EntityBase
{
    public int Id { get; set; }

    public uint RowVersion { get; set; }
}

