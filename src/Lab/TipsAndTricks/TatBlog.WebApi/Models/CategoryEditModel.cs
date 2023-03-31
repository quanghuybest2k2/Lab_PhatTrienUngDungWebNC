using System.ComponentModel;

namespace TatBlog.WebApi.Models
{
    public class CategoryEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
