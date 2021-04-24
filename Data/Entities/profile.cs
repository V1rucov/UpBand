using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace upband.Data.Entities
{
    public class profile
    {
        public int Id { get; set; }
        public List<artist> Subscriptions { get; set; } = new List<artist>();
        public List<playlist> Playlists { get; set; } = new List<playlist>();
        public List<song> FavoriteSongs { get; set; } = new List<song>();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
