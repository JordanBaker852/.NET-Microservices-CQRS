using CQRS.Core.Commands;

namespace Post.Command.Api.Commands;

public class DeletePostCommand : BaseCommand
{
    public Guid UserId { get; set; }
}