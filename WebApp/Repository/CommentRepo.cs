using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.DTOs.Comments;
using WebApp.Model;

namespace WebApp.Repository;

public class CommentRepo(DataContext context)
{
    public async Task Create(CreateCommentRequest request)
    {
        await context.Comments.AddAsync(new Comment
        {
            Content = request.Content,
            UserId = request.UserId,
            PostId = request.PostId,
        });
        await context.SaveChangesAsync();
    }

    public async Task Edit(ModifyCommentRequest request) => await context.Comments.Where(x => x.Id == request.CommentId && x.UserId == request.UserId)
        .ExecuteUpdateAsync(x => x.SetProperty(y => y.Content, request.Content));

    public async Task Delete(Guid id) => await context.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
}