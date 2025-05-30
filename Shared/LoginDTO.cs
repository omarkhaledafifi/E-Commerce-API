using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public record LoginDTO
    {

        [EmailAddress]
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
