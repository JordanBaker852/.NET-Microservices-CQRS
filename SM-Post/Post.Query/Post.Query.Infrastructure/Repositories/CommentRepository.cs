using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entites;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class CommentRepository(DatabaseContextFactory contextFactory) : ICommentRepository
{
    public async Task CreateAsync(CommentEntity comment)
    {
        using DatabaseContext context = contextFactory.CreateContext(); 
        context.Comments.Add(comment);

        _ = await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid commentId)
    {
        using DatabaseContext context = contextFactory.CreateContext();

        var comment = await GetByIdAsync(commentId);

        if (comment == null) return;

        context.Comments.Remove(comment);
        _ = await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> GetByIdAsync(Guid commentId)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        return await context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        using DatabaseContext context = contextFactory.CreateContext();
        context.Comments.Update(comment);

        _ = await context.SaveChangesAsync();
    }
}