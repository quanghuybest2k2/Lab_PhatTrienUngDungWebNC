using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        // Tìm bài viết có tên định danh là slug
        // và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);
        // Tìm Top N bai viết phổ biến được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);
        // Kiếm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);
        // Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        // Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false, CancellationToken cancellationToken = default);
        // Lấy danh sách từ khóa,thẻ và phân trang theo các tham số pagingParams
        Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
        // Tìm một thẻ (Tag) theo tên định danh (slug)
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
        //Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
        Task<IList<TagItem>> GetTagListAsync(CancellationToken cancellationToken = default);
        // Xóa một thẻ theo mã cho trước.
        Task<Tag> RemoveTagById(int id, CancellationToken cancellationToken = default);
        //Tìm một chuyên mục (Category) theo tên định danh (slug). 
        Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
        //Tìm một chuyên mục theo mã số cho trước.
        Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        Task<bool> AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default);

    }
}
