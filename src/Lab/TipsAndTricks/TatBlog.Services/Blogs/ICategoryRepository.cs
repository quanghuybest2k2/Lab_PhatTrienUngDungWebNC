using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ICategoryRepository
    {
        Task<IPagedList<CategoryItem>> GetPagedCategoryAsync(
         IPagingParams pagingParams,
          string name = null,
          CancellationToken cancellationToken = default);
        Task<Category> GetCachedCategoryByIdAsync(int categoryId);
        Task<bool> IsCategorySlugExistedAsync(
         int categoryId,
         string slug,
         CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdateAsync(
        Category category, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryAsync(
            int id, CancellationToken cancellationToken = default);

    }
}
