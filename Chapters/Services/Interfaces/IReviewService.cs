using Chapters.Dto.Requests;
using Chapters.Dto.Responses;

namespace Chapters.Services.Interfaces;

public interface IReviewService
{
    Task<List<GetReviewResponse>> GetChapters(GetReviewsRequest reviewsRequest);
}