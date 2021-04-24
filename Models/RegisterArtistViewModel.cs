using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upband.Models
{
    public class RegisterArtistViewModel
    {
        public IFormFile Logo { get; set; }
        [Required]
        public string BandName { get; set; }
    }
}
