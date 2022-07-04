using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBot.Consts;
using ToDoBot.Models;

namespace ToDoBot.Clients
{
    public class WeatherClient
    {
        private HttpClient _client;
        private string _address;
        public WeatherClient()
        {
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<WeatherModel> GetWeatherByCity(string city)
        {
            var responce = await _client.GetAsync($"Weather/GetWeather?city={city}");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<WeatherModel>(content);
                return result;
            }
            else
                return null;
        }
    }
}
