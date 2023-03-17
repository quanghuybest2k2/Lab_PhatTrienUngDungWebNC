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
    public class TagsController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly IValidator<TagEditModel> _tagValidator;

        public TagsController(ILogger<CategoriesController> logger, IBlogRepository blogRepository, IMediaManager mediaManager, IMapper mapper, IValidator<TagEditModel> tagValidator)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _mediaManager = mediaManager;
            _mapper = mapper;
            _tagValidator = tagValidator;
        }

        public async Task<IActionResult> Index(
            TagFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10)
        {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var tagQuery = _mapper.Map<TagQuery>(model);

            _logger.LogInformation("Lấy danh sách thẻ từ CSDL");

            var pagingParams = new PagingParams()
            {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.TagsList = await _blogRepository.GetPagedTagsAsync(pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id = 0)
        {
            var tag = id > 0 ? await _blogRepository.GetTagByIdAsync(id) : null;
            var model = tag == null ? new TagEditModel() : _mapper.Map<TagEditModel>(tag);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditModel model)
        {
            var validationResult = await _tagValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tag = model.Id > 0 ? await _blogRepository.GetTagByIdAsync(model.Id) : null;

            if (tag == null)
            {
                tag = _mapper.Map<Tag>(model);
                tag.Id = 0;
            }
            else
            {
                _mapper.Map(model, tag);
            }

            await _blogRepository.EditTagAsync(tag);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteTag(int id)
        {
            await _blogRepository.DeleteTagAsync(id);

            return RedirectToAction("Index");
        }
    }
}
