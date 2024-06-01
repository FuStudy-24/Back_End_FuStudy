using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //public async Task<BlogLikeResponse> CreateBlogLikeAsync(BlogLikeRequest request)
        //{
        //    var checkBlogLike = _unitOfWork.BlogLikeRepository
        //                            .Get(filter: x => x.Blog.Id == request.BlogId
        //                                && x.User.Id == request.UserId, includeProperties: "Blog,BlogLike")
        //                            .FirstOrDefault();


        //    if (checkBlogLike != null)
        //    {
        //        return null;
        //    }

        //    checkBlogLike.Status = true;
        //    checkBlogLike.Blog.TotalLike += 1;

        //    var blogLike = _mapper.Map<BlogLike>(request);
        //    _unitOfWork.BlogLikeRepository.AddAsync(blogLike);
        //    _unitOfWork.Save();

        //    return _mapper.Map<BlogLikeResponse>(blogLike);
        //}

        //public async Task<bool> DeleteBlogLikeAsync(long id)
        //{
        //    var checkBlog = _unitOfWork.BlogLikeRepository.GetByID(id);


        //    if(checkBlog == null)
        //    { 
        //        throw new CustomException.DataNotFoundException($"Blog Like {id} not found !");
        //    }

        //    checkBlog.Status = true ? false : checkBlog.Status;

        //    _unitOfWork.BlogLikeRepository.UpdateAsync(checkBlog);
        //    _unitOfWork.Save();
        //    return true;
        //}

        public async Task<BlogLikeResponse> GetBlogLikeByIdAsync(long id)
        {
            var getById = _unitOfWork.BlogLikeRepository.GetByIdAsync(id);
            return _mapper.Map<BlogLikeResponse>(getById);
        }

        //public async Task<IEnumerable<BlogLikeResponse>> GetAllBlogLikeAsync()
        //{
        //    var listBlogLike = _unitOfWork.BlogLikeRepository.Get();
        //    return _mapper.Map<IEnumerable<BlogLikeResponse>>(listBlogLike);
        //}

        public async Task<BlogLikeResponse> UpdateBlogLikeAsync(BlogLikeRequest request)
        {
            var checkBlogLike = _unitOfWork.BlogLikeRepository
                                    .Get(filter: x => x.Blog.Id == request.BlogId
                                        && x.User.Id == request.UserId, includeProperties: "Blog")
                                    .FirstOrDefault();
            if (checkBlogLike == null)
            {
                var response = _mapper.Map(request, checkBlogLike);
                response.Status = true;
                await _unitOfWork.BlogLikeRepository.AddAsync(response);
                return _mapper.Map<BlogLikeResponse>(response);
            }
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
