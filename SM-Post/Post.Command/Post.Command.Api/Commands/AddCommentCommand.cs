using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public Guid UserId { get; set; }
    public string Comment { get; set; } = string.Empty;
}