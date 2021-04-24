using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace upband.Models
{
    public class AddAlbumViewModel
    {
        public IFormFile Logo { get; set; }
        public string Name { get; set; }
    }
}
