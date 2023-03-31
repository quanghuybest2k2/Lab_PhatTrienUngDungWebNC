namespace TatBlog.WebApi.Models
{
    public class CommentFilterModel : PagingModel
    {
        public string UserName { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string PostTitle { get; set; }
        public bool Censored { get; set; }
    }
}
