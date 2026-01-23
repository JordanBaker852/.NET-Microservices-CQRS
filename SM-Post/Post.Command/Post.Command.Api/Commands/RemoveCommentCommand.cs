using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class RemoveCommentCommand : BaseCommand
{
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
}