using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entites;

[Table("comment")]
public class CommentEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid AuthorId { get; set; }
    public string Comment { get; set; }
    public DateTime DateCommented { get; set; }
    public bool Edited { get; set; }
    [JsonIgnore]
    public virtual PostEntity Post { get; set; }
}