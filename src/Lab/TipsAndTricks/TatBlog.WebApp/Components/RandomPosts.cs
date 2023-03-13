using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class RandomPosts:ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public RandomPosts(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.GetRandomArticlesAsync(5);
            return View(posts);
        }
    }
}
