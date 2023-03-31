namespace TatBlog.WebApi.Models
{
    public class CommentEditModel
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public bool Censored { get; set; }
    }
}
