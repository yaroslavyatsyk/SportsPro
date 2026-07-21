namespace SportsPro.Services
{
        public interface ICountryService
        {
            public Task<List<string>> GetCountriesAsync();
        }
    }
