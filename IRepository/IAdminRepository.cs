using Explorevia.Models;
using Explorevia.ViewModels;

namespace Explorevia.IRepository
{
    public interface IAdminRepository
    {
        Task<List<HotelRegistrationRequest>> Requests();
        Task<List<Hotel> > GetAllHotels();
        Task<HotelRegistrationRequest> GetDetails(int id);
        Task<bool> ApproveRequest(int requestId);
        Task<bool> RejectRequest(int requestId);
        Task<bool> DeleteHotel(int id);

    }
}
