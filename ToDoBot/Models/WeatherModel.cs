using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBot.Models
{
    public class WeatherModel
    {
        public List<List> List { get; set; }

        public City City { get; set; }
    }
    public class List
    {
        public Main Main { get; set; }
        
        public List<Weather> Weather { get; set; }
        
        public Wind Wind { get; set; }

    }
    public class City
    {
        public string Sunrise { get; set; }

        public string Sunset { get; set; }
    }
    public class Main
    {
        public double Temp { get; set; }
        
        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }
    public class Weather
    {
        public string Description { get; set; }
    }
    public class Wind
    {
        public double Speed { get; set; }
    }
}
