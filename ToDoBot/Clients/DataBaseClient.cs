using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ToDoBot.Consts;
using ToDoBot.Models;


namespace ToDoBot.Clients
{
    public class DataBaseClient
    {
        
        private HttpClient _client;
        private HttpClient _client2;
        private static string _address;
        public DataBaseClient()
        {
            _client2 = new HttpClient();
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
            _client2.BaseAddress = new Uri(_address);
        }
        public async Task<InfoModel> GetInfo(string UID)
        {
            var responce = await _client2.GetAsync($"DB/GetInfo?id={UID}");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<InfoModel>(content);
                return result;
            }
            else
                return null;
        }
        public async Task PostInfo(string UID, string city, string country)
        {
            InfoModel infoModel = new(UID, city, country);
            var json = JsonConvert.SerializeObject(infoModel);
            HttpContent httpContent = new StringContent(json, Encoding.Default, "application/json");
            await _client2.PostAsync("DB/PostInfo", httpContent);
        }
        public async Task DeleteInfo(string UID)
        {
            InfoModel infoModel = new(UID, null, null);
            var json = JsonConvert.SerializeObject(infoModel);
            HttpContent httpContent = new StringContent(json, Encoding.Default, "application/json");
            await _client2.DeleteAsync($"DB/DeleteInfo/{httpContent}");

        }
        public async Task PostTodo(string task, string UID)
        {
            DBModel dB = new(task, UID);
            var json = JsonConvert.SerializeObject(dB);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("DB/PostTask", httpContent);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
        public async Task DeleteTodo(string task, string UID)
        {
            DBModel db = new(task, UID);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = JsonContent.Create(db),
                Method = HttpMethod.Delete,
                RequestUri = new Uri("DB/DeleteTask", UriKind.Relative)
            };
            var response = await _client.SendAsync(request);
        }
        public async Task<List<GetTasksModel>> GetAllTasks(string UID)
        {
            var response = await _client.GetAsync($"DB/GetAllTasks?id={UID}");
            var content = response.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<List<GetTasksModel>>(content);
                return result;
            }
            else
                return null;

        }

    }
}
