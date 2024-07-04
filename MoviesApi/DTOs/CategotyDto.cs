using System.ComponentModel.DataAnnotations;

namespace Movies.PL.DTOs
{
    public class CategoryDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
