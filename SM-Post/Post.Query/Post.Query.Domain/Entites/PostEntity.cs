using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entites;

[Table("post")]
public class PostEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime DatePosted { get; set; }
    public string Message { get; set; }
    public uint Likes { get; set; }
    public virtual ICollection<CommentEntity> Comments { get; set; } 
}