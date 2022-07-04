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
    public class MusicClient
    {
        private HttpClient _client;
        private HttpClient _client2;
        private string _address;
        public MusicClient()
        {
            _client2 = new HttpClient();
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
            _client2.BaseAddress = new Uri(_address);
        }
        public async Task<MusicModel> GetFirstChartSongUri()
        {
            var responce = await _client.GetAsync($"TrackOfDay/GetSongUri");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<MusicModel>(content);
                return result;
            }
            else
                return null;
        }
        public async Task<SongModel> GetFirstChartSong()
        {
            var responce = await _client.GetAsync($"TrackOfDay/GetSong");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<SongModel>(content);
                return result;
            }
            else
                return null;
        }

    }
}
