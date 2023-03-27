using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class SubscribersController : Controller
    {

        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public SubscribersController(IBlogRepository blogRepository, IMapper mapper, ILogger<PostsController> logger)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index(SubscriberFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var subscriberQuery = _mapper.Map<SubscriberQuery>(model);

            ViewData["SubscribersList"] = await _blogRepository.GetSubscriberByQueryAsync(subscriberQuery, pageNumber, pageSize);
            ViewData["PagerQuery"] = new PagerQuery
            {
                Area = "Admin",
                Controller = "Subscribers",
                Action = "Index",
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSubscriber(int id)
        {
            await _blogRepository.DeleteSubscriberAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
