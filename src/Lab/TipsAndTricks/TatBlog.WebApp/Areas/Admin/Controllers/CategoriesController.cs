using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryEditModel> _categoryValidator;

        public CategoriesController(ILogger<CategoriesController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper, IValidator<CategoryEditModel> postValidator)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _categoryValidator = postValidator;
        }

        public async Task<IActionResult> Index(CategoryFilterModel model,
                                              [FromQuery(Name = "p")] int pageNumber = 1,
                                              [FromQuery(Name = "ps")] int pageSize = 10)
        {
            var categoryQuery = _mapper.Map<CategoryQuery>(model);

            ViewData["CategoriesList"] = await _blogRepository.GetCategoryByQueryAsync(categoryQuery, pageNumber, pageSize);

            ViewData["PagerQuery"] = new PagerQuery
            {
                Area = "Admin",
                Controller = "Categories",
                Action = "Index",
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id = 0)
        {
            var category = id > 0 ? await _blogRepository.GetCategoryByIdAsync(id) : null;

            var model = category == null ? new CategoryEditModel() : _mapper.Map<CategoryEditModel>(category);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryEditModel model)
        {
            var validator = HttpContext.RequestServices.GetService(typeof(IValidator<CategoryEditModel>));
            var validationResult = await _categoryValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
                return View(model);

            var category = model.Id > 0 ? await _blogRepository.GetCategoryByIdAsync(model.Id) : null;

            if (category == null)
            {
                category = _mapper.Map<Category>(model);

                category.Id = 0;
            }
            else
            {
                _mapper.Map(model, category);
            }

            await _blogRepository.AddOrUpdateCategoryAsync(category);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> VerifyCategorySlug(string urlSlug)
        {
            var slugExisted = await _blogRepository.IsCategorySlugExistedAsync(urlSlug);

            return slugExisted ? Json($"Slug '{urlSlug}' đã được sử dụng") : Json(true);
        }

        [HttpPost]
        public async Task<ActionResult> ShowedChanged(int categoryId)
        {
            await _blogRepository.ChangedCategoryStatusAsync(categoryId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _blogRepository.DeleteCategoryByIdAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
