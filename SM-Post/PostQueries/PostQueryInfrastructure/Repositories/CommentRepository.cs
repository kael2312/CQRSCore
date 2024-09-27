using Domain.Entities;
using Domain.Repositories;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

public class CommentRepository: ICommentRepository
{
    private readonly DatabaseContextFactory _databaseContextFactory;

    public CommentRepository(DatabaseContextFactory databaseContextFactory)
    {
        _databaseContextFactory = databaseContextFactory;
    }
    
    public async Task CreateAsync(CommentEntity comment)
    {
        using DatabaseContext context = _databaseContextFactory.CreateDbContext();
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CommentEntity comment)
    {
        using DatabaseContext context = _databaseContextFactory.CreateDbContext();
        context.Comments.Update(comment);
        await context.SaveChangesAsync();
    }

    public async Task<CommentEntity> GetByIdAsync(Guid commentId)
    {
        using DatabaseContext context = _databaseContextFactory.CreateDbContext();
        return await context.Comments.FindAsync(commentId);
    }

    public async Task DeleteAsync(Guid commentId)
    {
        using DatabaseContext context = _databaseContextFactory.CreateDbContext();
        var comment = await GetByIdAsync(commentId);
        if(comment == null) return;
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    }
}