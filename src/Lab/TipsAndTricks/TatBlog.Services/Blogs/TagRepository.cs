using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public TagRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IPagedList<TagItem>> GetPagedTagssAsync(
        IPagingParams pagingParams,
       string name = null,
       CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new TagItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    UrlSlug = a.UrlSlug,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        public async Task<Tag> GetTagByIdAsync(int id)
        {
            return await _context.Set<Tag>().FindAsync(id);
        }
        // detail
        public async Task<Tag> GetCachedTagByIdAsync(int id)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"tag.by-id.{id}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetTagByIdAsync(id);
                });
        }
        public async Task<bool> IsTagSlugExistedAsync(
         int id,
         string slug,
         CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        // add or update
        public async Task<bool> AddOrUpdateAsync(
      Tag tag, CancellationToken cancellationToken = default)
        {
            if (tag.Id > 0)
            {
                _context.Tags.Update(tag);
                _memoryCache.Remove($"tag.by-id.{tag.Id}");
            }
            else
            {
                _context.Tags.Add(tag);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        // delete
        public async Task<bool> DeleteTagAsync(
      int id, CancellationToken cancellationToken = default)
        {
            return await _context.Tags
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
    }
}
