using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
    public class UrlService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UrlService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
