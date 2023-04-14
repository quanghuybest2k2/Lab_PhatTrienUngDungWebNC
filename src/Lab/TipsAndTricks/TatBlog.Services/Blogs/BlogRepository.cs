using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SlugGenerator;
using System.Linq.Dynamic.Core;
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
        private readonly IMemoryCache _memoryCache;
        public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
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
                .Include(x => x.Tags)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        // Tìm bài viết có tên định danh là slug
        // và được đăng vào tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                                                      .Include(x => x.Category)
                                                      .Include(x => x.Author)
                                                      .Include(x => x.Tags);
            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (day > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Day == day);
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
        public async Task<bool> IsTagSlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().AnyAsync(t => t.UrlSlug.Equals(slug), cancellationToken);
        }
        public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>().AnyAsync(c => c.UrlSlug.Equals(slug), cancellationToken);
        }
        public async Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        // Lấy danh sách từ khóa,thẻ và phân trang theo các tham số pagingParams
        //public async Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        //{
        //    var tagQuery = _context.Set<Tag>()
        //        .Select(x => new TagItem()
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            UrlSlug = x.UrlSlug,
        //            Description = x.Description,
        //            PostCount = x.Posts.Count(p => p.Published)
        //        });
        //    return await tagQuery
        //        .ToPagedListAsync(pagingParams, cancellationToken);
        //}
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
        // Tìm một author theo mã số cho trước.
        public async Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
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
        public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(query).ToPagedListAsync(
                                    pageNumber,
                                    pageSize,
                                    nameof(Post.PostedDate),
                                    "DESC",
                                    cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(query).ToPagedListAsync(
                                            pagingParams,
                                            cancellationToken);
        }

        public async Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = mapper(FilterPosts(query));

            return await result.ToPagedListAsync(pagingParams, cancellationToken);
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

            if (condition.UnPublished)
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
        // loc tac gia
        private IQueryable<Author> FilterAuthors(AuthorQuery query)
        {
            IQueryable<Author> categoryQuery = _context.Set<Author>()
                                                           .Include(c => c.Posts);

            if (!string.IsNullOrWhiteSpace(query.UrlSlug))
            {
                categoryQuery = categoryQuery.Where(x => x.UrlSlug == query.UrlSlug);
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                categoryQuery = categoryQuery.Where(x => x.Email.Contains(query.Email));
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                categoryQuery = categoryQuery.Where(x => x.FullName.Contains(query.Keyword) ||
                             x.Notes.Contains(query.Keyword) ||
                             x.Posts.Any(p => p.Title.Contains(query.Keyword)));
            }

            return categoryQuery;
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
        //
        public async Task<Author> GetAuthorAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .FirstOrDefaultAsync(a => a.UrlSlug == slug, cancellationToken);
        }
        //
        public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .OrderBy(a => a.FullName)
                .Select(a => new AuthorItem()
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.ToString(),
                    JoinedDate = a.JoinedDate,
                    ImageUrl = a.ImageUrl,
                    UrlSlug = a.UrlSlug,
                    Notes = a.Notes,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        //
        public async Task<Post> GetPostByIdAsync(
            int postId, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
        }
        //
        public async Task<Tag> GetTagAsync(
        string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
        }

        public async Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        //
        public async Task<Post> CreateOrUpdatePostAsync(
            Post post, IEnumerable<string> tags,
            CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()//https://github.com/polischuk/SlugGenerator
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return post;
        }
        // Xóa một bài viết theo Id truyền vào
        public async Task<bool> DeletePostById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
        // change status
        public async Task ChangeStatusPushed(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
        }
        //
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
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
        //
        public async Task<bool> DeleteCategoryAsync(
            int categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _context.Set<Category>().FindAsync(categoryId);

            if (category is null) return false;

            _context.Set<Category>().Remove(category);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }
        //
        public async Task<Category> EditCategoryAsync(Category cat, CancellationToken cancellationToken = default)
        {
            var category = await _context.Set<Category>()
                .Include(p => p.Posts)
                .AnyAsync(x => x.Id == cat.Id, cancellationToken);
            if (category)
                _context.Entry(cat).State = EntityState.Modified;
            else
                await _context.Categories.AddAsync(cat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return cat;
        }
        //
        public async Task<Tag> GetTagByIdAsync(int tagId)
        {
            return await _context.Set<Tag>().FindAsync(tagId);
        }
        //
        public async Task<bool> DeleteTagAsync(
            int tagId, CancellationToken cancellationToken = default)
        {
            //var tag = await _context.Set<Tag>().FindAsync(tagId);

            //if (tag == null) return false;

            //_context.Set<Tag>().Remove(tag);
            //return await _context.SaveChangesAsync(cancellationToken) > 0;

            return await _context.Set<Tag>()
                .Where(x => x.Id == tagId)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
        // delete author
        public async Task<bool> DeleteAuthorByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            var author = await _context.Set<Author>().FindAsync(id);

            if (author is null) return await Task.FromResult(false);

            _context.Set<Author>().Remove(author);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }
        //
        public async Task<Tag> EditTagAsync(Tag newTag, CancellationToken cancellationToken = default)
        {
            var tag = await _context.Set<Tag>()
                .Include(p => p.Posts)
                .AnyAsync(x => x.Id == newTag.Id, cancellationToken);
            if (tag)
                _context.Entry(newTag).State = EntityState.Modified;
            else
                await _context.Tags.AddAsync(newTag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newTag;
        }
        // lay tac gia bang truy van
        public async Task<IPagedList<Author>> GetAuthorByQueryAsync(AuthorQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterAuthors(query).ToPagedListAsync(
                                    pageNumber,
                                    pageSize,
                                    nameof(AuthorQuery.FullName),
                                    "DESC",
                                    cancellationToken);
        }
        //
        public async Task<bool> AddOrUpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            if (author.Id > 0)
                _context.Update(author);
            else
                _context.Add(author);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        //
        public async Task<IPagedList<Category>> GetCategoryByQueryAsync(CategoryQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterCategory(query).ToPagedListAsync(
                                    pageNumber,
                                    pageSize,
                                    nameof(Category.Name),
                                    "DESC",
                                    cancellationToken);
        }
        private IQueryable<Category> FilterCategory(CategoryQuery query)
        {
            IQueryable<Category> categoryQuery = _context.Set<Category>()
                                                      .Include(c => c.Posts);

            if (query.ShowOnMenu)
            {
                categoryQuery = categoryQuery.Where(x => x.ShowOnMenu);
            }

            if (!string.IsNullOrWhiteSpace(query.UrlSlug))
            {
                categoryQuery = categoryQuery.Where(x => x.UrlSlug == query.UrlSlug);
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                categoryQuery = categoryQuery.Where(x => x.Name.Contains(query.Keyword) ||
                             x.Description.Contains(query.Keyword) ||
                             x.Posts.Any(p => p.Title.Contains(query.Keyword)));
            }

            return categoryQuery;
        }
        //
        public async Task AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
                _context.Update(category);
            else
                _context.Add(category);

            await _context.SaveChangesAsync(cancellationToken);
        }
        //
        public async Task ChangedCategoryStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Category>()
                              .Where(x => x.Id == id)
                              .ExecuteUpdateAsync(c => c.SetProperty(x => x.ShowOnMenu, x => !x.ShowOnMenu), cancellationToken);
        }
        //
        public async Task DeleteCategoryByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            if (id == null || _context.Categories == null)
            {
                Console.WriteLine("Không có danh mục nào");
                return;
            }
            var category = await _context.Set<Category>().FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(cancellationToken);

                Console.WriteLine($"Đã xóa danh mục với id {id}");
            }
        }
        //
        public async Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterComment(query).ToPagedListAsync(
                                              pageNumber,
                                              pageSize,
                                              nameof(Comment.PostDate),
                                              "DESC",
                                              cancellationToken);
        }
        public async Task<IPagedList<Comment>> GetCommentByQueryAsync(CommentQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await FilterComment(query).ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetCommentByQueryAsync<T>(CommentQuery query, IPagingParams pagingParams, Func<IQueryable<Comment>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = mapper(FilterComment(query));

            return await result.ToPagedListAsync(pagingParams, cancellationToken);
        }
        private IQueryable<Comment> FilterComment(CommentQuery query)
        {
            IQueryable<Comment> commentQuery = _context.Set<Comment>()
                                                           .Include(c => c.Post);

            if (query.Censored)
            {
                commentQuery = commentQuery.Where(x => x.Censored);
            }

            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                commentQuery = commentQuery.Where(x => x.UserName.Contains(query.UserName));
            }

            if (!string.IsNullOrWhiteSpace(query.PostTitle))
            {
                commentQuery = commentQuery.Where(x => x.Post.Title.Contains(query.PostTitle));
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                commentQuery = commentQuery.Where(x => x.Content.Contains(query.Keyword));
            }

            if (query.Year > 0)
            {
                commentQuery = commentQuery.Where(x => x.PostDate.Year == query.Year);
            }

            if (query.Month > 0)
            {
                commentQuery = commentQuery.Where(x => x.PostDate.Month == query.Month);
            }

            if (query.Day > 0)
            {
                commentQuery = commentQuery.Where(x => x.PostDate.Day == query.Day);
            }

            return commentQuery;
        }
        //
        public async Task ChangeCommentStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Comment>()
                              .Where(x => x.Id == id)
                              .ExecuteUpdateAsync(c => c.SetProperty(x => x.Censored, x => !x.Censored), cancellationToken);
        }
        //
        public async Task<bool> DeleteCommentByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Set<Comment>().FindAsync(id);

            if (comment is null) return await Task.FromResult(false);

            _context.Set<Comment>().Remove(comment);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }
        // tong post
        public async Task<int> TotalPostsAsync()
        {
            return await _context.Set<Post>().CountAsync();
        }
        //
        public async Task<int> TotalUnpublishedPostsAsync()
        {
            return await _context.Set<Post>().CountAsync(p => !p.Published);
        }
        //
        public async Task<int> TotalCategoriesAsync()
        {
            return await _context.Set<Category>().CountAsync();
        }
        //
        public async Task<int> TotalAuthorsAsync()
        {
            return await _context.Set<Author>().CountAsync();
        }
        //
        public async Task<int> TotalWaitingApprovalCommentAsync()
        {
            return await _context.Set<Comment>().CountAsync(c => !c.Censored);
        }
        //
        public async Task<int> TotalSubscriberAsync()
        {
            return await _context.Set<Subscriber>().CountAsync();
        }
        //
        public async Task<int> TotalNewerSubscribeDayAsync()
        {
            return await _context.Set<Subscriber>().CountAsync(s => s.SubDated.Day.Equals(DateTime.Now.Day));
        }
        //
        public async Task<IPagedList<Subscriber>> GetSubscriberByQueryAsync(SubscriberQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterSubscriber(query).ToPagedListAsync(
                                                  pageNumber,
                                                  pageSize,
                                                  nameof(Subscriber.SubDated),
                                                  "DESC",
                                                  cancellationToken);
        }
        private IQueryable<Subscriber> FilterSubscriber(SubscriberQuery query)
        {
            IQueryable<Subscriber> categoryQuery = _context.Set<Subscriber>();

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                categoryQuery = categoryQuery.Where(x => x.SubscribeEmail.Equals(query.Email));
            }

            if (query.ForceLock)
            {
                categoryQuery = categoryQuery.Where(x => x.ForceLock);
            }

            if (query.UnsubscribeVoluntary)
            {
                categoryQuery = categoryQuery.Where(x => x.UnsubscribeVoluntary);
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                categoryQuery = categoryQuery.Where(x => x.CancelReason.Contains(query.Keyword) ||
                             x.AdminNotes.Contains(query.Keyword));
            }

            return categoryQuery;
        }
        // delete subcriber
        public async Task<bool> DeleteSubscriberAsync(int id, CancellationToken cancellationToken = default)
        {
            var subscriber = await _context.Set<Subscriber>().FindAsync(id);

            if (subscriber is null)
                return await Task.FromResult(false);

            _context.Set<Subscriber>().Remove(subscriber);
            var affected = await _context.SaveChangesAsync(cancellationToken);

            return affected > 0;
        }

        //
        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
            PostQuery condition,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper)
        {
            var posts = FilterPosts(condition);
            var projectedPosts = mapper(posts);

            return await projectedPosts.ToPagedListAsync(pagingParams);
        }
        // lay chi tiet bài viết
        public async Task<Author> GetCachedPostByIdAsync(int postId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"author.by-id.{postId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetAuthorByIdAsync(postId);
                });
        }
        // set avatar
        public async Task<bool> SetImageUrlAsync(
       int id, string imageUrl,
       CancellationToken cancellationToken = default)
        {
            return await _context.Posts
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(a => a.ImageUrl, a => imageUrl),
                    cancellationToken) > 0;
        }
    }
}
