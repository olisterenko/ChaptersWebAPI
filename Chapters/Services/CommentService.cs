using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.CommentSpecs;
using Chapters.Specifications.UserRatingCommentSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRatingComment> _userRatingCommentRepository;

    public CommentService(
        IRepository<Comment> commentRepository,
        IRepository<User> userRepository,
        IRepository<UserRatingComment> userRatingCommentRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _userRatingCommentRepository = userRatingCommentRepository;
    }

    public async Task<List<GetCommentResponse>> GetComments(GetCommentRequest commentRequest)
    {
        var comments = await _commentRepository
            .ListAsync(new CommentWithUserRatingSpec(commentRequest.ChapterId));

        return comments
            .Select(comment => GetCommentResponse(commentRequest, comment))
            .ToList();
    }

    public async Task PostComment(string username, PostCommentRequest postCommentRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var comment = new Comment
        {
            ChapterId = postCommentRequest.ChapterId,
            Text = postCommentRequest.Text,
            AuthorId = user.Id,
            Rating = 0,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _commentRepository.AddAsync(comment);
    }

    public async Task<List<GetUserCommentResponse>> GetUserComments(GetUserCommentsRequest getUserCommentsRequest)
    {
        var comments = await _commentRepository
            .ListAsync(new CommentWithUserRatingSpec(getUserCommentsRequest.Author));

        return comments
            .Select(comment => GetUserCommentResponse(getUserCommentsRequest, comment))
            .ToList();
    }

    public async Task RateComment(string username, int commentId, bool isPositive)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var userRatingComment =
            await _userRatingCommentRepository.FirstOrDefaultAsync(new UserRatingCommentSpec(user.Id, commentId));

        if (userRatingComment is null)
        {
            userRatingComment = new UserRatingComment
            {
                CommentId = commentId,
                UserId = user.Id,
                UserRating = isPositive ? 1 : -1
            };

            await _userRatingCommentRepository.AddAsync(userRatingComment);
        }

        userRatingComment.UserRating = isPositive ? 1 : -1;

        await _userRatingCommentRepository.SaveChangesAsync();
    }

    private GetCommentResponse GetCommentResponse(GetCommentRequest reviewsRequest, Comment comment)
    {
        var userRating = 0;
        if (reviewsRequest.Username is not null)
        {
            userRating = comment.UserRatingComments
                .FirstOrDefault(ur => ur.User.Username == reviewsRequest.Username)?
                .UserRating ?? 0;
        }

        return new GetCommentResponse(
            Id: comment.Id,
            AuthorId: comment.AuthorId,
            AuthorUsername: comment.Author.Username,
            Text: comment.Text,
            Rating: comment.UserRatingComments.Sum(ur => ur.UserRating),
            UserRating: userRating,
            CreatedAt: comment.CreatedAt
        );
    }

    private GetUserCommentResponse GetUserCommentResponse(GetUserCommentsRequest reviewsRequest, Comment comment)
    {
        var userRating = 0;
        if (reviewsRequest.Username is not null)
        {
            userRating = comment.UserRatingComments
                .FirstOrDefault(ur => ur.User.Username == reviewsRequest.Username)?
                .UserRating ?? 0;
        }

        return new GetUserCommentResponse(
            Id: comment.Id,
            AuthorId: comment.AuthorId,
            AuthorUsername: comment.Author.Username,
            Text: comment.Text,
            Rating: comment.UserRatingComments.Sum(ur => ur.UserRating),
            UserRating: userRating,
            CreatedAt: comment.CreatedAt,
            ChapterId: comment.ChapterId,
            ChapterTitle: comment.Chapter.Title,
            BookId: comment.Chapter.BookId,
            BookTitle: comment.Chapter.Book.Title
        );
    }
}