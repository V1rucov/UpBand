using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using upband.Data.Entities;

namespace upband.Data
{
    public class CompilationsMaker
    {
        private readonly DataBaseContext dataBaseContext;
        public CompilationsMaker(DataBaseContext _dataBaseContext) {
            dataBaseContext = _dataBaseContext;
        }
        public void Compile() {
            List<user> Users = dataBaseContext.users.Include(u => u.UserProfile).ThenInclude(p=>p.Subscriptions).ToList();
            List<artist> Artists = dataBaseContext.artists.Include(a=>a.Subscribers).ThenInclude(s=>s.Subscriptions).ToList();

            foreach (var cc in Artists) { 
                
                //foreach
            }
        }
    }
}
