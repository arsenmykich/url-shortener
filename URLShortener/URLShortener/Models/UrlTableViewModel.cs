using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Collections.Generic;
using URLShortener.Models;

namespace URLShortener.ViewModels
{
    public class UrlTableViewModel
    {
        public IEnumerable<UrlDTO> Urls { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
        public string CurrentUserId { get; set; }
    }
}