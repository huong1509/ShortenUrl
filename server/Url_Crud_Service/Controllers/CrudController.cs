using Url_Crud_Service.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Url_Crud_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrudController : ControllerBase
    {
        private readonly CrudDbContext _db;

        public CrudController(CrudDbContext db)
        {
            _db = db;
        }

        // GET: api/urlcrud
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var emailClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (emailClaim == null)
                return Unauthorized();

            string email = emailClaim.Value;

            var urls = await _db.UrlCruds
                .Where(u => u.Email == email)   
                .OrderByDescending(u => u.DateTime)
                .ToListAsync();

            return Ok(urls);
        }

        // GET: api/urlcrud/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emailClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (emailClaim == null)
                return Unauthorized();

            string email = emailClaim.Value;

            var url = await _db.UrlCruds.FirstOrDefaultAsync(u => u.Id == id && u.Email == email);
            if (url == null)
                return NotFound("URL not found");

            return Ok(url);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCode(int id, [FromBody] string newCode)
        {
            if (string.IsNullOrWhiteSpace(newCode))
                return BadRequest("Code cannot be empty.");

            var existing = await _db.UrlCruds.FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
                return NotFound("URL not found.");

            bool exists = await _db.UrlCruds.AnyAsync(u => u.ShortenCode == newCode && u.Id != id);
            if (exists)
                return BadRequest("The code already exists, please choose another one.");

            existing.ShortenCode = newCode;
            const string baseUrl = "http://localhost:2000/api/shorten";
            existing.ShortenUrl = $"{baseUrl}/{newCode}";

            _db.Entry(existing).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var stillExists = await _db.UrlCruds.AnyAsync(u => u.Id == id);
                if (!stillExists)
                    return NotFound("URL no longer exists.");
                throw;
            }

            return Ok(new
            {
                existing.Id,
                existing.ShortenCode,
                existing.ShortenUrl
            });
        }

        // DELETE: api/urlcrud/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var url = await _db.UrlCruds.FirstOrDefaultAsync(u => u.Id == id);
            if (url == null)
                return NotFound("URL not found");

            _db.UrlCruds.Remove(url);
            await _db.SaveChangesAsync();

            return Ok($"Deleted URL with ID {id}");
        }
    }
}
