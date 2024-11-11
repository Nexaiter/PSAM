namespace PSAM.Exceptions
{
    public class PostExceptions : Exception
    {
        public PostExceptions(string message) : base(message) { }
    }

    public class PostDoesntExistException : PostExceptions
    {
        public PostDoesntExistException() : base("Post doesnt exist.") { }
    }
}
