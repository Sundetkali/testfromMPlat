using HtmlAgilityPack;
using System.Globalization;

namespace testfromMPlat.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<decimal> GetUsdToKztAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var usdRow = doc.DocumentNode.SelectSingleNode("//td[text()='USD']/parent::tr");
            if (usdRow != null)
            {
                var rateCell = usdRow.SelectSingleNode("./td[4]");
                if (rateCell != null)
                {
                    var rateText = rateCell.InnerText.Trim();
                    rateText = rateText.Replace(",", ".");
                    if (decimal.TryParse(rateText, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    {
                        return rate;
                    }
                    throw new Exception($"Невозможно преобразовать '{rateText}' в число.");
                }
                throw new Exception("Не найдена ячейка с курсом.");
            }
            throw new Exception("Не найдена строка с USD.");
        }
    }
}