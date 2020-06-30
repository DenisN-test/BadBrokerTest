using BadBrokerTest.Models;
using BadBrokerTest.Models.ResultModels;

namespace BadBrokerTest.Interfaces
{
    interface IExchangeApiService
    {
        ResultModel GetExchangeResult(InputModel model);
    }
}
