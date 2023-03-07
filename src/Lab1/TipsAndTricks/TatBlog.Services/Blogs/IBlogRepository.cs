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
        // Xóa một chuyên mục theo mã số cho trước
        Task<Category> RemoveCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        // Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa. 
        Task<bool> FindSlugExistedAsync(string slug, CancellationToken cancellationToken = default);
        // Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>.
        Task<IPagedList<CategoryItem>> Paginationcategory(IPagingParams pagingParams, CancellationToken cancellationToken);
        // Đếm số lượng bài viết trong N tháng gần nhất. N là tham số đầu vào. Kết
        // quả là một danh sách các đối tượng chứa các thông tin sau: Năm, Tháng, Số
        // bài viết.
        Task<int> CountObject_Valid_Condition_InPostQuery(PostQuery query, CancellationToken cancellationToken);
        // Thêm hay cập nhật một bài viết.
        Task<bool> AddOrUpdatePost(Post post, CancellationToken cancellationToken = default);
        // Tìm một bài viết theo mã số.
        Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default);
        // Chuyển đổi trạng thái Published của bài viết.
        Task<bool> ConvertStatusPublishedAsync(bool published, CancellationToken cancellationToken = default);
        // Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
        Task<IList<Post>> GetPostRandomsAsync(int numPosts, CancellationToken cancellationToken = default);
        //Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng
        // PostQuery(kết quả trả về kiểu IList<Post>).
        Task<IPagedList<Post>> GetPagedPostsByQueryAsync(IPostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        // Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong
        // đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        Task<IPagedList<Post>> FindAndPagination_Valid_Condition_InPostQuery(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken);
        // Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối
        // tượng PostQuery.

    }
}
