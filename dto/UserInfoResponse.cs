using RickYMorty.model;

namespace RickYMorty.dto
{
    public class UserInfoResponse
    {
        public required string Username { get; set; }
        public List<CharacterResponse> Characters { get; set; }
        public List<EpisodeResponse> Episodes { get; set; }
        public double Money { get; set; }
        public DateTime? LastWorked { get; set; }

        public UserInfoResponse()
        {
            Characters = new List<CharacterResponse>();
            Episodes = new List<EpisodeResponse>();
        }
    }
}