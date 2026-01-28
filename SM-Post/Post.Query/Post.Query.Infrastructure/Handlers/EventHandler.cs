using Post.Common.Events;
using Post.Query.Domain.Entites;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler(IPostRepository postRepository, ICommentRepository commentRepository) : IEventHandler
{
    public async Task On(PostCreatedEvent @event)
    {
        var post = new PostEntity
        {
            Id = @event.Id,
            AuthorId = @event.AuthorId,
            DatePosted = @event.DatePosted,
            Message = @event.Message
        };

        await postRepository.CreateAsync(post);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        var post = await postRepository.GetByIdAsync(@event.Id);

        if (post == null) return;

        post.Message = @event.Message;
        await postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        var post = await postRepository.GetByIdAsync(@event.Id);

        if (post == null) return;

        post.Likes++;
        await postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        var comment = new CommentEntity
        {
            Id = @event.CommentId,
            PostId = @event.Id,
            Comment = @event.Comment,
            AuthorId = @event.AuthorId,
            DateCommented = @event.CommentDate
        };

        await commentRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        var comment = await commentRepository.GetByIdAsync(@event.CommentId);

        if (comment == null) return;

        comment.Comment = @event.Comment;
        comment.EditedDate = @event.EditedDate;
        
        await commentRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        await commentRepository.DeleteAsync(@event.CommentId);
    }

    public async Task On(PostRemovedEvent @event)
    {
        await postRepository.DeleteAsync(@event.Id);
    }
}

public interface IEventHandler
{
    Task On(PostCreatedEvent @event);
    Task On(MessageUpdatedEvent @event);
    Task On(PostLikedEvent @event);
    Task On(CommentAddedEvent @event);
    Task On(CommentUpdatedEvent @event);
    Task On(CommentRemovedEvent @event);
    Task On(PostRemovedEvent @event);
}