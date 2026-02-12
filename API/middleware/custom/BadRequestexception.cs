namespace RickYMorty.middleware
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) : base(400, message)
        {
        }
    }
}