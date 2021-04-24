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
    public class ArtistController : Controller
    {
        private readonly DataBaseContext dataBaseContext;
        private readonly UserManager<user> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly RoleManager<IdentityRole> roleManager;
        public ArtistController(DataBaseContext _dataBaseContext, UserManager<user> _userManager, IWebHostEnvironment _webHostEnvironment, RoleManager<IdentityRole> _roleManager) {
            dataBaseContext = _dataBaseContext;
            userManager = _userManager;
            webHostEnvironment = _webHostEnvironment;
            roleManager = _roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Artist")]
        public async Task<IActionResult> Artist(string Name) {
            artist Artist = await dataBaseContext.artists.Where(a => a.BandName == Name).Include(a => a.Albums).Include(a => a.Subscribers).FirstOrDefaultAsync();
            if(Artist!=null)
                return View(Artist);
            return Redirect("/404");
        }
        [HttpGet]
        public IActionResult RegisterArtist() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterArtist(RegisterArtistViewModel model) {
            if (roleManager.FindByNameAsync("artist").Result == null) {
                await roleManager.CreateAsync(new IdentityRole("artist"));
            }
            if (ModelState.IsValid)
            {
                if (dataBaseContext.artists.Where(a => a.BandName == model.BandName).FirstOrDefault() == null) {
                    user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name)
                        .Include(u => u.ArtistProfile).FirstOrDefaultAsync();
                    if (User.ArtistProfile == null)
                    {
                        string FilePath = @"\static-files\";
                        var filestream1 = new FileStream(webHostEnvironment.WebRootPath+FilePath + model.BandName + "Logo.png", FileMode.Create); //webHostEnvironment.WebRootPath
                        await model.Logo.CopyToAsync(filestream1 );
                        filestream1.Close();
                        
                        artist Artist = new artist()
                        {
                            BandName = model.BandName,
                            LogoPath =  FilePath + model.BandName + "Logo.png"
                        };
                        User.ArtistProfile = Artist;
                        await dataBaseContext.artists.AddAsync(Artist);
                        await userManager.AddToRoleAsync(User,"artist");
                        return Redirect("/Home/Index");
                    }
                    else {
                        ModelState.AddModelError("BandName", "Только один исполнитель на аккаунт!");
                        return View(model);
                    }
                }
                ModelState.AddModelError("BandName", "Это название уже занято!");
                return View(model);
            }
            else {
                ModelState.AddModelError("BandName", "Что-то пошло не так...");
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadSong(UploadSongViewModel model) {
            if (ModelState.IsValid) {
                user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name).Include(u => u.ArtistProfile).FirstOrDefaultAsync();
                if (User.ArtistProfile != null && User.ArtistProfile.Songs.Where(s=>s.Name==model.Name)==null) {
                    string FilePath = @"\songs\";
                    await model.SongFile.CopyToAsync(new FileStream(webHostEnvironment.WebRootPath + FilePath + User.ArtistProfile.BandName +model.Name+"Song", FileMode.Create));
                    await model.SongLogo.CopyToAsync(new FileStream(webHostEnvironment.WebRootPath + FilePath + User.ArtistProfile.BandName + model.Name+"SongLogo", FileMode.Create));

                    song Song = new song() {Name = model.Name, Lyrics = model.Lyrics,
                        SongFilePath = webHostEnvironment.WebRootPath + FilePath + User.ArtistProfile.BandName + model.Name + "Song",
                        SongLogoPath = webHostEnvironment.WebRootPath + FilePath + User.ArtistProfile.BandName + model.Name + "SongLogo"
                    };
                    User.ArtistProfile.Songs.Add(Song);
                    await dataBaseContext.SaveChangesAsync();
                }
                ModelState.AddModelError("Name", "Что-то пошло не так...");
                return Redirect("/");
            }
            ModelState.AddModelError("Name", "Что-то пошло не так...");
            return Redirect("/");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAlbum(AddAlbumViewModel model) {
            if (ModelState.IsValid){
                user User = await dataBaseContext.users.Where(u => u.UserName == HttpContext.User.Identity.Name).Include(u => u.ArtistProfile).FirstOrDefaultAsync();
                if (User.ArtistProfile != null){
                    string FilePath = @"\static-files\";
                    await model.Logo.CopyToAsync(new FileStream(webHostEnvironment.WebRootPath + FilePath + model.Name + "AlbumLogo", FileMode.Create));
                    playlist Album = new playlist() { Name = model.Name, LogoPath=webHostEnvironment.WebRootPath + FilePath + model.Name + "AlbumLogo" };
                    User.ArtistProfile.Albums.Add(Album);
                    await dataBaseContext.playlists.AddAsync(Album);
                    await dataBaseContext.SaveChangesAsync();
                }
                ModelState.AddModelError("Name", "Что-то пошло не так...");
                return Redirect("/");
            }
            ModelState.AddModelError("Name", "Что-то пошло не так...");
            return Redirect("/");
        }
    }
}
