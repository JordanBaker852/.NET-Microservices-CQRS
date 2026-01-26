using CQRS.Core.Handlers;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Api.Commands;

public class CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler) : ICommandHandler
{
    public async Task HandleAsync(NewPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.AuthorId, command.Message);

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditMessageCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.EditMessage(command.Message);

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.LikePost();

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.AddComment(command.AuthorId, command.Comment);

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditCommentCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.EditComment(command.AuthorId, command.AuthorId, command.Comment);

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.RemoveComment(command.AuthorId, command.CommentId);

        await eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        var aggregate = await eventSourcingHandler.GetByIdAsync(command.Id);

        aggregate.DeletePost(command.AuthorId);

        await eventSourcingHandler.SaveAsync(aggregate);
    }
}

public interface ICommandHandler
{
    Task HandleAsync(NewPostCommand command);
    Task HandleAsync(EditMessageCommand command);
    Task HandleAsync(LikePostCommand command);
    Task HandleAsync(AddCommentCommand command);
    Task HandleAsync(EditCommentCommand command);
    Task HandleAsync(RemoveCommentCommand command);
    Task HandleAsync(DeletePostCommand command);
}