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
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public SubscriberRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
       // public async Task<IPagedList<Subscriber>> GetPagedSubscribersAsync(
       // IPagingParams pagingParams,
       //string name = null,
       //CancellationToken cancellationToken = default)
       // {
       //     return await _context.Set<Subscriber>()
       //         .AsNoTracking()
       //         .WhereIf(!string.IsNullOrWhiteSpace(name),
       //             x => x.FullName.Contains(name))
       //         .Select(a => new Subscriber()
       //         {
       //             Id = a.Id,
       //             FullName = a.FullName,
       //             Email = a.Email,
       //             JoinedDate = a.JoinedDate,
       //             ImageUrl = a.ImageUrl,
       //             UrlSlug = a.UrlSlug,
       //             PostCount = a.Posts.Count(p => p.Published)
       //         })
       //         .ToPagedListAsync(pagingParams, cancellationToken);
       // }
    }
}
