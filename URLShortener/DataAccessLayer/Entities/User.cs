using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User : IdentityUser
    {
        public ICollection<Url> CreatedUrls { get; set; } = new List<Url>();
    }
}
