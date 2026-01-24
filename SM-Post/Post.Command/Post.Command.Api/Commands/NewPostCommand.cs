using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class NewPostCommand : BaseCommand
{
    public Guid AuthorId { get; set; }
    public string Message { get; set; } = string.Empty;
}