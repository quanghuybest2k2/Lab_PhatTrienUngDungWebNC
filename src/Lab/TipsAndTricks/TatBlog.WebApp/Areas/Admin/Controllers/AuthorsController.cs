using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorEditModel> _authorValidator;

        public AuthorsController(IBlogRepository blogRepository, IMapper mapper, IMediaManager mediaManager, ILogger<PostsController> logger, IValidator<AuthorEditModel> authorValidator)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _mediaManager = mediaManager;
            _logger = logger;
            _authorValidator = authorValidator;
        }

        public async Task<IActionResult> Index(AuthorFilterModel model, [FromQuery(Name = "p")] int pageNumber = 1, [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var authorQuery = _mapper.Map<AuthorQuery>(model);
            ViewData["AuthorsList"] = await _blogRepository.GetAuthorByQueryAsync(authorQuery, pageNumber, pageSize);
            ViewData["PagerQuery"] = new PagerQuery
            {
                Area = "Admin",
                Controller = "Authors",
                Action = "Index",
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var author = id > 0 ? await _blogRepository.GetCategoryByIdAsync(id) : null;
            var model = author == null ? new AuthorEditModel() : _mapper.Map<AuthorEditModel>(author);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AuthorEditModel model)
        {
            var validator = HttpContext.RequestServices.GetService(typeof(IValidator<AuthorEditModel>));
            var validationResult = await _authorValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var author = model.Id > 0 ? await _blogRepository.GetAuthorByIdAsync(model.Id) : null;

            if (author == null)
            {
                author = _mapper.Map<Author>(model);

                author.Id = 0;
            }
            else
            {
                _mapper.Map(model, author);
            }
            if (model.ImageFile?.Length > 0)
            {
                var newImagePath = await _mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);

                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _mediaManager.DeleteFileAsync(author.ImageUrl);
                    author.ImageUrl = newImagePath;
                }
            }
            await _blogRepository.AddOrUpdateAuthorAsync(author);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> VerifyAuthorSlug(int id, string urlSlug)
        {
            var slugExisted = await _blogRepository.IsAuthorSlugExistedAsync(id, urlSlug);

            return slugExisted ? Json($"Slug '{urlSlug}' đã được sử dụng") : Json(true);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            await _blogRepository.DeleteAuthorByIdAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
