using Newtonsoft.Json;
using System.Net;

namespace SportsPro.Services
{
    public class CountryService : ICountryService
    {
        public async Task<List<string>> GetCountriesAsync()
        {
            using var client = new WebClient();
            var data = await client.DownloadStringTaskAsync(
                "https://restcountries.com/v3.1/all?fields=name,capital,currencies");

            dynamic json = JsonConvert.DeserializeObject(data);
            var listOfCountries = new List<string>();

            foreach (var jsonObject in json)
            {
                if ((string)jsonObject.name.common != "Russia")
                    listOfCountries.Add((string)jsonObject.name.common);
            }

            listOfCountries.Sort();
            return listOfCountries;
        }
    }
}
