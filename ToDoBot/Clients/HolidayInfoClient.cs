using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoBot.Models;
using ToDoBot.Consts;
using Newtonsoft.Json;

namespace ToDoBot.Clients
{
    public class HolidayInfoClient
    {
        private HttpClient _client;
        private string _address;
        public HolidayInfoClient()
        {
            _client = new HttpClient();
            _address = Constant.address;
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<HolidayInfo> GetHolidayInfoAsync(DateTime date, string country)
        {
            string[] dt = GetDate(date);
            string year = dt[2];
            string month = dt[1];
            string day = dt[0];
            var responce = await _client.GetAsync($"HolidayInfo/GetHolidayInfo?country={country}&year={year}&month={month}&day={day}");
            var content = responce.Content.ReadAsStringAsync().Result;
            if (content != null && content.Length != 0)
            {
                var result = JsonConvert.DeserializeObject<HolidayInfo>(content);
                return result;
            }
            else
                return null;
        }
        private string[] GetDate(DateTime date)
        {
            string d = date.ToString();
            d = d.Remove(10);
            string[] g = d.Split('.');
            return g;
        }
         
    }
}
