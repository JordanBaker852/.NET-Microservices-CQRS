namespace Post.Common.Events;

public class MessageUpdatedEvent : BaseEvent
{
    public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent)) { }

    public Guid UserId { get; set; }
    public string Message { get; set; }
    public DateTime DatePosted { get; set; }
}