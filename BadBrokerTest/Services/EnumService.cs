using System;
using System.Linq;

namespace BadBrokerTest.Services
{
    public class EnumService
    {
        public static string GetAllCurrenciesFromEnum<T>(params T[] excludeEnum) where T : struct {
            var enums = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Where(e => !excludeEnum.Contains(e))
                .Select(e => e.ToString())
                .ToArray();
            return string.Join(",", enums);
        }
    }
}