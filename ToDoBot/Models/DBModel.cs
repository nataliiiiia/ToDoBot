using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBot.Models
{
    public class DBModel 
    {
        public string UID { get; set; }
        public string Task { get; set; }
        public DBModel(string task, string Id)
        {
            Task = task;
            UID = Id;
        }
    }
    public class InfoModel
    {
        public string Country { get; set; }
        public string UID { get; set; }
        public string City { get; set; }
        public InfoModel(string id, string country, string city)
        {
            UID = id;
            City = city;
            Country = country;
        }
    }
    
    
   
}
