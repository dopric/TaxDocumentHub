namespace TaxDocumentHub.Api.DTOs
{
    public record CreateCategoryDto(string Name);

    public record CategoryResponseDto(Guid Id, string Name);
}
