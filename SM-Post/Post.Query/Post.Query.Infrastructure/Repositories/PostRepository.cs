using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entites;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository(DatabaseContextFactory contextFactory) : IPostRepository
{
    public async Task CreateAsync(PostEntity post)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        context.Posts.Add(post);

        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid postId)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        var post = await GetByIdAsync(postId);

        if (post == null) return;

        context.Remove(post);
        _ = await context.SaveChangesAsync();
    }

    public async Task<ICollection<PostEntity>> GetAllAsync()
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Posts.AsNoTracking()
            .Include(x => x.Comments).AsNoTracking()
            .ToListAsync();
    }

    public async Task<ICollection<PostEntity>> GetAllWithComments()
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Posts.AsNoTracking()
            .Include(x => x.Comments).AsNoTracking()
            .Where(x => x.Comments.Any())
            .ToListAsync();
    }

    public async Task<ICollection<PostEntity>> GetByAuthor(Guid authorId)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Posts.AsNoTracking()
            .Include(x => x.Comments).AsNoTracking()
            .Where(x => x.AuthorId == authorId)
            .ToListAsync();
    }

    public async Task<PostEntity> GetByIdAsync(Guid postId)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Posts
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<ICollection<PostEntity>> GetByLikeCount(uint numberOfLikes)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Posts.AsNoTracking()
            .Include(x => x.Comments).AsNoTracking()
            .Where(x => x.Likes >= numberOfLikes)
            .ToListAsync();
    }

    public async Task UpdateAsync(PostEntity post)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        context.Posts.Update(post);
        
        _ = await context.SaveChangesAsync();
    }
}