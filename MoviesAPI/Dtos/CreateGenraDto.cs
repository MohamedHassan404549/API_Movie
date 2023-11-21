using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos
{
    public class CreateGenraDto
    {
        [MaxLength(100)]
        public string name { get; set; }
    }
}
