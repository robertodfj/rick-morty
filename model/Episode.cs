namespace RickYMorty.model
{
    public class Episode
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string AirDate { get; set; }
        public required string EpisodeCode { get; set; }
        public required string[] Characters { get; set; }
        public required string Url { get; set; }
        public required string Created { get; set; }
    }
}