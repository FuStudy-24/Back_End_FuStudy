using AutoMapper;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Service
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateBlog(BlogRequest request)
        {
            try
            {
                request.CreateDate = DateTime.Now;
                var respone = _mapper.Map<Blog>(request);
                await _unitOfWork.BlogRepository.AddAsync(respone);
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeleteBlog(long id)
        {
            var getBlogById = GetOneBlog(id);
            if (getBlogById != null)
            {
                _unitOfWork.BlogRepository.Delete(getBlogById);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public async Task<BlogRespone> GetOneBlog(long id)
        {
            var getOneBlog = _unitOfWork.BlogRepository.GetByID(id);
            if (getOneBlog == null)
            {
                throw new InvalidDataException("Blog is error");
            }
            var respone = _mapper.Map<BlogRespone>(getOneBlog);
            return respone;
        }

        public async Task<bool> UpdateBlog(BlogRequest request)
        {
            try
            {
                request.CreateDate = DateTime.Now;
                var respone = _mapper.Map<Blog>(request);
                await _unitOfWork.BlogRepository.UpdateAsync(respone);
                _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
