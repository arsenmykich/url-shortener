using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services;
using URLShortener.ViewModels;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlService _urlService;

        public HomeController(UrlService urlService)
        {
            _urlService = urlService;
        }

        public IActionResult Index()
        {
            var urls = _urlService.GetAllUrls();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            var isAdmin = User.IsInRole("Admin");

            var viewModel = new UrlTableViewModel
            {
                Urls = urls,
                IsAuthenticated = User.Identity.IsAuthenticated,
                IsAdmin = isAdmin,
                CurrentUserId = userId // Populate the CurrentUserId property
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUrl(string originalUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var urlDTO = new UrlDTO
            {
                OriginalUrl = originalUrl,
                Code = _urlService.GenerateUniqueCode(),
                ShortenedUrl = $"{Request.Scheme}://{Request.Host}/{_urlService.GenerateUniqueCode()}",
                CreatedById = userId,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                await _urlService.AddUrlAsync(urlDTO, userId);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            try
            {
                await _urlService.DeleteUrlAsync(id, userId, isAdmin);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
        [Authorize] // Restrict access to authenticated users
        public IActionResult Details(int id)
        {
            var url = _urlService.GetUrlById(id);
            if (url == null)
            {
                return NotFound(); // Return 404 if the URL is not found
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            var viewModel = new UrlDetailsViewModel
            {
                Url = url,
                IsAuthenticated = User.Identity.IsAuthenticated,
                IsAdmin = isAdmin
            };

            return View(viewModel);
        }
    }
}