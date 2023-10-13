using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroToApi.ConsoleApp.Models;
using Newtonsoft.Json;

namespace IntroToApi.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();

            // Http response method.
            HttpResponseMessage response = httpClient.GetAsync("https://swapi.dev/api/people/1").Result;

            // Checks to see if the response was successful.
            if (response.IsSuccessStatusCode)
            {
                // Converts Http response into a C# object in two different ways.
                // First way (Newtonsoft Methods).
                // var content = response.Content.ReadAsStringAsync().Result;
                // var person = JsonConvert.DeserializeObject<Person>(content);

                // Second way (Microsft Web Api).
                Person luke = response.Content.ReadAsAsync<Person>().Result;
                Console.WriteLine(luke.Name);

                // Loop that iterates through each vehicle in our Person object and displays it to the console.
                foreach(string vehiclesUrl in luke.Vehicles)
                {
                    // Gets each vehicle and it's properties from our person object.
                    HttpResponseMessage vehicleResponse = httpClient.GetAsync(vehiclesUrl).Result;
                    // Console.WriteLine(vehicleResponse.Content.ReadAsStringAsync().Result);

                    // Applys each vehicle to an object.
                    Vehicle vehicle = vehicleResponse.Content.ReadAsAsync<Vehicle>().Result;
                    Console.WriteLine(vehicle.Name);
                }
            }

            Console.WriteLine();

            SWAPIService service = new SWAPIService();
            Person person = service.GetPersonAsync("https://swapi.dev/api/people/11").Result;

            if (person != null)
            {
                Console.WriteLine(person.Name);
                
                foreach(var vehicleUrl in person.Vehicles)
                {
                    var vehicle1 = service.GetVehicleAsync(vehicleUrl).Result;
                    Console.WriteLine(vehicle1.Name);
                }
            }

            Console.WriteLine();

            // var genericResponse = service.GetAsync<Vehicle>("https://swapi.dev/api/vehicles/4").Result;
            var genericResponse = service.GetAsync<Person>("https://swapi.dev/api/people/4").Result;

            if (genericResponse != null)
            {
                Console.WriteLine(genericResponse.Name);
            }
            else 
            {
                Console.WriteLine("Targeted object does not exist.");
            }

            Console.WriteLine();


            var person2Response = service.GetPersonAsync("https://swapi.dev/api/people/5").Result;

            if (person2Response != null)
            {
                Console.WriteLine(person2Response.Name);
            }
            else 
            {
                Console.WriteLine("Targeted object does not exist.");
            }

            Console.WriteLine();

            SearchResult<Person> skywalkers = service.GetPersonSearchAsync("skywalker").Result;
            foreach ( Person p in skywalkers.Results)
            {
                Console.WriteLine(p.Name);
            }

            Console.WriteLine();

            var genericSearch = service.GetSearchAsync<Vehicle>("speeder", "vehicles").Result;
            var genericSearchResult = genericSearch.Results[0].Name;
            Console.WriteLine(genericSearchResult);
            
            var vehicleSearch = service.GetVehicleSearchAsync("speeder").Result;
            var vehicleSearchResult = vehicleSearch.Results[1].Name;
            Console.WriteLine(vehicleSearchResult);
        }
    }
}