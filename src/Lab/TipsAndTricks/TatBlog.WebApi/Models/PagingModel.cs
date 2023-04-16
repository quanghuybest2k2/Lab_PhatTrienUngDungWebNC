using TatBlog.Core.Contracts;

namespace TatBlog.WebApi.Models
{
    public class PagingModel : IPagingParams
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
