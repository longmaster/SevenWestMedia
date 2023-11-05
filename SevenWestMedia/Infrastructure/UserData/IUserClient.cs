

using Domain;

namespace Infrastructure.UserData
{
    public interface IUserClient
    {
        Task<IEnumerable<User>> GetDataAsync();
    }
}
