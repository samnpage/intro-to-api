using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using IntroToApi.ConsoleApp.Models;

namespace IntroToApi.ConsoleApp
{
    public class SWAPIService
    {
        // Private field that we dont want to change "readonly".
        private readonly HttpClient _httpClient = new HttpClient();

        // Async method.
        public async Task<Person> GetPersonAsync(string url)
        {
            // // Get Request.
            // HttpResponseMessage response = await _httpClient.GetAsync(url);

            // if (response.IsSuccessStatusCode)
            // {
            //     // Was a success.
            //     Person person = await response.Content.ReadAsAsync<Person>();
            //     return person;
            // }

            // // Was not a success.
            // return null;
            return await GetAsync<Person>(url);
        }

        public async Task<Vehicle> GetVehicleAsync(string url)
        {
            // Get Request.
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Was a success.
                Vehicle person = await response.Content.ReadAsAsync<Vehicle>();
                return person;
            }

            // Was not a success.
            return null;
        }

        public async Task<T> GetAsync<T>(string url) where T: class
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                T content = await response.Content.ReadAsAsync<T>();
                return content;
            }

            return null;
        }

        public async Task<SearchResult<Person>> GetPersonSearchAsync(string query)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://swapi.dev/api/people/?search=" + query);

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<SearchResult<Person>>();

            return null;
        }

        public async Task<SearchResult<T>> GetSearchAsync<T>(string query, string category)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://swapi.dev/api/{category}?search={query}");

            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsAsync<SearchResult<T>>()
                : default;
        }

        public async Task<SearchResult<Vehicle>> GetVehicleSearchAsync(string query)
        {
            return await GetSearchAsync<Vehicle>(query, "vehicles");
        }
    }
}