using Chapters.Domain.Entities;
using Chapters.Dto.Requests;
using Chapters.Dto.Responses;
using Chapters.Services.Interfaces;
using Chapters.Specifications.CommentSpecs;

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