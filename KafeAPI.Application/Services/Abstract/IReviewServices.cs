using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.ReviewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Abstract
{
    public interface IReviewServices
    {
        Task<ResponseDto<List<ResultReviewDto>>> GetAllReviews();

        Task<ResponseDto<List<DetailReviewDto>>> GetByIdReview(int id);

        Task<ResponseDto<object>> AddReview(CreateReviewDto dto);

        Task<ResponseDto<object>> UpdateReview(UpdateReviewDto dto);

        Task<ResponseDto<object>> DeleteReview(int id);




    }
}
