namespace PSAM.Exceptions
{
    public class CommentExceptions : Exception
    {
        public CommentExceptions(string message) : base(message) { }
    }
    public class BothIdsProvidedException : CommentExceptions
    {
        public BothIdsProvidedException() : base("Both postId and commentId cannot be provided at the same time.") { }
    }

    public class NoIdProvidedException : CommentExceptions
    {
        public NoIdProvidedException() : base("Either postId or commentId must be provided.") { }
    }

    public class PostNotFoundException : CommentExceptions
    {
        public PostNotFoundException() : base("The specified post does not exist.") { }
    }

    public class CommentNotFoundException : CommentExceptions
    {
        public CommentNotFoundException() : base("The specified comment does not exist.") { }
    }

}
