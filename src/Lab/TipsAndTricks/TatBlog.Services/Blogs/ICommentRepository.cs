using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICommentRepository
    {
        Task<IPagedList<Comment>> GetCommentPostIdAsync(int postId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<Comment> GetCachedCommentByIdAsync(int id);
        Task<bool> AddOrUpdateAsync(
       Comment comment, CancellationToken cancellationToken = default);
        Task<bool> DeleteCommentAsync(
            int id, CancellationToken cancellationToken = default);
        Task<bool> IsCommentExistedAsync(
            int id,
            string content,
            CancellationToken cancellationToken = default);
    }
}
