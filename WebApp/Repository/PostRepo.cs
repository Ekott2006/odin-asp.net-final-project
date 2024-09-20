using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.DTOs.Posts;
using WebApp.Model;
using WebApp.Model.Enum;

namespace WebApp.Repository;

public class PostRepo(DataContext context, FriendRepo friendRepo, UserRepo userRepo)
{
    public async Task<Guid?> Create(CreatePostRequest request)
    {
        if (request.Content is null && request.ImageLink is null) return null;
        if (request.Content is not null && request.ImageLink is not null) return null;
        Post newPost = new()
        {
            PostDetail = new PostDetail
            {
                Type = request.Content is not null ? PostType.Text : PostType.Image,
                Content = request.Content,
                Link = request.ImageLink,
                Caption = request.Content ?? string.Empty,
            },
            AuthorId = request.AuthorId
        };
        await context.Posts.AddAsync(newPost);
        await context.SaveChangesAsync();
        return newPost.Id;
    }

    public async Task<PaginatedPostsResponse> GetByUserId(string userId, int page = 1, int perPage = 20)
    {
        List<string> friends = await friendRepo.GetByState(userId, FriendStatus.Accepted)
            .Select(x => x.SenderId == userId ? x.ReceiverId : x.SenderId).ToListAsync();
        IQueryable<Post> query = context.Posts.Where(x => x.AuthorId == userId || friends.Contains(x.AuthorId))
            .Include(x => x.PostDetail)
            .Include(x => x.Author)
            .Include(x => x.Comments)
            .Include(x => x.Likes);
        int total = await query.CountAsync();
        return new PaginatedPostsResponse
        {
            Pages = total % perPage == 0 ? total / perPage : total / perPage + 1,
            PerPage = perPage,
            Total = total,
            Posts = await query.OrderByDescending(x => x.CreatedAt).Skip(perPage * (page - 1)).Take(perPage)
                .Select(x => PostResponse.FromPost(x, userId)).ToListAsync(),
            Page = page
        };
    }
    
    public async Task<PostAndCommentResponse?> GetById(Guid id, string userId) => await context.Posts
        .Where(x => x.Id == id)
        .Include(x => x.PostDetail)
        .Include(x => x.Author)
        .Include(x => x.Comments).ThenInclude(x =>x.Commenter)
        .Include(x => x.Likes)
        .Select(x => PostAndCommentResponse.FromPost(x, userId)).FirstOrDefaultAsync();

    public async Task Delete(Guid id, string authorId) =>
        await context.Posts.Where(x => x.Id == id && x.AuthorId == authorId).ExecuteDeleteAsync();

    public async Task<bool> HandleLike(Guid id, string likerId)
    {
        User? user = await userRepo.Get(likerId);
        if (user is not null) return await HandleLike(id, user);
        return false;
    }

    public async Task<bool> HandleLike(Guid id, User liker)
    {
        Post? post = await context.Posts.Include(post => post.Likes).FirstOrDefaultAsync(x => x.Id == id);
        if (post is null) return false;
        bool hasBeenLiked = post.Likes.Contains(liker);
        if (hasBeenLiked) post.Likes.Remove(liker);
        else post.Likes.Add(liker);
        await context.SaveChangesAsync();
        return true;
    }
}