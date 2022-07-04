using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBot.Models
{
    public class GetTasksModel
    {
        public ID ID { get; set; }
        public Task0 task { get; set; }
    }
    public class Task0
    {
        public string s { get; set; }
    }

    public class ID
    {
        public string s { get; set; }
    }
}
