using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MoviesAPI;
     
namespace MoviesAPI.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext()
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=MoviesAPI; Integrated Security=True; Encrypt=False;");

        //}
        public MoviesContext(DbContextOptions<MoviesContext> options):base(options)
        {
        }

        public virtual DbSet<Genre>? Genres { get; set; }
        public virtual DbSet<Movie>? Movies { get; set; }
        
       
        


       
       
        

    }
}