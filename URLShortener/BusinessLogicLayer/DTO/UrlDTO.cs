using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class UrlDTO
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string? Code { get; set; }
        public string? ShortenedUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedById { get; set; }
        public string? CreatedByEmail { get; set; } 
    }
}
