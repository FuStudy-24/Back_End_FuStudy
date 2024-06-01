using System.Threading.Tasks;
using CoreApiResponse;
using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FuStudy_API.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var response = await _blogService.GetAllBlog();
            if (response == null)
            {
                return CustomResult("Data not found", System.Net.HttpStatusCode.NotFound);
            }
            return CustomResult("Data loaded", response);
        }

        [HttpGet("{id}/{userId}")]
        public async Task<IActionResult> GetOneBlog(long id, long userId)
        {
            var response = await _blogService.GetOneBlog(id, userId);
            if(response == null) { 
                return CustomResult("Data not found", System.Net.HttpStatusCode.NotFound);
            }
            return CustomResult("Data loaded", response);
        }

        [HttpPost("/CreateBlog")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequest request)
        {
            var reponse = await _blogService.CreateBlog(request);
            if(reponse == null)
            {
                return CustomResult("Create is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Create successfully", reponse);
        }

        [HttpPost("/UpdateBlog/{id}")]
        public async Task<IActionResult> UpdateBlog(long id, [FromBody]BlogRequest request)
        {
            var reponse = await _blogService.UpdateBlog(id, request);
            if (reponse == null)
            {
                return CustomResult("Update is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Update successfully", reponse);
        }

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> DeleteOneBlog(long id, long userId)
        {
            var reponse = await _blogService.DeleteBlog(id, userId);
            if (!reponse)
            {
                return CustomResult("Delete is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Delete successfully", reponse);
        }
    }
}
