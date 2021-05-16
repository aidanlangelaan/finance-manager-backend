using System;
using CsvHelper.Configuration.Attributes;

namespace FinanceManager.Business.Services.Import
{
    public class CsvImportRabo
    {
        [Name("IBAN/BBAN")]
        public string Iban { get; set; }

        [Name("Datum")]
        public DateTime Date { get; set; }

        [Name("Bedrag")]
        public decimal Amount { get; set; }

        [Name("Saldo na trn")]
        public decimal BalanceAfter { get; set; }

        [Name("Tegenrekening IBAN/BBAN")]
        public string CounterPartyIban { get; set; }

        [Name("Naam tegenpartij")]
        public string CounterPartyName { get; set; }

        [Name("Omschrijving-1")]
        public string Description { get; set; }
    }
}
