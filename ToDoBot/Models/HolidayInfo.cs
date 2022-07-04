using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBot.Models
{
    public class HolidayInfo
    {
        public Responce Response { get; set; }
    }
    public class Responce
    {
        public List<Holiday> Holidays { get; set; }
    }
    public class Holiday
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Date Date { get; set; }
    }
    public class Date
    {
        public string Iso { get; set; }
    }
}
