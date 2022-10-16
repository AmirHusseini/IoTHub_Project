using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WPFApp.MVVM.Models;

namespace WPFApp.Services
{
    internal interface IGetCurrentWeatherOutside
    {
        
        public Task<WeatherResponse> GetWeatherDataAsync();
    }
    internal class GetCurrentWeatherOutside : IGetCurrentWeatherOutside
    {
        public async Task<WeatherResponse> GetWeatherDataAsync()
        {
            string uri = "https://api.openweathermap.org/data/2.5/weather?lat=59.334591&lon=18.063240&appid=7cfd9147cae9a58ff400c4fb14076490";
            
            try
            {
                using var client = new HttpClient();
                var response = await client.GetFromJsonAsync<WeatherApiResponse>(uri);
                return new WeatherResponse
                {
                    Temperature = (int)response!.main.temp,
                    Humidity = response.main.humidity,
                    WeatherCondition = response.weather[0].main,
                    Icon = response.weather[0].icon
                };
            }
            catch { }
            return null!;
        }
    }
}
