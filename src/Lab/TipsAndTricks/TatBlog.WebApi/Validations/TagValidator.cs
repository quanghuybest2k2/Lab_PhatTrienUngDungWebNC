using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class TagValidator : AbstractValidator<TagEditModel>
    {
        public TagValidator()
        {
            RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Tên tác giả không được để trống")
            .MaximumLength(100)
            .WithMessage("Tối đa 100 kí tự");

            RuleFor(a => a.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của tác giả không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa 1000 kí tự");
        }
    }

}
