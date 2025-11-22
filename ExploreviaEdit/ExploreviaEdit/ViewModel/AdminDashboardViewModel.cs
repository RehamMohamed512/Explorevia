using ExploreviaEdit.Models;
using System.Collections.Generic;

namespace ExploreviaEdit.ViewModel
{
    public class AdminDashboardViewModel
    {
        public List<HotelRegistrationRequest> PendingRequests { get; set; }
        public List<Hotel> Hotels { get; set; }
    }
}
