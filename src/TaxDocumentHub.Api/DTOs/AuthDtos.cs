namespace TaxDocumentHub.Api.DTOs
{
   public record RegisterUserDto(string Email, string Password);
    public record LoginUserDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, Guid UserId);
}
