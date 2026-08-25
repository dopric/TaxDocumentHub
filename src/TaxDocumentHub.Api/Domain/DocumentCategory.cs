namespace TaxDocumentHub.Api.Domain
{
    public class DocumentCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        // Jede Kategorie gehört zu genau einem Benutzer
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public ICollection<TaxDocument> Documents { get; set; } = new List<TaxDocument>();
    }
}
