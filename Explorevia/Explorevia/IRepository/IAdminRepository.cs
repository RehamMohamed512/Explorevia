using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IAdminRepository
    {
        Task<List<HotelRegistrationRequest>> Requests();
        Task<HotelRegistrationRequest> GetDetails(int id);
        Task<bool> ApproveRequest(int requestId);
        Task<bool> RejectRequest(int requestId);


    }
}
