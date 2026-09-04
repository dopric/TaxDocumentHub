using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxDocumentHub.Api.Data;
using TaxDocumentHub.Api.Domain;
using TaxDocumentHub.Api.DTOs;

namespace TaxDocumentHub.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public DocumentsController(AppDbContext dbContext, IWebHostEnvironment hostingEnvironment)
    {
        _dbContext = dbContext;
        _hostingEnvironment = hostingEnvironment;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var documents = await _dbContext.Documents.ToListAsync();
        return Ok(documents);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Create([FromForm] CreateDocumentDto request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("Keine gültige Datei ausgewählt");
        }

        if (request.File.ContentType != "application/pdf")
        {
            return BadRequest("Nur PDF-Dateien sind erlaubt");
        }

        var userId = GetUserId();
        var existingCategory = await _dbContext.Categories.FindAsync(request.CategoryId);
        if (existingCategory == null)
        {
            return BadRequest("Kategorie ungültig");
        }
        var uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var storageFileName = $"{Guid.NewGuid()}_{request.File.FileName}";
        var filePath = Path.Combine(uploadFolder, storageFileName);
        
        // Datei physisch auf dem Server ablegen
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var document = new TaxDocument
        {
            Title = request.Title,
            CategoryId = existingCategory.Id,
            OriginalFileName = request.File.FileName,
            StorageFileName = storageFileName,
            FileSizeBytes = request.File.Length,
            UserId = userId,
            ContentType = request.File.ContentType
        };

        await _dbContext.Documents.AddAsync(document);
        await _dbContext.SaveChangesAsync();
        return Ok(new { document.Id , document.Title, document.OriginalFileName});
    }
}