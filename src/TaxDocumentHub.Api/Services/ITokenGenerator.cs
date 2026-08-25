using TaxDocumentHub.Api.Domain;

namespace TaxDocumentHub.Api.Services
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
