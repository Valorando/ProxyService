using receive_ID.Models;
namespace receive_ID.Interfaces
{
    public interface IDbService
    {
        public Task<UserData> GetUser(int id);
        public Task SaveUser(UserData userData);
    }


}
