using System;
using System.Collections.Generic;

namespace BadBrokerTest.Models.Enums
{
    public class ApiModel
    {
        public DateTime? Start_at { get; set; }

        public CurrencyEnum Base { get; set; }

        public DateTime End_at { get; set; }

        public Dictionary<DateTime, Dictionary<CurrencyEnum, double>> Rates { get; set; }
    }
}