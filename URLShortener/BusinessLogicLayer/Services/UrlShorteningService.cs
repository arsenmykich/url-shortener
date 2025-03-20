using DataAccessLayer.Data;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class UrlShorteningService
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int NumberOfCharsInShortLink = 7;
        private readonly Random _random = new();
        private readonly AppDbContext _context;
        public string GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharsInShortLink];
            while (true)
            {

                for (int i = 0; i < NumberOfCharsInShortLink; i++)
                {
                    var randomIndex = _random.Next(chars.Length - 1);
                    codeChars[i] = chars[randomIndex];
                }

                string code = new string(codeChars);

                if (!_context.Urls.Any(s => s.ShortenedUrl == code))
                {
                    return code;
                }
            }

        }
    }
}
