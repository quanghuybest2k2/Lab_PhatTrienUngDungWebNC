using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class BestAuthors : ViewComponent
    {

        private readonly IBlogRepository _blogRepository;

        public BestAuthors(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authors = await _blogRepository.GetPopularAuthorsAsync(4);

            return View(authors);
        }
    }
}
