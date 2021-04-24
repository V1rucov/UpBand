using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace upband.Data.Entities
{
    public class playlist
    {
        public string LogoPath { get; set; }
        public int Id { get; set; }
        public playlist() {
            Songs = new List<song>();
        }
        public List<song> Songs { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public profile Owner { get; set; }
    }
}
