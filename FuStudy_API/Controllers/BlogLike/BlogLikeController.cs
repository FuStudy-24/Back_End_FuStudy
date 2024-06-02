using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FuStudy_API.Controllers.BlogLike
{
    [Route("api/[controller]")]
    [Controller]
    public class BlogLikeController : BaseController
    {
        private readonly IBlogLikeService _blogLikeService;
        public BlogLikeController(IBlogLikeService blogLikeService)
        {
            _blogLikeService = blogLikeService;
        }

        //[HttpGet("/get-all-blog-like")]
        //public async Task<IActionResult> GetAllBlogLike()
        //{
        //    var response = await _blogLikeService.GetAllBlogLikeAsync();
        //    if (response == null)
        //    {
        //        return CustomResult("Data not found", System.Net.HttpStatusCode.NotFound);
        //    }
        //    return CustomResult("Data loaded", response);
        //}

        //[HttpPost("/create-blog-like")]
        //public async Task<IActionResult> CreateBlogLike([FromBody] BlogLikeRequest request)
        //{
        //    try
        //    {
        //        var response = await _blogLikeService.CreateBlogLikeAsync(request);
        //        return CustomResult("Create successfully", response);
        //    }
        //    catch
        //    {
        //        return CustomResult("Error!", System.Net.HttpStatusCode.BadRequest);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateBlogLike([FromBody] BlogLikeRequest request)
        {
            try
            {
                var response = await _blogLikeService.UpdateBlogLikeAsync(request);
                return CustomResult("Update successfully", response);
            }
            catch
            {
                return CustomResult("Error!", System.Net.HttpStatusCode.BadRequest);
            }
        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBlogLike(long id)
        //{
        //    try
        //    {
        //        var response = _blogLikeService.DeleteBlogLikeAsync(id);
        //        return CustomResult("Delete Successfull (Status)", response, HttpStatusCode.OK);
        //    }
        //    catch
        //    {
        //        return CustomResult("Error!", System.Net.HttpStatusCode.BadRequest);
        //    }
        //}
    }
}
