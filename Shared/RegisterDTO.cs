﻿namespace Shared
{
    public record RegisterDTO
    {

        public string DisplayName { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
