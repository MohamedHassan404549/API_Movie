using MoviesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos
{
    public class MovieDetalesDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }

        public int Year { get; set; }
        public double Rate { get; set; }
        
        public string StoreLine { get; set; }

        public byte[] poster { get; set; }

        public byte GenreId { get; set; }

        public string GenreName { get; set; }
    }
}

