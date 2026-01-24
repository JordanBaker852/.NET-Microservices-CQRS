using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Command.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private bool _active;
    private Guid _authorId;
    private readonly Dictionary<Guid, UserComment> _comments = new();

    public bool Active
    {
        get => _active; set => _active = value;
    }

    public PostAggregate() { }

    public PostAggregate(Guid id, Guid authorId, string message)
    {
        RaiseEvent(new PostCreatedEvent
        {
            Id = id,
            AuthorId = authorId,
            Message = message,
            DatePosted = DateTime.Now
        });
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _authorId = @event.AuthorId;
    }

    public void EditMessage(string message)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You can't edit messgaes of an inactive post");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please providea valid {nameof(message)}");
        }

        RaiseEvent(new MessageUpdatedEvent
        {
            Id = _id,
            Message = message
        });
    }

    public void Apply(MessageUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void LikePost()
    {
        if (_active)
        {
            throw new InvalidOperationException("You cannot like an invalid post");
        }

        RaiseEvent(new PostLikedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(Guid userId, string comment)
    {
        if (_active)
        {
            throw new InvalidOperationException("You cannot add a comment to an inactive post");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please providea valid {nameof(comment)}");
        }

        RaiseEvent(new CommentAddedEvent
        {
            Id = _id,
            CommentId = Guid.NewGuid(),
            Comment = comment,
            AuthorId = userId,
            CommentDate = DateTime.Now
        });
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new UserComment(@event.AuthorId, @event.Comment));
    }

    public void EditComment(Guid userId, Guid commentId, string comment)
    {
        if (_active)
        {
            throw new InvalidOperationException("You cannot edit a comment of an inactive post");
        };

        if (_comments[commentId].GetAuthor() != userId)
        {
            throw new InvalidOperationException($"You cannot edit the comment of another user");
        }

        RaiseEvent(new CommentUpdatedEvent
        {
            Id = _id,
            CommentId = commentId,
            Comment = comment,
            AuthorId = userId,
            EditedDate = DateTime.Now
        });
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new UserComment(@event.AuthorId, @event.Comment);
    }

    public void RemoveComment(Guid userId, Guid commentId)
    {
        if (_active)
        {
            throw new InvalidOperationException("You cannot remove a comment of an inactive post");
        };

        if (_comments[commentId].GetAuthor() != userId)
        {
            throw new InvalidOperationException($"You cannot remove the comment of another user");
        }

        RaiseEvent(new CommentRemovedEvent
        {
            Id = _id,
            CommentId = commentId
        });
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(Guid userId)
    {
        if (_active)
        {
            throw new InvalidOperationException("The post has already been removed");
        };

        if (_authorId != userId)
        {
            throw new InvalidOperationException($"You cannot remove the post of another user");
        }

        RaiseEvent(new PostRemovedEvent
        {
            Id = _id
        });
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
       _active = false;
    }
}

public class UserComment
{
    private Guid _userId;
    private string _message;

    public UserComment(Guid userId, string message)
    {
        _userId = userId;
        _message = message; 
    }

    public Guid GetAuthor()
    {
        return _userId;
    }

    public string GetUserMessage()
    {
        return _message;
    }
}