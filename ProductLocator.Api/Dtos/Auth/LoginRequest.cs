public record LoginRequest(
    string Username,
    string Password,
    UserSummary User
);