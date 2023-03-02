using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;
        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }
        // Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();
            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }
            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        // Tìm Top N bai viết phổ biến được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        // Tìm bài viết có tên định danh là slug
        // và được đăng vào tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category) // Include lấy quan hệ 2 bảng, post quan hệ với bảng khác
                .Include(x => x.Author);
            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }
            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }
        // Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }
        // Kiếm tra xem tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(int postId, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        // Lấy danh sách từ khóa,thẻ và phân trang theo các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });
            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        // Tìm một thẻ (Tag) theo tên định danh (slug)
        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(t => t.UrlSlug == slug, cancellationToken);
        }
        //Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết quả trả về kiểu IList<TagItem>.
        public async Task<IList<TagItem>> GetTagListAsync(CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                 .Select(x => new TagItem()
                 {
                     Id = x.Id,
                     Name = x.Name,
                     UrlSlug = x.UrlSlug,
                     Description = x.Description,
                     PostCount = x.Posts.Count(p => p.Published)
                 });
            return await tagQuery.ToListAsync(cancellationToken);
        }
        // Xóa một thẻ theo mã cho trước.
        public async Task<Tag> RemoveTagById(int id, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var tag = await _context.Set<Tag>()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (tag != null)
            {
                // Xóa thẻ khỏi cơ sở dữ liệu
                _context.Set<Tag>().Remove(tag);
                await _context.SaveChangesAsync(cancellationToken);
                Console.WriteLine("Đã xóa thành công!");
            }
            else
            {
                Console.WriteLine("Không có ID này!");
            }
            return tag;
        }
        //Tìm một chuyên mục (Category) theo tên định danh (slug). 
        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
        }
        // Tìm một chuyên mục theo mã số cho trước.
        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                 .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
        // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        public async Task<bool> AddOrUpdateCategory(Category category, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var category_id = await _context.Set<Category>()
                .FirstOrDefaultAsync(c => c.Id == category.Id, cancellationToken);
            if (category_id == null)
            {
                // Name = ".NET Core", Description = ".NET Core", UrlSlug="asp-dot-net-core", ShowOnMenu=true
                 _context.Set<Category>().Add(category);
            }
            else
            {
                Console.WriteLine("Đã tồn tại Category này!");
            }
            return true;
        }
    }
}
