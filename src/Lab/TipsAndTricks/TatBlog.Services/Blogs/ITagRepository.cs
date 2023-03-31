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
    public interface ITagRepository
    {
        Task<IPagedList<TagItem>> GetPagedTagssAsync(
        IPagingParams pagingParams,
       string name = null,
       CancellationToken cancellationToken = default);
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> GetCachedTagByIdAsync(int id);
        Task<bool> IsTagSlugExistedAsync(
         int id,
         string slug,
         CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdateAsync(
      Tag tag, CancellationToken cancellationToken = default);
        Task<bool> DeleteTagAsync(
      int id, CancellationToken cancellationToken = default);

    }
}
