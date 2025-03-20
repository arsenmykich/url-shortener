using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using Microsoft.EntityFrameworkCore;
using URLShortener.ViewModels;

namespace URLShortener.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AboutController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var aboutContent = await _context.AboutContents.FirstOrDefaultAsync();
            var isAdmin = User.IsInRole("Admin");

            var model = new AboutViewModel
            {
                Content = aboutContent?.Description ?? "Default description of the URL shortening algorithm.",
                IsAdmin = isAdmin
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string content)
        {
            var aboutContent = await _context.AboutContents.FirstOrDefaultAsync();
            if (aboutContent == null)
            {
                aboutContent = new AboutContent { Description = content };
                _context.AboutContents.Add(aboutContent);
            }
            else
            {
                aboutContent.Description = content;
                _context.AboutContents.Update(aboutContent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
