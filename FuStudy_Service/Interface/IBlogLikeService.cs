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
        //Task<IEnumerable<BlogLikeResponse>> GetAllBlogLikeAsync();
        Task<BlogLikeResponse> GetBlogLikeByIdAsync(long id);
        Task<BlogLikeResponse> CreateBlogLikeAsync(BlogLikeRequest request);
        Task<BlogLikeResponse> UpdateBlogLikeAsync(BlogLikeRequest request);
        //Task<bool> DeleteBlogLikeAsync(long id);
    }
}
