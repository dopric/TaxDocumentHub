using System.Xml.Linq;

namespace TaxDocumentHub.Api.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<TaxDocument> Documents { get; set; } = new List<TaxDocument>();
        public ICollection<DocumentCategory> Categories { get; set; } = new List<DocumentCategory>();
    }
}
