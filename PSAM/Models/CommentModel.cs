namespace PSAM.Models
{
    public record CommentModel(string Text, int? PostId = null, int? ParentCommentId = null)
    {
    }
}
