using Post.Query.Domain.Entites;

namespace Post.Query.Domain.Repositories;

public interface ICommentRepository
{
    Task CreateAsync(CommentEntity comment);
    Task UpdateAsync(CommentEntity comment);
    Task GetByIdAsync(Guid commentId);
    Task DeleteAsync(Guid commentId);
}