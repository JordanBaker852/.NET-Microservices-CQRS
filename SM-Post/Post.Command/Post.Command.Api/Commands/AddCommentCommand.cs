using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class AddCommentCommand : BaseCommand
{
    public Guid AuthorId { get; set; }
    public string Comment { get; set; } = string.Empty;
}