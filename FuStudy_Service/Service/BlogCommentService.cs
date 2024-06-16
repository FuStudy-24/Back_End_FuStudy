using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Tools;

namespace FuStudy_Service.Service
{
    public class BlogCommentService : IBlogCommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogCommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BlogCommentResponse> CreateBlogComment(BlogCommentRequest request)
        {
            try
            {
                var checkBlog = _unitOfWork.BlogRepository.GetByID(request.BlogId);

                if (checkBlog == null)
                {
                    throw new CustomException.DataNotFoundException("Bad request! Blog doest not exist !");
                }

                if (request.CommentImage?.Image.Count() > 1)
                {
                    throw new CustomException.DataNotFoundException("Bad request! Just one image !");
                }

                var blogCmt = _mapper.Map<BlogComment>(request);
                blogCmt.CreateDate = DateTime.Now;
                blogCmt.ModifiedDate = blogCmt.CreateDate;
                blogCmt.Status = true;

                await _unitOfWork.BlogCommentRepository.AddAsync(blogCmt);
                var response = _mapper.Map<BlogCommentResponse>(blogCmt);

                if (request.CommentImage != null)
                {
                    request.CommentImage.BlogCommentId = blogCmt.Id;
                    var cmtImg = _mapper.Map<CommentImage>(request.CommentImage);
                    await _unitOfWork.CommentImageRepository.AddAsync(cmtImg);
                    var responsecmtImg = _mapper.Map<CommentImageResponse>(cmtImg);
                    response.CommentImage = responsecmtImg;
                    _unitOfWork.Save();
                }

                return response;
            }
            catch
            {
                throw new CustomException.DataNotFoundException("Error! Have some thing wrong at BlogComment !");
            }
        }

        public async Task<bool> DeleteBlogComment(long id)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    var getBlogComment = _unitOfWork.BlogCommentRepository.GetByID(id);
                    var getCommentImgExist = _unitOfWork.CommentImageRepository
                                             .Get(filter: x => x.BlogCommentId == id)
                                             .FirstOrDefault();

                    if (getCommentImgExist != null)
                    {
                        getCommentImgExist.Status = false;
                        _unitOfWork.CommentImageRepository.Update(getCommentImgExist);
                    }

                    getBlogComment.Status = false;
                    _unitOfWork.BlogCommentRepository.Update(getBlogComment);

                    _unitOfWork.Save();
                    transaction.Complete();
                    return true;
                }
            }
            catch
            {
                throw new CustomException.InvalidDataException("Invalid data! Please check.");
            }
        }

        public async Task<IEnumerable<BlogCommentResponse>> GetAllCommentABlog(QueryObject queryObject)
        {
            try
            {
                Expression<Func<Blog, bool>> filter = null;
                if (!string.IsNullOrWhiteSpace(queryObject.Search))
                {
                    filter = blogCmt => blogCmt.Id.ToString().Contains(queryObject.Search);
                }

                var checkBlog = _unitOfWork.BlogRepository.Get(
                                                filter: filter,
                                                pageIndex: queryObject.PageIndex,
                                                pageSize: queryObject.PageSize)
                                                        .FirstOrDefault();
                if (checkBlog == null)
                {
                    throw new CustomException.InvalidDataException("Bad request!");
                }
                var commentContent = _unitOfWork.BlogCommentRepository
                                .Get(filter: p => p.BlogId == checkBlog.Id && p.Status == true,
                                includeProperties: "User");

                var response = _mapper.Map<IEnumerable<BlogCommentResponse>>(commentContent);
                //CommentImg
                foreach (var img in response)
                {
                    var existImg = _unitOfWork.CommentImageRepository
                                    .Get(filter: x => x.BlogCommentId == img.Id && x.Status)
                                    .FirstOrDefault();

                    if (existImg != null)
                    {
                        var mapImg = _mapper.Map<CommentImageResponse>(existImg);
                        img.CommentImage = mapImg;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new CustomException.InvalidDataException(ex.Message, "Bad Request!");
            }
        }
        public async Task<BlogCommentResponse> UpdateBlogComment(BlogCommentRequest request, long id)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var getBlogComment = _unitOfWork.BlogCommentRepository.GetByID(id);
                var getCommentImgExist = _unitOfWork.CommentImageRepository
                                         .Get(filter: x => x.BlogCommentId == id)
                                         .FirstOrDefault();

                if (getCommentImgExist != null)
                {
                    getCommentImgExist.Image = request.CommentImage.Image;
                    _unitOfWork.CommentImageRepository.Update(getCommentImgExist);
                    _unitOfWork.Save();
                }

                getBlogComment.Comment = request.Comment;
                getBlogComment.ModifiedDate = DateTime.Now;
                _unitOfWork.BlogCommentRepository.Update(getBlogComment);
                _unitOfWork.Save();

                transaction.Complete();

                var response = _mapper.Map<BlogCommentResponse>(getBlogComment);
                response.CommentImage = _mapper.Map<CommentImageResponse>(getCommentImgExist);

                return response;
            }
        }
    }
}
