namespace TatBlog.WebApi.Models
{
    public class TagFilterModel: PagingModel
    {
        public string Name { get; set; }
        public string Keyword { get; set; }
    }
}
