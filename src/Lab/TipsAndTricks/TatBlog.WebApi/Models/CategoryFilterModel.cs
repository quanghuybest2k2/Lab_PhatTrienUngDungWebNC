using System.ComponentModel;

namespace TatBlog.WebApi.Models
{
    public class CategoryFilterModel: PagingModel
    {
        public string Name { get; set; }
        public string UrlSlug { get; set; }
        public string Description { get; set; }
    }
}
