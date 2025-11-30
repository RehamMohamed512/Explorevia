using Explorevia.Models;

namespace Explorevia.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<HotelRegistrationRequest> PendingRequests { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}
