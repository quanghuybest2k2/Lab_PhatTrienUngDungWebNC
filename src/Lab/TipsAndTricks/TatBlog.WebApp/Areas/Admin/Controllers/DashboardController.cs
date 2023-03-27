using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IBlogRepository _blogRepository;

        public DashboardController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["TotalPosts"] = await _blogRepository.TotalPostsAsync();
            ViewData["TotalUnpublishedPosts"] = await _blogRepository.TotalUnpublishedPostsAsync();
            ViewData["TotalCategories"] = await _blogRepository.TotalCategoriesAsync();
            ViewData["TotalAuthors"] = await _blogRepository.TotalAuthorsAsync();
            ViewData["TotalWaitingApprovalComment"] = await _blogRepository.TotalWaitingApprovalCommentAsync();
            ViewData["TotalSubscriber"] = await _blogRepository.TotalSubscriberAsync();
            ViewData["TotalNewerSubscribeDay"] = await _blogRepository.TotalNewerSubscribeDayAsync();
            return View();
        }
    }
}
