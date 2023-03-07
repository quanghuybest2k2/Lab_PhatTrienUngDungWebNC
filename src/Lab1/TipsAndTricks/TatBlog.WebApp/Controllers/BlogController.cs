using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Services;
using TatBlog.Services.Blogs;
namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index(
           [FromQuery(Name = "k")] string keyword = null,
           [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 5)
        {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                Keyword =  keyword
            };
            var pagingParams = new PagingParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };
           
            var postsList = await _blogRepository.GetPagedPostsByQueryAsync(postQuery, pagingParams);
            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
