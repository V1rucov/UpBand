using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace upband.Data.Entities
{
    public class user : IdentityUser
    {   
        public new int Id { get; set; }
        public artist ArtistProfile { get; set; }
        public profile UserProfile { get; set; }
    }
}
