using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.CommentSpecs;
using Chapters.Specifications.UserSpecs;

namespace Chapters.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IRepository<UserChapter> _userChapterRepository;
    private readonly IRepository<User> _userRepository;

    public CommentService(
        IRepository<Comment> commentRepository,
        IRepository<UserChapter> userChapterRepository,
        IRepository<User> userRepository)
    {
        _commentRepository = commentRepository;
        _userChapterRepository = userChapterRepository;
        _userRepository = userRepository;
    }

    public async Task<List<GetCommentResponse>> GetComments(GetCommentRequest commentRequest)
    {
        var comments = await _commentRepository
            .ListAsync(new CommentWithUserRatingSpec(commentRequest.ChapterId));

        return comments
            .Select(comment => GetCommentResponse(commentRequest, comment))
            .ToList();
    }

    public async Task PostComment(string username, int chapterId, PostCommentRequest postCommentRequest)
    {
        var user = await _userRepository.FirstAsync(new UserSpec(username));

        var comment = new Comment
        {
            ChapterId = chapterId,
            Text = postCommentRequest.Text,
            AuthorId = user.Id,
            Rating = 0,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _commentRepository.AddAsync(comment);
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
}