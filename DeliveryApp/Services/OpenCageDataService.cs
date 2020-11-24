using System;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryApp.Models;
using DeliveryApp.Services.Models;
using DeliveryApp.Util;
using Newtonsoft.Json;

namespace DeliveryApp.Service
{
    public interface IOpenCageDataService
    {
        Task<OpenCageDataResponse> ReverseGeocodingAsync(Location location);
    }

    public class OpenCageDataService : IOpenCageDataService
    {
        private readonly HttpClient _httpClient;

        public OpenCageDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenCageDataResponse> ReverseGeocodingAsync(Location location)
        {
            HttpResponseMessage response = await _httpClient
                .GetAsync(GetRequestUrlByLocation(location));
            response.EnsureSuccessStatusCode();
            string responseBody = await response?.Content?.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<OpenCageDataResponse>(responseBody); 

            return responseObject;
        }

        private string GetRequestUrlByLocation(Location location)
        {
            return $"/geocode/v1/json?key={Constants.OPEN_CAGE_DATA_API_KEY}&q={location.Latitude}%2C{location.Longitude}&pretty=1";
        }

    }
}