using BadBrokerTest.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BadBrokerTest.Models
{
    public class InputModel
    {
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Amount of money in USD")]
        [Range(1, 1000000000)]
        [Required]
        public double MoneyAmount { get; set; }

        // default USD
        public CurrencyEnum CurrencyEnum { get => CurrencyEnum.USD; }
    }
}