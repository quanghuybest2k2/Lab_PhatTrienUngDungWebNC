using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services;
using TatBlog.Services.Blogs;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
                Keyword = keyword
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
        public async Task<IActionResult> Category(string slug)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug
            };
            var categoriesList = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", categoriesList);
        }
        public async Task<IActionResult> Author(string slug)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug
            };
            var authors = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", authors);
        }
        public async Task<IActionResult> Tag(string slug)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug
            };
            var authors = await _blogRepository.GetPagedPostsAsync(postQuery);
            ViewBag.PostQuery = postQuery;
            return View("Index", authors);
        }
        public async Task<IActionResult> Post(
                            int year = 2023,
                            int month = 1,
                            int day = 1,
                            string slug = null)
        {
            if (slug == null) return NotFound();

            var post = await _blogRepository.GetPostAsync(year, month, day, slug);

            if (post == null) return Content("Không tìm thấy bài viết nào");

            if (!post.Published)
            {
                ModelState.AddModelError("denied access", "Bài viết này không được phép truy cập");
                return View();
            }
            else
            {
                await _blogRepository.IncreaseViewCountAsync(post.Id);
            }

            ViewData["Comments"] = await _blogRepository.GetCommentPostIdAsync(post.Id);

            return View(post);
        }
        public async Task<IActionResult> Archives(int year, int month, [FromQuery(Name = "p")] int pageNumber = 1,
           [FromQuery(Name = "ps")] int pageSize = 5)
        {
            PostQuery postQuery = new PostQuery
            {
                Year = year,
                Month = month
            };

            var posts = await _blogRepository.GetPostsAsync(postQuery, pageNumber, pageSize);

            ViewData["PostQuery"] = postQuery;

            return View(posts);
        }
        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Rss() => Content("Nội dung sẽ được cập nhật");
    }
}
