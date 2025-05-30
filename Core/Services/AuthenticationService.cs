
global using Domain.Entities.Identity;
global using Domain.Exceptions;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Shared.OrderModels;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using OrderAddress = Domain.Entities.OrderEntities.Address;
global using UserAddress = Domain.Entities.Identity.Address;
namespace Services
{
    internal class AuthenticationService(UserManager<User> userManager,
        IOptions<JwtOptions> options,
        IMapper mapper)
        : IAuthenticationService

    {
        public async Task<bool> CheckEmailExist(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return user != null;
        }

        public async Task<AddressDTO> GetUserAddress(string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);



            return mapper.Map<AddressDTO>(user.Address);
            //return new AddressDTO
            //{
            //    City = user.Address.City,
            //    Country = user.Address.Country,
            //    FirstName = user.Address.FirstName,
            //    LastName = user.Address.LastName,
            //};
        }

        public async Task<UserResultDTO> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);

            return new UserResultDTO(
           user.DisplayName,
           user.Email!,
           await CreateTokenAsync(user));
        }

        public async Task<UserResultDTO> LoginAsync(LoginDTO loginModel)
        {
            // 
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) throw new UnAuthorizedException($"Email {loginModel.Email} doesn't Exist."); // 

            var result = await userManager.CheckPasswordAsync(user, loginModel.Password);

            if (!result) throw new UnAuthorizedException();

            return new UserResultDTO(
              user.DisplayName,
              user.Email!,
              await CreateTokenAsync(user));

        }

        public async Task<UserResultDTO> RegisterAsync(RegisterDTO registerModel)
        {

            var user = new User
            {
                Email = registerModel.Email,
                DisplayName = registerModel.DisplayName,
                UserName = registerModel.DisplayName,
                PhoneNumber = registerModel.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            return new UserResultDTO(
             user.DisplayName,
             user.Email!,
             await CreateTokenAsync(user));
        }

        public async Task<AddressDTO> UpdateUserAddress(AddressDTO address, string email)
        {
            var user = await userManager.Users.Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Email == email)
            ?? throw new UserNotFoundException(email);

            //var userAddress = mapper.Map<Address>(address);
            //user.Address= userAddress;

            if (user.Address != null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;

            }
            else
            {
                var userAddress = mapper.Map<UserAddress>(address);
                user.Address = userAddress;
            }

            await userManager.UpdateAsync(user);

            return address;


        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = options.Value;
            // Create Claims 

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: authClaims,
                signingCredentials: creds,
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issure,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays)
                );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
