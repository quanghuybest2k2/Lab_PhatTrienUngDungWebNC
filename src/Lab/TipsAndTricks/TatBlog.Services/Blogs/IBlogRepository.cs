﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default);
        // Tìm Top N bai viết phổ biến được nhiều người xem nhất
        Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default);
        // Kiếm tra xem tên định danh của bài viết đã có hay chưa
        Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default);
        // kiem tra tag slug ton tai chua
        Task<bool> IsTagSlugExistedAsync(string slug, CancellationToken cancellationToken = default);
        // kiem tra category slug ton tai chua
        Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default);
        // kiem tra author slug ton tai chua
        Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        // Tăng số lượt xem của một bài viết
        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        // Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false, CancellationToken cancellationToken = default);
        // Lấy danh sách từ khóa,thẻ và phân trang theo các tham số pagingParams
        //Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
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

        Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);

        // Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong
        // đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>) // Đếm số lượng bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối
        // tượng PostQuery.
        //IQueryable<Post> FilterPost(PostQuery pq);
        //để hiển thị danh sách bài viết được đăng
        //trong tháng và năm đã chọn(do người dùng click chuột vào các tháng
        //trong view component Archives ở bài tập 3). 
        Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
        // Hiển thị TOP 5 bài viết ngẫu nhiên. Người dùng có thể click  chuột để xem chi tiết.
        Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default);
        // top 4 authors has article
        Task<IList<Author>> GetPopularAuthorsAsync(int numAuthor, CancellationToken cancellationToken = default);
        // dem theo 12 thang gan nhat
        Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths, CancellationToken cancellationToken = default);
        //
        Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default);
        //
        Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);
        //
        Task<Post> GetPostByIdAsync(int postId, bool includeDetails = false, CancellationToken cancellationToken = default);
        //
        Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default);
        //
        Task<Post> CreateOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);
        //
        Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default);
        //
        Task ChangeStatusPushed(int id, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);
        //
        Task<bool> DeleteCategoryAsync(
           int categoryId, CancellationToken cancellationToken = default);
        //
        Task<Category> EditCategoryAsync(Category cat, CancellationToken cancellationToken = default);
        //
        Task<Tag> GetTagByIdAsync(int tagId);
        //
        Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams, CancellationToken cancellationToken = default);
        //
        Task<Tag> EditTagAsync(Tag newTag, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<Author>> GetAuthorByQueryAsync(AuthorQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        //
        Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default);
        //
        Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default);
        //
        Task<bool> DeleteAuthorByIdAsync(int? id, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<Category>> GetCategoryByQueryAsync(CategoryQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        //
        Task AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
        //
        Task ChangedCategoryStatusAsync(int id, CancellationToken cancellationToken = default);
        //
        Task DeleteCategoryByIdAsync(int? id, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<T>> GetCommentByQueryAsync<T>(CommentQuery query, IPagingParams pagingParams, Func<IQueryable<Comment>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);
        //
        Task ChangeCommentStatusAsync(int id, CancellationToken cancellationToken = default);
        //
        Task<bool> DeleteCommentByIdAsync(int? id, CancellationToken cancellationToken = default);
        //
        Task<int> TotalPostsAsync();
        //
        Task<int> TotalUnpublishedPostsAsync();
        //
        Task<int> TotalCategoriesAsync();
        //
        Task<int> TotalAuthorsAsync();
        //
        Task<int> TotalWaitingApprovalCommentAsync();
        //
        Task<int> TotalSubscriberAsync();
        //
        Task<int> TotalNewerSubscribeDayAsync();
        //
        Task<IPagedList<Subscriber>> GetSubscriberByQueryAsync(SubscriberQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        //
        Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default);
        //
        Task<IPagedList<T>> GetPagedPostsAsync<T>(
                 PostQuery condition,
                 IPagingParams pagingParams,
                 Func<IQueryable<Post>, IQueryable<T>> mapper);
        //
        Task<Post> GetCachedPostByIdAsync(int postId);
        //
        Task<bool> SetImageUrlAsync(
       int id, string imageUrl,
       CancellationToken cancellationToken = default);
    }
}
