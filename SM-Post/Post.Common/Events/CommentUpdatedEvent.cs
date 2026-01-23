namespace Post.Common.Events;

public class CommentUpdatedEvent : BaseEvent
{
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent)) { }

    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public string Comment { get; set; }
    public DateTime EditedDate { get; set; }
}