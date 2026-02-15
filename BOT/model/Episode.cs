namespace Bot.model.response
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string AirDate { get; set; } = "";
        public string EpisodeCode { get; set; } = "";
        public string Created { get; set; } = "";
        public bool ForSale { get; set; }
        public double Price { get; set; }
    }
}