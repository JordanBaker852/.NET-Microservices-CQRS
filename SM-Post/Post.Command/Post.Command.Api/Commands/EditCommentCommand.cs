using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class EditCommentCommand : BaseCommand
{
    public Guid UserId { get; set; }
    public string Comment { get; set; } = string.Empty;
}