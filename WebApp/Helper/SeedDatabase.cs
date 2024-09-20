using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.DTOs.Comments;
using WebApp.DTOs.Posts;
using WebApp.Model;
using WebApp.Model.Enum;
using WebApp.Repository;

namespace WebApp.Helper;

public class SeedDatabase(
    DataContext context,
    UserManager<User> userManager,
    PostRepo postRepo,
    CommentRepo commentRepo,
    FriendRepo friendRepo, UserRepo userRepo,
    ILogger<SeedDatabase> logger)
{
    public async Task FakeData()
    {
        await context.Database.EnsureCreatedAsync();
        if (await context.Users.AnyAsync()) return;

        List<User> users = new UserFaker().Generate(10);
        Random random = new();
        foreach (User user in users)
        {
            string password = new Faker().Internet.Password() + "1234567890"[random.Next(9)] +
                              "!@#$%^&*"[random.Next(5)];
            IdentityResult result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                logger.LogInformation("USERS SEEDING ERRORS: {}", result.ToString());
                continue;
            }

            logger.LogInformation("USER Seeding Successful!!! Username: {}, Password: {}", user.UserName, password);
            for (int i = 0; i < random.Next(3, 10); i++)
            {
                await postRepo.Create(new CreatePostRequestTextFaker(user.Id));
            }
        }

        users = await context.Users.ToListAsync();
        if (users.Count == 0)
        {
            logger.LogError("No User Seeded");
            return;
        }

        // COMMENTS
        foreach (User user in users)
        {
            PaginatedPostsResponse posts = await postRepo.GetByUserId(user.Id);
            foreach (PostResponse post in posts.Posts)
            {
                for (int i = 0; i < random.Next(5); i++)
                {
                    await postRepo.HandleLike(post.Id, users[random.Next(users.Count - 1)].Id);
                    await commentRepo.Create(new CreateCommentRequestFaker(users[random.Next(users.Count - 1)].Id,
                        post.Id));
                }
            }
        }

        // FRIENDS 
        foreach (User user1 in users)
        {
            foreach (User user2 in users)
            {
                Guid? id = await friendRepo.Modify(user1.Id, user2.Id);
                if (id is { } a)
                {
                    await friendRepo.Modify(a, Enum.GetValues<FriendStatus>()[random.Next(1)]);
                }
            }
        }

        foreach (User? user in users)
        {
            User? userData = await userRepo.Get(user.Id);
            if (userData is null) return;
            logger.LogInformation("Username: {} Posts: {}, Post 1 ID: {}", userData.UserName, userData.Posts.Count, userData.Posts.First().Id);
        }
        logger.LogInformation("Seeding Complete");
    }

    private sealed class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            RuleFor(d => d.UserName, f => f.Internet.UserName());
            RuleFor(d => d.Email, f => f.Internet.Email());
            RuleFor(d => d.ProfilePicture, f => f.Internet.Avatar());
        }
    }

    private sealed class CreateCommentRequestFaker : Faker<CreateCommentRequest>
    {
        public CreateCommentRequestFaker(string userId, Guid postId)
        {
            RuleFor(d => d.Content, f => f.Lorem.Sentences());
            RuleFor(d => d.UserId, _ => userId);
            RuleFor(d => d.PostId, _ => postId);
        }
    }

    private sealed class CreatePostRequestTextFaker : Faker<CreatePostRequest>
    {
        public CreatePostRequestTextFaker(string userId)
        {
            RuleFor(d => d.Content, f => f.Lorem.Sentences());
            RuleFor(d => d.AuthorId, _ => userId);
        }
    }
}