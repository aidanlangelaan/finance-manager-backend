using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Data.Enums;

namespace FinanceManager.Data.Entities
{
    public class Account : EntityBase
    {
        [Column(TypeName = "varchar(255)")]
        public string Iban { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "tinyint")]
        public AccountType Type { get; set; } = AccountType.Expense;

        // Relations
        public virtual IEnumerable<Transaction> TransactionsFrom { get; set; }
        public virtual IEnumerable<Transaction> TransactionsTo { get; set; }


        public Account(string iban, string name, AccountType type = AccountType.Expense)
        {
            Iban = iban;
            Name = name;
            Type = type;
        }
    }
}
