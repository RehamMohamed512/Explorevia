namespace Explorevia.IRepository
{
    public interface IAdminRepository
    {
         Task<bool> Requests();
         Task<bool> GetDetails(int Id);
         Task<bool> ApproveRequest(int requestId);
         Task<bool> RejectRequest(int requestId);


    }
}
