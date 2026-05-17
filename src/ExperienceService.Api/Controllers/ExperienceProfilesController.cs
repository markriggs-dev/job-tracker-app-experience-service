using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExperienceService.Core.Services;

namespace ExperienceService.Api.Controllers;

[ApiController]
[Route("api/experience-profiles")]
[Authorize]
public class ExperienceProfilesController : ControllerBase
{
    private readonly ExperienceProfileService _service;

    public ExperienceProfilesController(ExperienceProfileService service) => _service = service;

    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await _service.GetAllAsync(UserId);
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] string profileName, IFormFile file)
    {
        if (string.IsNullOrWhiteSpace(profileName))
            return BadRequest(new { error = "Profile name is required." });

        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        var allowedTypes = new[]
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "text/plain"
        };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { error = "Only PDF, DOC, DOCX, and TXT files are accepted." });

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest(new { error = "File size must not exceed 10 MB." });

        var result = await _service.UploadAsync(UserId, profileName, file);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _service.DownloadAsync(id, UserId);
        if (result is null) return NotFound();
        return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id, UserId);
        return deleted ? NoContent() : NotFound();
    }
}
