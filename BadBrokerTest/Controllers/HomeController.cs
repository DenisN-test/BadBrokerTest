using BadBrokerTest.Interfaces;
using BadBrokerTest.Models;
using BadBrokerTest.Services;
using System.Web.Mvc;

namespace BadBrokerTest.Controllers
{
    public class HomeController : Controller
    {
        private static readonly IExchangeApiService _exchangeApiService = new ExchangeApiService();

        public ActionResult Index() {
            return View(new InputModel());
        }

        [HttpPost]
        public ActionResult Index(InputModel inputModel) {
            ModelState.Clear();

            if (!TryValidateModel(inputModel))
                return View(inputModel);

            var resultModel = _exchangeApiService.GetExchangeResult(inputModel);

            return View("~/Views/Home/Result.cshtml", resultModel);
        }
    }
}