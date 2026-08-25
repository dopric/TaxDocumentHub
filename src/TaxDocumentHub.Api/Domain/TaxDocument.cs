namespace TaxDocumentHub.Api.Domain
{
    public class TaxDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public string StorageFileName { get; set; } = string.Empty; // Z.B. Name im Dateisystem/UUID
        public string ContentType { get; set; } = "application/pdf";
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;

        // Isolation: Welcher User besitzt dieses Dokument?
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Zuordnung: Zu welcher Kategorie gehört es?
        public Guid CategoryId { get; set; }
        public DocumentCategory? Category { get; set; }
    }
}
