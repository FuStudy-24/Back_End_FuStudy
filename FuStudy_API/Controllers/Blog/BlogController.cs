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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneBlog(long id)
        {
            var response = await _blogService.GetOneBlog(id);
            if(response == null) { 
                return CustomResult("Data not found", System.Net.HttpStatusCode.NotFound);
            }
            return CustomResult("Data loaded", response);
        }

        [HttpPost("/create-blog")]
        public async Task<IActionResult> CreateBlog([FromBody] BlogRequest request)
        {
            var reponse = await _blogService.CreateBlog(request);
            if(!reponse)
            {
                return CustomResult("Create is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Create successfully", reponse);
        }

        [HttpPost("/update-blog")]
        public async Task<IActionResult> UpdateBlog([FromBody]BlogRequest request)
        {
            var reponse = await _blogService.UpdateBlog(request);
            if (!reponse)
            {
                return CustomResult("Update is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Update successfully", reponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOneBlog(long id)
        {
            var reponse = await _blogService.DeleteBlog(id);
            if (!reponse)
            {
                return CustomResult("Delete is not success !", System.Net.HttpStatusCode.BadRequest);
            }
            return CustomResult("Delete successfully", reponse);
        }
    }
}
