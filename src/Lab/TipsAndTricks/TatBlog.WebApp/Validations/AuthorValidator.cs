using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
    public class AuthorValidator: AbstractValidator<AuthorEditModel>
    {
        private readonly IBlogRepository _blogRepository;
        public AuthorValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            RuleFor(a => a.FullName)
            .NotEmpty()
            .WithMessage("Tên tác giả không được để trống")
            .MaximumLength(500)
            .WithMessage("Tên tác giả dài tối đa 500");

            RuleFor(a => a.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug của tác giả không được để trống")
            .MaximumLength(1000)
            .WithMessage("Slug dài tối đa 1000");

            RuleFor(a => a.UrlSlug)
            .MustAsync(async (slug, cancellationToken) => !await _blogRepository.IsAuthorSlugExistedAsync(0, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

            RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Email của tác giả không được để trống");

            When(a => a.Id <= 0, () => {
                RuleFor(a => a.ImageFile)
                .Must(f => f is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
            })
            .Otherwise(() => {
                RuleFor(a => a.ImageFile)
        .MustAsync(SetImageIfNotExist)
        .WithMessage("Bạn phải chọn hình ảnh cho tác giả");
            });
        }
        private async Task<bool> SetImageIfNotExist(AuthorEditModel authorModel, IFormFile imageFile, CancellationToken cancellationToken)
        {
            // Lấy thông tin bài viết từ CSDL
            var post = await _blogRepository.GetPostByIdAsync(authorModel.Id, false, cancellationToken);
            // Nếu bài viết đã có hình ảnh => không bắt buộc chọn file
            if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
                return true;
            // Ngược lại (bài viết chưa có hình ảnh), kiểm tra xem người dùng đã chọn file hay chưa. Nếu chưa thì báo lỗi.
            return imageFile is { Length: > 0 };
        }
    }
}
