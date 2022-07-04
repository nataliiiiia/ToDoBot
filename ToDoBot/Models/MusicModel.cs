using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoBot.Models
{

    public class MusicModel
    {
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public class Artist
    {
        public string name { get; set; }
    }

    public class SongModel
    {
        public Tracks tracks { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public Artist artist { get; set; }
    }

    public class Tracks
    {
        public List<Track> track { get; set; }
    }
}
