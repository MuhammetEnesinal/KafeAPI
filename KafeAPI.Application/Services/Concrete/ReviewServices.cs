using AutoMapper;
using KafeAPI.Application.Dtos.ResponseDtos;
using KafeAPI.Application.Dtos.ReviewDtos;
using KafeAPI.Application.Interfaces;
using KafeAPI.Application.Services.Abstract;
using KafeAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeAPI.Application.Services.Concrete
{
    public class ReviewServices : IReviewServices
    {
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;
        public ReviewServices(IGenericRepository<Review> reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public Task<ResponseDto<object>> AddReview(CreateReviewDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<object>> DeleteReview(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<List<ResultReviewDto>>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewRepository.GetAllAsync();
                var result = _mapper.Map<List<ResultReviewDto>>(reviews);
                if(result == null || result.Count == 0)
                {
                    return new ResponseDto<List<ResultReviewDto>>
                    {
                        Success = false,
                        Message = "Hiçbir yorum bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                return new ResponseDto<List<ResultReviewDto>>
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseDto<List<ResultReviewDto>>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public async Task<ResponseDto<List<DetailReviewDto>>> GetByIdReview(int id)
        {
            try
            {
                var review = await _reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    return new ResponseDto<List<DetailReviewDto>>
                    {
                        Success = false,
                        Message = "Yorum bulunamadı.",
                        Data = null,
                        ErrorCode = ErrorCodes.NotFound
                    };
                }
                var result = _mapper.Map<List<DetailReviewDto>>(review);
                return new ResponseDto<List<DetailReviewDto>>
                {
                    Success = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {

                return new ResponseDto<List<DetailReviewDto>>
                {
                    Success = false,
                    Message = "Bir Hata olustu.",
                    Data = null,
                    ErrorCode = ErrorCodes.Exception
                };
            }
        }

        public Task<ResponseDto<object>> UpdateReview(UpdateReviewDto dto)
        {
            throw new NotImplementedException();
        }

      
    }
}
