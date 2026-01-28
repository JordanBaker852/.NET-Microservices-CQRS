using Post.Query.Domain.Entites;

namespace Post.Query.Domain.Repositories;

public interface IPostRepository
{
    Task CreateAsync(PostEntity post);
    Task UpdateAsync(PostEntity post);
    Task DeleteAsync(Guid postId);
    Task<PostEntity> GetByIdAsync(Guid postId);
    Task<ICollection<PostEntity>> GetAllAsync();
    Task<ICollection<PostEntity>> GetByAuthor(Guid authorId);
    Task<ICollection<PostEntity>> GetByLikeCount(uint numberOfLikes);
    Task<ICollection<PostEntity>> GetAllWithComments();
}