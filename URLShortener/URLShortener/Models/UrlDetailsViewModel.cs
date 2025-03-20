using BusinessLogicLayer.DTO;

namespace URLShortener.ViewModels
{
    public class UrlDetailsViewModel
    {
        public UrlDTO Url { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsAdmin { get; set; }
    }
}