using BadBrokerTest.DataContext;
using BadBrokerTest.Interfaces;
using BadBrokerTest.Models;
using BadBrokerTest.Models.Enums;
using BadBrokerTest.Models.Repositories;
using BadBrokerTest.Models.ResultModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BadBrokerTest.Services
{
    public class ExchangeApiService : IExchangeApiService
    {
        private readonly IHttpService _httpService = new HttpService();
        private readonly IRepository<Currency> _currencyRepository = new Repository<Currency>(new TestDataContext());
        private static readonly string _siteUrl = "https://api.exchangeratesapi.io/";

        public ResultModel GetExchangeResult(InputModel model) {
            var resultModel = ConvertToResultModel(model, model.MoneyAmount);
            return resultModel;
        }

        private string PrepareUrl(InputModel model) {
            var start = model.StartDate?.ToString("yyyy-MM-dd");
            var end = model.EndDate?.ToString("yyyy-MM-dd");
            var currency = model.CurrencyEnum;
            var symbols = EnumService.GetAllCurrenciesFromEnum(currency);
            return $"{_siteUrl}history?start_at={start}&end_at={end}&base={currency}&symbols={symbols}";
        }

        private ApiModel GetResultModelFromRequest(InputModel model) {
            try {
                var url = PrepareUrl(model);
                var result = _httpService.GetResponse(url);
                return JsonConvert.DeserializeObject<ApiModel>(result);
            } catch (Exception ex) {
                //logger should be here
                throw new HttpException(500, "An error has occurred");
            }
        }

        private (DateTime? StartDate, DateTime? EndDate) CheckDates(DateTime? startDate, DateTime? endDate, List<Currency> currencies) {
            if (!startDate.HasValue || !endDate.HasValue)
                return (startDate, endDate);

            var dates = Enumerable.Range(0, (endDate - startDate).Value.Days + 1)
                .Select(d => startDate.Value.AddDays(d))
                .Where(d => !new[] { DayOfWeek.Sunday, DayOfWeek.Saturday }.Contains(d.DayOfWeek)) //no datas for that days
                .ToList();

            var storedDays = currencies.Where(c => c.Date.HasValue).Select(c => c.Date.Value).ToList();

            var except = dates.Except(storedDays).OrderBy(d => d.Date).ToList();

            if (except.Any())
                return (except.First(), except.Last());

            return (null, null);
        }

        private ResultModel ConvertToResultModel(InputModel model, double money) {

            var currenciesList = _currencyRepository.GetAll()
                .Where(r => r.Date >= model.StartDate && r.Date <= model.EndDate)
                .ToList();

            var dates = CheckDates(model.StartDate, model.EndDate, currenciesList);

            if (dates != (null, null)) {
                model.StartDate = dates.StartDate;
                model.EndDate = dates.EndDate;
                var apiModel = GetResultModelFromRequest(model);

                if (apiModel?.Rates != null) {
                    var currency = apiModel.Base;
                    var days = apiModel.Rates
                        .SelectMany(d => d.Value
                        .Select(c => new Currency {
                            Date = d.Key,
                            SellCurrency = currency,
                            BuyCurrency = c.Key,
                            ExchangeRate = c.Value
                        }))
                        .OrderBy(c => c.Date)
                        .ToList();

                    var daysToAdd = days
                        .Where(d => currenciesList
                                .All(c => d.Date != c.Date
                                && d.SellCurrency != c.SellCurrency
                                && d.BuyCurrency != c.BuyCurrency))
                        .ToList();

                    Task.Factory.StartNew(() => {
                        foreach (var day in daysToAdd)
                            _currencyRepository.Create(day);
                        _currencyRepository.SaveChanges();
                    });

                    currenciesList.AddRange(days);
                }
            }

            var groups = currenciesList.GroupBy(c => c.BuyCurrency);
            var currencies = groups.Select(g => {
                var rows = g.SelectMany(day => {
                    var availableDays = g.Where(d => d.Date > day.Date).ToList();
                    var dayRows = availableDays.Select(d => {
                        var fee = (d.Date - day.Date)?.Days;
                        var result = (day.ExchangeRate / d.ExchangeRate * money) - fee;

                        return new ResultRowsModel {
                            Profit = (double)result - money,
                            Detalization = $"{day.ExchangeRate:0.##} * {money} / {d.ExchangeRate:0.##} - {fee} (days broker fee) = {result:0.##}"
                        };
                    });

                    return dayRows;
                }).ToList();

                var resultDays = g.Select(day => new ResultDayModel {
                    Date = day.Date,
                    Exchange = day.ExchangeRate.ToString("0.##")
                }).ToList();

                return new ResultCurrencyModel {
                    CurrencyEnum = g.Key,
                    CurrenciesExchange = $"{model.CurrencyEnum}/{g.Key}",
                    Days = resultDays,
                    Rows = rows
                };
            }).ToList();

            return new ResultModel {
                Currencies = currencies
            };
        }
    }
}