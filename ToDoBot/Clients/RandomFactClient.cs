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
    public class RandomFactClient
    {
        private HttpClient _client;
        private string _address;
        public RandomFactClient()
        {
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<RandomFactModel> GetRandomFactAsync()
        {
            var response = await _client.GetAsync("RandomFact/GetRandomFact");
            var content = response.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<RandomFactModel>(content);
                return result;
            }
            else
                return null;
        }
    }
}
