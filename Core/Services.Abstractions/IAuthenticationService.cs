using Shared;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        // Login & Register 

        // (LoginModel) => UserResult 
        // (Register Model) => UserResult

        public Task<UserResultDTO> LoginAsync(LoginDTO loginModel);
        public Task<UserResultDTO> RegisterAsync(RegisterDTO registerModel);


        // Get Current User 
        public Task<UserResultDTO> GetUserByEmail(string email);

        // Check Email Exist 
        public Task<bool> CheckEmailExist(string email);
        // Get User Address

        public Task<AddressDTO> GetUserAddress(string email);
        // Update user Address 

        public Task<AddressDTO> UpdateUserAddress(AddressDTO address, string email);
    }
}
