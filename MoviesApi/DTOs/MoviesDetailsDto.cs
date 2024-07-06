namespace Movies.PL.DTOs
{
    public class MoviesDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}
