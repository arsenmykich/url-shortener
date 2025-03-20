using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace URLShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlService _urlService;

        private readonly UrlShorteningService _urlShorteningService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlsController(
            UrlService urlService,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _urlService = urlService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }


        [HttpGet]
        public IActionResult Get()
        {
            var urls = _urlService.GetAllUrls();
            return Ok(urls);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var url = _urlService.GetUrlById(id);
            return Ok(url);
        }


        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] UrlDTO urlDTO)
        //{
        //    // Get current user's ID
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    // Pass user ID to the service
        //    await _urlService.AddUrlAsync(urlDTO, userId);
        //    return Ok();
        //}
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UrlDTO urlDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //UrlDTO urlDTO = new UrlDTO();
            //urlDTO.OriginalUrl = OriginalUrl;
            urlDTO.CreatedById = userId; // Set the current user's ID
            urlDTO.CreatedDate = DateTime.UtcNow; // Set the current timestamp
            if (!Uri.TryCreate(urlDTO.OriginalUrl, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL");
            }
            urlDTO.Code = _urlService.GenerateUniqueCode();
            urlDTO.ShortenedUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                                  $"{_httpContextAccessor.HttpContext.Request.Host}/api/{urlDTO.Code}";

            _urlService.AddUrl(urlDTO);
            return Ok(urlDTO);
        }




        [HttpPut]
        public IActionResult Put([FromBody] UrlDTO urlDTO)
        {
            _urlService.UpdateUrl(urlDTO);
            return Ok();
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _urlService.DeleteUrl(id);
            return Ok();
        }
    }
}
