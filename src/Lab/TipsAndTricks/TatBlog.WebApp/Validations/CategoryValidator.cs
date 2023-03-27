using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public CategoryValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            RuleFor(p => p.Name)
              .NotEmpty()
              .WithMessage("Tên chủ đề không được để trống")
              .MaximumLength(500)
              .WithMessage("Tên chủ đề dài tối đa 500");

            RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Mô tả về chủ đề không được để trống");

            RuleFor(p => p.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của chủ đề không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa 1000");

            RuleFor(p => p.UrlSlug)
            .MustAsync(async (slug, cancellationToken) => !await _blogRepository.IsCategorySlugExistedAsync(slug, cancellationToken))
            .WithMessage("Slug đã được sử dụng");
        }
    }
}
