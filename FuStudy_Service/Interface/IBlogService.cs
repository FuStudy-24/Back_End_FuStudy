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
        Task<bool> CreateBlog(BlogRequest request);
        Task<bool> DeleteBlog(long id);
        Task<BlogResponse> GetOneBlog(long id);
        Task<bool> UpdateBlog(BlogRequest request);

    }
}
