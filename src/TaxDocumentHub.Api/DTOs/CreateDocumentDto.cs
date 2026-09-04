namespace TaxDocumentHub.Api.DTOs;

public class CreateDocumentDto
{
    public string Title { get; set; } =  string.Empty;
    public Guid CategoryId { get; set; } = Guid.Empty;
    public IFormFile File { get; set; }
}