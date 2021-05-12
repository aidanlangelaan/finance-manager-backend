using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities
{
    public class Transaction : EntityBase
    {
        [Required]
        [Column(TypeName = "float")]
        public double Amount { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public double BalanceAfter { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int FromAccountId { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int ToAccountId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(7)")]
        public DateTime Date { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int CategoryId { get; set; }


        // Foreign keys
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
