using Movies.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Movies.PL.DTOs
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public IFormFile? Poster { get; set; }
        public int CategoryId { get; set; }
    }
}
