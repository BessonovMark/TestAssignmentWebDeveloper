using Countries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Countries.Services
{
    public interface ICountryService {
        public Task<Country> GetCountryById(int Id);
        public Task<IList<Country>> GetCountryList();
        public Task<IList<Country>> GetCountryByName(string countryName);
    }
    public class CountryService : ICountryService
    {
        private List<Country> _countries = new List<Country>();

        private readonly IMemoryCache _cache;
        private const string _url = "https://restcountries.com/v3.1/all";
        public CountryService(IMemoryCache cache) {
            this._cache = cache;
        }
        async public Task<Country> GetCountryById(int Id) {
            IList <Country>? result;
            _cache.TryGetValue("CountryList", out result);
            if (result == null) {
                result = await GetCountryList();
                _cache.TryGetValue("CountryList", out result);
            }
            return result.Where(country => country.Id == Id).First();
        }
        async public Task<IList<Country>> GetCountryByName(string countryName) {
            IList<Country>? result;
            _cache.TryGetValue("CountryList", out result);
            if (result == null) {
                result = await GetCountryList();
            }
            return result.Where(country => country.Name.ToLower() == countryName.ToLower()).ToList();
        }
        async public Task<IList<Country>> GetCountryList() {
            IList<Country>? results;

            _cache.TryGetValue("CountryList", out results);
            if (results != null) return results.OrderBy(country => country.Name).ToList();

            try {
                string jsonRespone;
                using (var client = new HttpClient()) {
                    using var response = await client.GetAsync(_url);
                    if (!response.IsSuccessStatusCode) {
                        throw new Exception("Something went wrong!");
                    }
                    using (StreamReader sr = new StreamReader(response.Content.ReadAsStream())) {
                        jsonRespone = sr.ReadToEnd();
                    }
                }

                JArray serverResponse = JArray.Parse(jsonRespone);
                results = serverResponse
                    .Select((country, index) => new Country {
                        Id = index,
                        Name = country["name"]["common"]!.ToString(),
                        Capital = country["capital"]?.Select(capital => capital.ToString()).ToList(),
                        Region = country["region"]!.ToString(),
                        Languages = country["languages"]?.Select(language => language.First.ToString()).ToList(),
                    }).ToList();

                _cache.Set("CountryList", results, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                return results.OrderBy(country => country.Name).ToList();
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return Array.Empty<Country>();
            }

        }
    }
}
