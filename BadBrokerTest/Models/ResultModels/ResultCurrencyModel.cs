using BadBrokerTest.Models.Enums;
using System.Collections.Generic;

namespace BadBrokerTest.Models.ResultModels
{
    public class ResultCurrencyModel
    {
        public CurrencyEnum CurrencyEnum { get; set; }

        public string CurrenciesExchange { get; set; }

        public IList<ResultDayModel> Days { get; set; }

        public IList<ResultRowsModel> Rows { get; set; }

    }
}