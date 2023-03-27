using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {

        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public CommentsController(IBlogRepository blogRepository, IMapper mapper, ILogger<PostsController> logger)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CommentFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var commentQuery = _mapper.Map<CommentQuery>(model);
            ViewData["CommentsList"] = await _blogRepository.GetCommentByQueryAsync(commentQuery, pageNumber, pageSize);
            ViewData["PagerQuery"] = new PagerQuery
            {
                Area = "Admin",
                Controller = "Comments",
                Action = "Index",
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CensoredChanged(string commentId)
        {
            await _blogRepository.ChangeCommentStatusAsync(Convert.ToInt32(commentId));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(string id)
        {
            await _blogRepository.DeleteCommentByIdAsync(Convert.ToInt32(id));

            return RedirectToAction(nameof(Index));
        }
    }
}
