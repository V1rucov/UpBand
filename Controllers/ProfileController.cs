using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using upband.Models;
using upband.Data;
using upband.Data.Entities;

namespace upband.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly DataBaseContext dataBaseContext;
        private readonly UserManager<user> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProfileController(UserManager<user> _userManager, DataBaseContext _dataBaseContext, IWebHostEnvironment _webHostEnvironment) {
            dataBaseContext = _dataBaseContext;
            userManager = _userManager;
            webHostEnvironment = _webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Playlists(string UserName) {
            if (UserName != null) {
                List<playlist> Playlists = dataBaseContext.users.Where(u => u.UserName == UserName)
                .Include(u => u.UserProfile).ThenInclude(p => p.Playlists).FirstOrDefaultAsync().Result.UserProfile.Playlists;
                if (Playlists != null)
                    return View(Playlists);
                else return Redirect("/404");
            }
            else return Redirect("/404");
        }
        [HttpGet]
        public async Task<IActionResult> Playlist(int Id)
        {
            if (Id != 0)
            {
                playlist Playlist = await dataBaseContext.playlists.Where(p => p.Id == Id).Include(p => p.Owner).Include(p => p.Songs).FirstOrDefaultAsync();
                if (HttpContext.User.Identity.Name ==  Playlist.Owner.UserName) {
                    ViewData["IsOwner"] = "true";
                }
                else ViewData["IsOwner"] = "false";

                if (Playlist != null)
                    return View(Playlist);
                else return Redirect("/404");
            }
            else return Redirect("/404");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePlayList(string PlaylistName) {
            if (PlaylistName != null) {
                user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name).Include(u => u.UserProfile)
                    .ThenInclude(p=>p.Playlists).FirstOrDefaultAsync();
                playlist Playlist = new playlist() { Name = PlaylistName, Owner = User };
                User.UserProfile.Playlists.Add(Playlist);
                await dataBaseContext.playlists.AddAsync(Playlist);
                await dataBaseContext.SaveChangesAsync();
                return Redirect("/Profile/Playlists?UserName="+HttpContext.User.Identity.Name);
            }
            else return Redirect("/404");
        }
        [HttpGet]
        public IActionResult CustomizePlaylist(int PlaylistId)
        {
            ViewData["PlaylistId"] = PlaylistId.ToString();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CustomizePlaylist(CustomizePlaylistViewModel model) {
            playlist P = await dataBaseContext.playlists.Where(p => p.Id == model.PlaylistId).FirstOrDefaultAsync();
            if (dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name).Include(u=>u.UserProfile.Playlists).FirstOrDefault().UserProfile.Playlists.Contains(P))
            {
                if (model.Logo != null) {
                    string FilePath = @"\static-files\";
                    var filestream1 = new FileStream(webHostEnvironment.WebRootPath + FilePath + model.PlaylistId + "PlaylistLogo.png", FileMode.Create); //webHostEnvironment.WebRootPath
                    await model.Logo.CopyToAsync(filestream1);
                    filestream1.Close();
                    P.LogoPath = FilePath + model.PlaylistId + "PlaylistLogo.png";
                }
                P.Name = model.Name;
                P.Description = model.Description;
                await dataBaseContext.SaveChangesAsync();
                return Redirect("/");
            }
            else return Redirect("/404");
        }
        [HttpPost]
        public async Task<IActionResult> AddToFavorite(string SongId) {
            if (SongId != null)
            {
                int sId = int.Parse(SongId);
                song Song = await dataBaseContext.songs.Where(s => s.Id == sId).FirstOrDefaultAsync();
                user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name).Include(u => u.UserProfile).FirstOrDefaultAsync();
                if (Song != null && User != null)
                {
                    User.UserProfile.FavoriteSongs.Add(Song);
                    await dataBaseContext.SaveChangesAsync();
                    return Redirect("/");
                }
            }
            return Redirect("/");
        }
        [HttpPost]
        public async Task<IActionResult> AddToPlaylist(int PlaylistId, int SongId) {
            user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name)
                .Include(u => u.UserProfile.Playlists.Where(p => p.Id == PlaylistId)).FirstOrDefaultAsync();
            if (User.UserProfile.Playlists != null)
            {
                song Song = await dataBaseContext.songs.Where(s => s.Id == SongId).FirstOrDefaultAsync();
                User.UserProfile.Playlists[0].Songs.Add(Song);
                await dataBaseContext.SaveChangesAsync();
                return Redirect("/Profile/Playlist?Id=" + PlaylistId);
            }
            else {
                return Redirect("/");
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> Artists(string UserName) {
            user User = await dataBaseContext.users.Where(u => u.UserName == UserName).Include(u => u.UserProfile).FirstOrDefaultAsync();
            if(User!=null)
                return View(User);
            return Redirect("/404");
        }
        [HttpPost]
        public async Task<IActionResult> CustomizeProfile(CustomizeProfileViewModel model)
        {
            DateTime dateOfBirth = Convert.ToDateTime(model.DateOfBirth);
            if (ModelState.IsValid)
            {
                if (DateTime.Now.Year - dateOfBirth.Year >= 16)
                {
                    user User = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    User.UserProfile = new profile()
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Gender = model.Gender,
                        DateOfBirth = dateOfBirth.ToString("dd//MM/YYYY")
                    };
                    await dataBaseContext.SaveChangesAsync();
                    return View("/Account/Settings");
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "Вы должны быть старше 16 лет");
                    return View("/Account/Settings", model);
                }
            }
            else return View("/Account/Settings", model);
        }
    }
}