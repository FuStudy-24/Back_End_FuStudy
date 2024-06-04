using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Tools;

namespace FuStudy_Service.Service
{
    public class BlogLikeService : IBlogLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BlogLikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BlogLikeResponse> CreateBlogLikeAsync(BlogLikeRequest request)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var checkBlogLike = _unitOfWork.BlogLikeRepository
                                    .Get(filter: x => x.Blog.Id == request.BlogId
                                        && x.User.Id == request.UserId, includeProperties: "Blog")
                                    .FirstOrDefault();

                if (checkBlogLike != null)
                {
                    throw new CustomException.InvalidDataException("Invalid data");
                }

                var blogLike = _mapper.Map(request, checkBlogLike);
                blogLike.Status = true;
                await _unitOfWork.BlogLikeRepository.AddAsync(blogLike);

                var getBlog = _unitOfWork.BlogRepository.GetByID(request.BlogId);
                getBlog.TotalLike += 1;

                _unitOfWork.Save();

                transaction.Complete();
                return _mapper.Map<BlogLikeResponse>(blogLike);
            }
        }

        public async Task<BlogLikeResponse> GetBlogLikeByIdAsync(long id)
        {
            var getById = await _unitOfWork.BlogLikeRepository.GetByIdAsync(id);
            return _mapper.Map<BlogLikeResponse>(getById);
        }

        public async Task<BlogLikeResponse> DeleteBlogLikeAsync(BlogLikeRequest request) // Change Status, NOT DELETE
        {

            var checkBlogLike = _unitOfWork.BlogLikeRepository
                                    .Get(filter: x => x.Blog.Id == request.BlogId
                                        && x.User.Id == request.UserId, includeProperties: "Blog")
                                    .FirstOrDefault();

            checkBlogLike.Status = !checkBlogLike.Status;

            if (checkBlogLike.Status)
            {
                checkBlogLike.Blog.TotalLike += 1;
            }
            else
            {
                var result = checkBlogLike.Blog.TotalLike <= 0 ? 0 : checkBlogLike.Blog.TotalLike -= 1;
            }
            _unitOfWork.Save();

            var blogLike = _mapper.Map<BlogLike>(checkBlogLike);
            await _unitOfWork.BlogLikeRepository.UpdateAsync(blogLike);

            return _mapper.Map<BlogLikeResponse>(blogLike);
        }
    }
}
