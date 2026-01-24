using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid AuthorId { get; set; }
    public Guid CommentId { get; set; }
}