using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace upband.Data.Entities
{
    public class song
    {
        public string SongFilePath { get; set; }
        public string SongLogoPath { get; set; }
        public int Id { get; set; }
        public string Lyrics { get; set; }
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public artist Artist { get; set; }
    }
}
