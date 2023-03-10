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
            var categoryQuery = await _context.Set<Category>().SingleOrDefaultAsync(c => c.Id.Equals(category.Id), cancellationToken);
            if (categoryQuery != null)
            {
                categoryQuery.Name = category.Name;
                categoryQuery.Description = category.Description;
                categoryQuery.UrlSlug = category.UrlSlug;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                await _context.Set<Category>().AddAsync(category, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
        // Xóa một chuyên mục theo mã số cho trước
        public async Task<Category> RemoveCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var category = await _context.Set<Category>()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            if (category != null)
            {
                // Xóa chuyen muc khỏi cơ sở dữ liệu
                _context.Set<Category>().Remove(category);
                await _context.SaveChangesAsync(cancellationToken);
                Console.WriteLine("Đã xóa thành công!");
            }
            else
            {
                Console.WriteLine("Không có ID này!");
            }
            return category;
        }
        // Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>.

        // Đếm số lượng bài viết trong N tháng gần nhất. N là tham số đầu vào. Kết
        // quả là một danh sách các đối tượng chứa các thông tin sau: Năm, Tháng, Số
        // bài viết.
        public async Task<int> CountObject_Valid_Condition_InPostQuery(PostQuery query, CancellationToken cancellationToken)
        {
            IQueryable<Post> postQuery = _context.Set<Post>().Include(x => x.Tags);
            if (query.Year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == query.Year);
            }
            if (query.Month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == query.Month);
            }
            if (query.AuthorId > 0)
            {
                postQuery = postQuery.Where(x => x.AuthorId == query.AuthorId);
            }
            if (query.CategoryId > 0)
            {
                postQuery = postQuery.Where(x => x.CategoryId == query.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(query.TagSlug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == query.TagSlug);
            }

            return await postQuery.CountAsync(cancellationToken);

        }
        // Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa. 
        public async Task<bool> FindSlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var category_slug = await _context.Set<Category>()
                .FirstOrDefaultAsync(c => c.UrlSlug == slug, cancellationToken);
            if (category_slug != null)
            {
                Console.WriteLine($"Đã tồn tại slug: {slug} này!");
            }
            else
            {
                Console.WriteLine($"Chưa có slug: {slug}.");
            }
            return true;
        }
        // Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu IPagedList<CategoryItem>.
        public async Task<IPagedList<CategoryItem>> Paginationcategory(IPagingParams pagingParams, CancellationToken cancellationToken)
        {
            var tagQuery = _context.Set<Category>()
                                     .Select(x => new CategoryItem()
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         UrlSlug = x.UrlSlug,
                                         Description = x.Description,
                                         ShowOnMenu = x.ShowOnMenu,
                                         PostCount = x.Posts.Count(p => p.Published)
                                     });
            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }
        // Đếm số lượng bài viết trong N tháng gần nhất. N là tham số đầu vào. Kết  quả là một danh sách các đối tượng chứa
        // các thông tin sau: Năm, Tháng, Số bài viết.
        // Thêm hay cập nhật một bài viết.
        public async Task<bool> AddOrUpdatePost(Post post, CancellationToken cancellationToken = default)
        {
            var postQuery = await _context.Set<Post>().SingleOrDefaultAsync(p => p.Id.Equals(post.Id), cancellationToken);
            if (postQuery != null)
            {
                postQuery.Title = post.Title;
                postQuery.ShortDescription = post.ShortDescription;
                postQuery.Description = post.Description;
                postQuery.Meta = post.Meta;
                postQuery.UrlSlug = post.UrlSlug;
                postQuery.ImageUrl = post.ImageUrl;
                postQuery.ViewCount = post.ViewCount;
                postQuery.Published = post.Published;
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            else
            {
                await _context.Set<Post>().AddAsync(post, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
        // Tìm một bài viết theo mã số.
        public async Task<Post> FindPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (post == null)
            {
                Console.WriteLine("Không thấy bài viết này");
            }
            return post;
        }
        // Chuyển đổi trạng thái Published của bài viết.
        public async Task<bool> ConvertStatusPublishedAsync(bool published, CancellationToken cancellationToken = default)
        {
            // Tìm thẻ theo ID
            var post = await _context.Set<Post>()
                .FirstOrDefaultAsync(p => p.Published == published, cancellationToken);
            if (post.Published == true)
            {
                post.Published = false;
            }
            else
            {
                post.Published = true;
            }
            return true;
        }
        // Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
        public async Task<IList<Post>> GetPostRandomsAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            var random = new Random();
            return await _context.Set<Post>().OrderBy(x => Guid.NewGuid()).Take(numPosts).ToListAsync(cancellationToken);

        }
        //Tìm tất cả bài viết thỏa mãn điều kiện tìm kiếm được cho trong đối tượng
        // PostQuery(kết quả trả về kiểu IList<Post>).

        public async Task<IPagedList<Post>> GetPagedPostsByQueryAsync(IPostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var categoryQuery = _context.Set<Post>()
                    .Include(a => a.Author)
                    .Include(c => c.Category)
                    .Include(t => t.Tags)
                    .Where(x => x.Published && (
                        x.AuthorId == query.AuthorId ||
                        x.CategoryId == query.CategoryId ||
                        x.PostedDate.Year == query.Year ||
                        x.PostedDate.Month == query.Month ||
                        (!string.IsNullOrWhiteSpace(query.CategorySlug) &&
                            x.Category.UrlSlug.Contains(query.CategorySlug)) ||
                        (!string.IsNullOrWhiteSpace(query.AuthorSlug) &&
                            x.Author.UrlSlug.Contains(query.AuthorSlug)) ||
                        (!string.IsNullOrWhiteSpace(query.TagSlug) &&
                            x.Tags.Any(t => t.UrlSlug.Contains(query.TagSlug))) ||
                        (!string.IsNullOrWhiteSpace(query.Keyword) &&
                            x.Title.ToLower().Contains(query.Keyword.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(query.Keyword) &&
                            x.ShortDescription.ToLower().Contains(query.Keyword.ToLower())) ||
                        (!string.IsNullOrWhiteSpace(query.Keyword) &&
                            x.Description.ToLower().Contains(query.Keyword.ToLower()))));
                return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
            }
            else
            {
                var categoryQuery = _context.Set<Post>()
                    .Include(a => a.Author)
                    .Include(c => c.Category)
                    .Include(t => t.Tags)
                    .Where(x => x.Published);
                return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
            }
        }
        // Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong
        // đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        //
        public async Task<IPagedList<Post>> GetPagedPostsAsync(
        PostQuery condition,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }
        // lọc bài viết
        public IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;
        }
        //để hiển thị danh sách bài viết được đăng
        //trong tháng và năm đã chọn(do người dùng click chuột vào các tháng
        //trong view component Archives ở bài tập 3). 
        public async Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition)
                .OrderByDescending(x => x.PostedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        // Hiển thị TOP 5 bài viết ngẫu nhiên. Người dùng có thể click  chuột để xem chi tiết.
        public async Task<IList<Post>> GetRandomArticlesAsync(
        int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .OrderBy(x => Guid.NewGuid())
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        // top n tac gia co nhieu bai viet nhat
        public async Task<IList<Author>> GetPopularAuthorsAsync(int numAuthor, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderByDescending(p => p.Posts)
                .Take(numAuthor)
                .ToListAsync(cancellationToken);
        }
        // dem thang ngay bai viet
        public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(int numMonths, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(x => x.Published)
                .GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
                .Select(g => new MonthlyPostCountItem()
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    PostCount = g.Count(x => x.Published)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync(cancellationToken);
        }
    }
}
