using CQRS.Core.Events;

namespace Post.Common.Events;

public class PostCreatedEvent : BaseEvent
{
    public PostCreatedEvent() : base(nameof(PostCreatedEvent)) { }

    public Guid AuthorId { get; set; }
    public string Message { get; set; }
    public DateTime DatePosted { get; set; }
}