using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Respone;
using FuStudy_Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuStudy_API.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogRespone>> GetOneBlog(long id)
        {
            var response = await _blogService.GetOneBlog(id);
            return Ok(response);
        }

        [HttpPost("/create-blog")]
        public async Task<ActionResult<BlogRespone>> CreateBlog([FromBody] BlogRequest request)
        {
            await _blogService.CreateBlog(request);
            return Ok();
        }

        [HttpPost("/update-blog")]
        public async Task<ActionResult<BlogRespone>> UpdateBlog([FromBody]BlogRequest request)
        {
            await _blogService.UpdateBlog(request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogRespone>> DeleteOneBlog(long id)
        {
            await _blogService.DeleteBlog(id);
            return Ok();
        }
    }
}
