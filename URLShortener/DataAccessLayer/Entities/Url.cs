using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Url
    {
        public int Id { get; set; }

        [Required]
        [Url]
        public string OriginalUrl { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        public string ShortenedUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string CreatedById { get; set; }

        public User CreatedBy { get; set; }
    }
}
