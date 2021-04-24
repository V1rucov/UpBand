using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace upband.Models
{
    public class UploadSongViewModel
    {
        [Required(ErrorMessage ="Загрузите mp3 файл")]
        public IFormFile SongFile { get; set; }
        public IFormFile SongLogo { get; set; }
        public string Lyrics { get; set; }
        public string Name { get; set; }
    }
}
