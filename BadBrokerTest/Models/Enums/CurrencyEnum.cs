using System.ComponentModel.DataAnnotations;

namespace BadBrokerTest.Models.Enums
{
    public enum CurrencyEnum
    {
        [Display(Name = "USD")]
        USD = 0,

        [Display(Name = "RUB")]
        RUB = 1,

        [Display(Name = "EUR")]
        EUR = 2,

        [Display(Name = "GBP")]
        GBP = 3,

        [Display(Name = "JPY")]
        JPY = 4
    }
}