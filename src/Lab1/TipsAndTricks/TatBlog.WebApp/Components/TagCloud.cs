using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class TagCloud : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;

        public TagCloud(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tags = await _blogRepository.GetTagListAsync();

            return View(tags);
        }
    }
}
