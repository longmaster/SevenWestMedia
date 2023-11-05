using Application.Interface;
using Domain;

namespace Infrastructure.UserData
{
    public class UserEngine : IUserEngine
    {
        private readonly IUserClient _userClient;
        public UserEngine(IUserClient userClient) {
            _userClient = userClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userClient.GetDataAsync();
        }
    }
}
