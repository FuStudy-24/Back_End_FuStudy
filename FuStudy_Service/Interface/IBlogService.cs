using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogResponse>> GetAllBlog();
        Task<BlogResponse> CreateBlog(BlogRequest request);
        Task<bool> DeleteBlog(long id);
        Task<BlogResponse> GetOneBlog(long id);
        Task<BlogResponse> UpdateBlog(long id, BlogRequest request);

    }
}
