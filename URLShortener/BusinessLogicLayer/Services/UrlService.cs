using System;
using System.Collections.Generic;
using System.Linq;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.DTO;

using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace BusinessLogicLayer.Services
{
    public class UrlService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        private readonly AppDbContext _context;
        public UrlService(UnitOfWork unitOfWork, IMapper mapper, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;

        }
        public IEnumerable<UrlDTO> GetAllUrls()
        {
            var urls = _unitOfWork.UrlRepository.Get();
            return _mapper.Map<IEnumerable<UrlDTO>>(urls);
        }
        public UrlDTO GetUrlById(int id)
        {
            var url = _unitOfWork.UrlRepository.GetByID(id);
            return _mapper.Map<UrlDTO>(url);
        }
        public void AddUrl(UrlDTO urlDTO)
        {
            var url = _mapper.Map<Url>(urlDTO);
            _unitOfWork.UrlRepository.Insert(url);
            _unitOfWork.Save();
        }
        public async Task AddUrlAsync(UrlDTO urlDTO, string userId)
        {
            var url = _mapper.Map<Url>(urlDTO);

            // Auto-set properties
            url.CreatedById = userId; // Set the current user's ID
            url.CreatedDate = DateTime.UtcNow; // Set the current timestamp

            _unitOfWork.UrlRepository.Insert(url);
            await _unitOfWork.SaveAsync(); // Use SaveAsync for asynchronous saving
        }

        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int NumberOfCharsInShortLink = 7;
        private readonly Random _random = new();

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

                if (!_context.Urls.Any(s => s.Code == code))
                {
                    return code;
                }
            }

        }


        public void UpdateUrl(UrlDTO urlDTO)
        {
            var url = _mapper.Map<Url>(urlDTO);
            _unitOfWork.UrlRepository.Update(url);
            _unitOfWork.Save();
        }
        public void DeleteUrl(int id)
        {
            _unitOfWork.UrlRepository.Delete(id);
            _unitOfWork.Save();
        }
    }
}
