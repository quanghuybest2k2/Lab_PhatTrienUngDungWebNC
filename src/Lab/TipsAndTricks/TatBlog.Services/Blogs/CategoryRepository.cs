using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CategoryRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public async Task<IPagedList<CategoryItem>> GetPagedCategoryAsync(
         IPagingParams pagingParams,
          string name = null,
          CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    UrlSlug = a.UrlSlug,
                    Description = a.Description,
                    ShowOnMenu = a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        //
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>().FindAsync(categoryId);
        }
        public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{categoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryByIdAsync(categoryId);
                });
        }
        //
        public async Task<bool> IsCategorySlugExistedAsync(
       int categoryId,
       string slug,
       CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AnyAsync(x => x.Id != categoryId && x.UrlSlug == slug, cancellationToken);
        }
        // them hoac cap nhat
        public async Task<bool> AddOrUpdateAsync(
        Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
            {
                _context.Categories.Update(category);
                _memoryCache.Remove($"category.by-id.{category.Id}");
            }
            else
            {
                _context.Categories.Add(category);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        // xoa
        public async Task<bool> DeleteCategoryAsync(
            int id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
    }
}
