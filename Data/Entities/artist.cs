using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace upband.Data.Entities
{
    public class artist
    {
        public string LogoPath { get; set; }
        public artist() {
            Songs = new List<song>();
            Albums = new List<playlist>();
        }
        public int Id { get; set; }
        public string BandName { get; set; }
        public List<song> Songs { get; set; }
        public List<playlist> Albums { get; set; }
        public List<profile> Subscribers { get; set; } = new List<profile>();
    }
}