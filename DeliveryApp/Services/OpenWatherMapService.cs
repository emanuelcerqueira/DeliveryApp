using System.Net.Http;
using System.Threading.Tasks;
using DeliveryApp.Models;
using DeliveryApp.Services.Models;
using DeliveryApp.Util;
using Newtonsoft.Json;

namespace DeliveryApp.Service
{
    public interface IOpenWatherMapService
    {
        Task<OpenWeatherMapResponse> FindWeatherInfoAsync(Location initialLocation);
    }

    public class OpenWatherMapService : IOpenWatherMapService
    {
        private readonly HttpClient _httpClient;

        public OpenWatherMapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenWeatherMapResponse> FindWeatherInfoAsync(Location location)
        {

            HttpResponseMessage response = await _httpClient
                .GetAsync(GetRequestUrlByLocation(location));
            response.EnsureSuccessStatusCode();
            string responseBody = await response?.Content?.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(responseBody); 
            return responseObject;
        }

        private string GetRequestUrlByLocation(Location location)
        {
            return $"?lat={location.Latitude}&lon={location.Longitude}&units=metric&appid={Constants.OPEN_WEATHER_MAP_API_KEY}";
        }

    }
}