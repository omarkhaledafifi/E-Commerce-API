using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.OrderModels;
using System.Security.Claims;

namespace Presentation
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {


        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDTO>> Login(LoginDTO login)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(login));

        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDTO>> Register(RegisterDTO register)
            => Ok(await serviceManager.AuthenticationService.RegisterAsync(register));


        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.AuthenticationService.CheckEmailExist(email));
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.AuthenticationService.GetUserByEmail(email);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.GetUserAddress(email);
            return Ok(result);
        }


        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.AuthenticationService.UpdateUserAddress(address, email);
            return Ok(result);
        }

    }
}
