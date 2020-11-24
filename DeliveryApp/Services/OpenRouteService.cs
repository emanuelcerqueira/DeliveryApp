using System;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryApp.Models;
using DeliveryApp.Services.Models;
using DeliveryApp.Util;
using Newtonsoft.Json;

namespace DeliveryApp.Service
{
    public interface IOpenRouteService
    {
        Task<OpenRouteServiceResponse> findRoutingInfoAsync(Location initialLocation, Location deliveryLocation);
        
    }

    public class OpenRouteService : IOpenRouteService
    {
        private readonly HttpClient _httpClient;

        public OpenRouteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<OpenRouteServiceResponse> findRoutingInfoAsync(Location initialLocation, Location endLocation)
        {
            HttpResponseMessage response = await _httpClient
                .GetAsync(GetRequestUrlByInitialAndEndLocation(initialLocation, endLocation));
            response.EnsureSuccessStatusCode();
            string responseBody = await response?.Content?.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<OpenRouteServiceResponse>(responseBody); 

            return responseObject;
        }

        private string GetRequestUrlByInitialAndEndLocation(Location initialLocation, Location endLocation)
        {
            return $"?api_key={Constants.OPEN_ROUTE_SERVICE_API_KEY}&start={initialLocation.Longitude},{initialLocation.Latitude}&end={endLocation.Longitude},{endLocation.Latitude}";
        }
    }
}