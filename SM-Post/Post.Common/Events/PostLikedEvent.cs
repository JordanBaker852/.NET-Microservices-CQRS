using CQRS.Core.Events;

namespace Post.Common.Events;

public class PostLikedEvent : BaseEvent
{
    public PostLikedEvent() : base(nameof(PostLikedEvent)) { }

    public Guid UserId { get; set; }
    public DateTime DatePosted { get; set; }
}