using ExploreviaEdit.Models;

namespace ExploreviaEdit.IRepository
{
    public interface IAdminRepository
    {
        Task<List<HotelRegistrationRequest>> GetPendingRequests();
        Task<bool> ApproveRequest(int requestId);
        Task<bool> RejectRequest(int requestId);

        Task<List<Hotel>> GetHotels();
        Task<Hotel> GetHotelById(int id);
        Task<bool> AddHotel(Hotel hotel);
        Task<bool> UpdateHotel(Hotel hotel);
        Task<bool> DeleteHotel(int id);


    }
}
