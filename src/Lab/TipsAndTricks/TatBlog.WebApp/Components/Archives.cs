using Microsoft.AspNetCore.Mvc;
using System;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
    public class Archives : ViewComponent
    {
        private readonly IBlogRepository _blogRepository;
        public Archives(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _blogRepository.CountMonthlyPostsAsync(12);

            return View(posts);
        }
    }
}
