using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly MoviesContext _context;

        public GenresController(MoviesContext context)
        {
            _context = context;
        }

        //MoviesContext db = new MoviesContext();
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.Genres.ToListAsync();

            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenraDto dto)
        {
            var genre = new Genre { Name = dto.name };
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return Ok(genre);
        }

        [HttpPut("{Id}")]
        
        public async Task<IActionResult> UpdateAsync(int id , [FromBody] CreateGenraDto dto)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound($"not genre was found whis ID:{id} ");
            }
            genre.Name = dto.name;
            _context.SaveChanges(); 

            return Ok(genre );
        }


        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if (genre == null)
            {
                return NotFound($"not genre was found whis ID:{id} ");
            }

            _context.Remove(genre);
            _context.SaveChangesAsync();

            return Ok(genre);
        }
    }
}