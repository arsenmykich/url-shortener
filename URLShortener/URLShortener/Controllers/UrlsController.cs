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
        private readonly UserManager<User> _userManager;

        public UrlsController(
            UrlService urlService,
            UserManager<User> userManager)
        {
            _urlService = urlService;
            _userManager = userManager;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UrlDTO urlDTO)
        {
            // Get current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Pass user ID to the service
            await _urlService.AddUrlAsync(urlDTO, userId);
            return Ok();
        }
        [HttpPut]
        public IActionResult Put([FromBody] UrlDTO urlDTO)
        {
            _urlService.UpdateUrl(urlDTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _urlService.DeleteUrl(id);
            return Ok();
        }
    }
}
