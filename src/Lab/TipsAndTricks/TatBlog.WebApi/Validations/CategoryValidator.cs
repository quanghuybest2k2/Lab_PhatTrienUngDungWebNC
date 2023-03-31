using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryValidator()
    {
        RuleFor(a => a.Name)
        .NotEmpty()
        .WithMessage("Tên chuyên mục không được để trống")
        .MaximumLength(100)
        .WithMessage("Tên tác giả dài tối đa 100 kí tự");

        RuleFor(a => a.UrlSlug)
        .NotEmpty()
        .WithMessage("Slug của chuyên mục không được để trống")
        .MaximumLength(1000)
        .WithMessage("Slug dài tối đa 1000 kí tự");
    }
}
