using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class TagValidator: AbstractValidator<TagEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public TagValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Tên thẻ không được để trống")
            .MaximumLength(30)
            .WithMessage("Tên thẻ dài tối đa 30");

            RuleFor(a => a.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của thẻ không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa 1000");

            RuleFor(a => a.UrlSlug)
            .MustAsync(async (slug, cancellationToken) => !await _blogRepository.IsTagSlugExistedAsync(slug, cancellationToken))
            .WithMessage("Slug đã được sử dụng");

            RuleFor(a => a.Description)
            .NotEmpty()
            .WithMessage("Mô tả của thẻ không được để trống");
        }
    }
}
