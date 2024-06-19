using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Service.Interface
{
    public interface IBlogLikeService
    {
        Task<BlogLikeResponse> GetBlogLikeByIdAsync(long id);
        Task<BlogLikeResponse> CreateBlogLikeAsync(BlogLikeRequest request);
        Task<BlogLikeResponse> DeleteBlogLikeAsync(BlogLikeRequest request);
        Task<IEnumerable<BlogLikeResponse>> GetAllBlogLikeByBlogId(long id);
    }
}
