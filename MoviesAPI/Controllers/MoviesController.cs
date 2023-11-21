using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using System.Data;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 2097152;
        public MoviesController(MoviesContext context)
        {
            _context = context;
        }
        [HttpGet] //GetAll

        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Select(m => new MovieDetalesDto
                {
                    Id=m.Id,
                    GenreId=m.GenreId,
                    GenreName=m.Genre.Name,
                    poster=m.poster,
                    Rate = m.Rate,
                    StoreLine = m.StoreLine,
                    Title = m.Title,
                    Year = m.Year


                })
                
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]  //GetById

        public async Task<IActionResult> GetbyIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre) .SingleOrDefaultAsync(m => m.Id == id); 
            if (movie == null)
                return NotFound();

            var dto = new MovieDetalesDto
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                poster = movie.poster,
                Rate = movie.Rate,
                StoreLine = movie.StoreLine,
                Title = movie.Title,
                Year = movie.Year
            };
            return Ok(dto);
        }

        [HttpPost] //Add or Create
        public async Task<IActionResult> CreateAsync([FromForm] CreateMovieDto dto)
        {
            if (dto.poster == null)
                return BadRequest("poster is required");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.poster.FileName).ToLower()))
                return BadRequest("only .png and .jpg!");

            if (dto.poster.Length > _maxAllowedPosterSize)
                return BadRequest("MAx Allowed Size is 2MB!");

            var  isValidGenre = await _context.Movies.AnyAsync(g => g.Id == dto.GenreId );
            if (!isValidGenre)
                return BadRequest($"Invalid ID:{dto.GenreId}!");

            using var dateStream = new MemoryStream();
            await dto.poster.CopyToAsync(dateStream);
            var Movie = new Movie
            { 
                GenreId = dto.GenreId,
                Title  = dto.Title,
                poster =dateStream.ToArray(),
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                Year = dto.Year
            };

            await _context.AddAsync(Movie);
            _context.SaveChanges();
            return Ok(Movie);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateAsync(int id, [FromForm]CreateMovieDto dto)
        {
            var movie = await _context.Movies.FindAsync( id);

            if (movie == null)
                return NotFound($"not genre was found whis ID:{id} ");
            

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest($"Invalid ID:{dto.GenreId}!");

            if (dto.poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.poster.FileName).ToLower()))
                    return BadRequest("only .png and .jpg!");

                if (dto.poster.Length > _maxAllowedPosterSize)
                    return BadRequest("MAx Allowed Size is 2MB!");

                using var dateStream = new MemoryStream();
                await dto.poster.CopyToAsync(dateStream);
                movie.poster = dateStream.ToArray();
            }


            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            movie.StoreLine = dto.StoreLine;
            movie.Year = dto.Year;
            
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete("{id}")] //Delete
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(g => g.Id == id);

            if (movie == null)
            {
                return NotFound($"not movie was found whis ID:{id} ");
            }

            _context.Remove(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

    }
}
