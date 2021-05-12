using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities
{
    public class Account : EntityBase
    {
        [Column(TypeName = "varchar(255)")]
        public string Iban { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        // Relations
        public virtual IEnumerable<Transaction> TransactionsFrom { get; set; }
        public virtual IEnumerable<Transaction> TransactionsTo { get; set; }
    }
}
