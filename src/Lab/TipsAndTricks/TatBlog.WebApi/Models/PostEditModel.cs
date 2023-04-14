using System.ComponentModel;

namespace TatBlog.WebApi.Models
{
    public class PostEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tiêu đề")]
        public string Title { get; set; }
        [DisplayName("Giới thiệu")]
        public string ShortDescription { get; set; }
        [DisplayName("Nội dung")]
        public string Description { get; set; }
        [DisplayName("Metadata")]
        public string Meta { get; set; }
        [DisplayName("Slug")]
        public string UrlSlug { get; set; }
        [DisplayName("Chọn hình ảnh")]
        public IFormFile ImageFile { get; set; }
        [DisplayName("Hình hiện tại")]
        public string ImageUrl { get; set; }
        [DisplayName("Xuất bản ngay")]
        public bool Published { get; set; }
        [DisplayName("Tác giả")]
        public int AuthorId { get; set; }
        [DisplayName("Chủ đề")]
        public int CategoryId { get; set; }
        [DisplayName("Từ khoá (mỗi từ một dòng)")]
        public string SelectedTags { get; set; }

        // Tách chuỗi chứa các thẻ thành một mảng các chuỗi
        public List<string> GetSelectedTags()
        {
            return (SelectedTags ?? "").Split(new[] { ",", ";", ".", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static async ValueTask<PostEditModel> BindAsync(HttpContext context)
        {
            var form = await context.Request.ReadFormAsync();
            return new PostEditModel()
            {
                ImageFile = form.Files["ImageFile"],
                Id = int.Parse(form["Id"]),
                Title = form["Title"],
                ShortDescription = form["ShortDescription"],
                Description = form["Description"],
                Meta = form["Meta"],
                Published = form["Published"] != "false",
                CategoryId = int.Parse(form["CategoryId"]),
                AuthorId = int.Parse(form["AuthorId"]),
                SelectedTags = form["SelectedTags"]
            };
        }
    }
}
