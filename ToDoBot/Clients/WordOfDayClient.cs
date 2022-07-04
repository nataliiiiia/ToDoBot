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
    public class WordOfDayClient
    {
        private HttpClient _client;
        private string _address;
        public WordOfDayClient()
        {
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<WordOfDayModel> GetWordAsync()
        {
            var responce = await _client.GetAsync($"WordOfDay");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<WordOfDayModel>(content);
                return result;
            }
            else
                return null;
        }
    }
}
