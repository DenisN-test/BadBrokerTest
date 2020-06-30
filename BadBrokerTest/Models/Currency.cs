using BadBrokerTest.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BadBrokerTest.Models
{
    public class Currency
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime? Date { get; set; }

        public CurrencyEnum SellCurrency { get; set; }

        public CurrencyEnum BuyCurrency { get; set; }

        public double ExchangeRate { get; set; }
    }
}