using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
    public class SubscriberValidator : AbstractValidator<SubscriberEditModel>
    {
        public SubscriberValidator()
        {
            RuleFor(a => a.SubscribeEmail)
            .NotEmpty()
            .WithMessage("Email của tác giả không được để trống")
            .EmailAddress()
            .WithMessage("Phải là một email");

            RuleFor(a => a.ReasonCancel)
            .NotEmpty()
            .WithMessage("Lý do huỷ không được để trống")
            .MaximumLength(500)
            .WithMessage("Tối đa 500 ký tự");

            RuleFor(a => a.AdminNotes)
            .MaximumLength(500)
            .WithMessage("Ghi chú dài tối đa 500 ký tự");
        }
    }
}
