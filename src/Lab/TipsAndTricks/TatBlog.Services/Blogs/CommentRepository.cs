using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CommentRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // comment data
        public async Task<IPagedList<Comment>> GetCommentPostIdAsync(int postId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var commentQuery = _context.Set<Comment>()
                                           .Where(c => c.PostID.Equals(postId));

            commentQuery = commentQuery.Where(c => c.Censored);

            return await commentQuery.ToPagedListAsync(pageNumber,
                                                       pageSize,
                                                       nameof(Comment.PostDate),
                                                       "DESC",
                                                       cancellationToken);
        }
        //
        // public async Task<IList<Comment>> GetCommentsAsync(
        //CancellationToken cancellationToken = default)
        // {
        //     return await _context.Set<Author>()
        //         .OrderBy(a => a.FullName)
        //         .Select(a => new AuthorItem()
        //         {
        //             Id = a.Id,
        //             FullName = a.FullName,
        //             Email = a.Email,
        //             JoinedDate = a.JoinedDate,
        //             ImageUrl = a.ImageUrl,
        //             UrlSlug = a.UrlSlug,
        //             PostCount = a.Posts.Count(p => p.Published)
        //         })
        //         .ToListAsync(cancellationToken);
        // }
        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Set<Comment>().FindAsync(id);
        }

        public async Task<Comment> GetCachedCommentByIdAsync(int id)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"comment.by-id.{id}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCommentByIdAsync(id);
                });
        }
        public async Task<bool> AddOrUpdateAsync(
       Comment comment, CancellationToken cancellationToken = default)
        {
            if (comment.Id > 0)
            {
                _context.Comments.Update(comment);
                _memoryCache.Remove($"comment.by-id.{comment.Id}");
            }
            else
            {
                _context.Comments.Add(comment);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        public async Task<bool> IsCommentExistedAsync(
           int id,
           string content,
           CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .AnyAsync(x => x.Id != id && x.Content == content, cancellationToken);
        }
        public async Task<bool> DeleteCommentAsync(
             int id, CancellationToken cancellationToken = default)
        {
            return await _context.Comments
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(cancellationToken) > 0;
        }
    }
}
