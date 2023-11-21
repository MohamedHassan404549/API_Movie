using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MoviesAPI.Dtos
{
    public class CreateMovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoreLine { get; set; }
        
        public IFormFile? poster { get; set; }

        public byte GenreId { get; set; }

    }
}
