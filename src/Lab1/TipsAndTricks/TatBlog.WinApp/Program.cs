using System.Data.Common;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

//---------------------------------------------------------------------------------
//// Tạo đối tượng DBContext để quản lý phiên làm việc
//// với CSDL và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Tạo đối tượng khởi tạo dữ liệu mẫu
//var seeders = new DataSeeder(context);
//// Gọi hàm initialize để nhập dữ liệu mẫu
//seeders.Initialize();
//// đọc danh sách tác giả từ csdl
//var authors = context.Authors.ToList();
//// Xuất danh sách tác giả ra màn hình
//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Date");
//foreach (var author in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}", author.Id, author.FullName, author.Email, author.JoinedDate);
//}
//---------------------------------------------------------------------------------
//// Tạo đối tượng DBContext để quản lý phiên làm việc
//// với CSDL và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Đọc danh sách bài viết từ CSDL
//// Lấy kém tên tác giả và chuyên mục
//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullName,
//        Category = p.Category.Name
//    })
//    .ToList();
////Xuất danh sách bài viết ra màn hình
//foreach (var post in posts)
//{
//    Console.WriteLine("ID               : {0}", post.Id);
//    Console.WriteLine("Title            : {0}", post.Title);
//    Console.WriteLine("ViewCount        : {0}", post.ViewCount);
//    Console.WriteLine("Date             : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author           : {0}", post.Author);
//    Console.WriteLine("Category         : {0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}

//---------------------------------------------------------------------------------
//// Tạo đối tượng DBContext để quản lý phiên làm việc
//// với CSDL và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Tạo đối tượng BlogRepository
//IBlogRepository blogRepo = new BlogRepository(context);
//// Tìm 3 bài viết được xem/đọc nhiều nhất
//var posts = await blogRepo.GetPopularArticlesAsync(3);

//// Xuất danh sách bài viết ra màn hình
//foreach (var post in posts)
//{
//    Console.WriteLine("ID               : {0}", post.Id);
//    Console.WriteLine("Title            : {0}", post.Title);
//    Console.WriteLine("ViewCount        : {0}", post.ViewCount);
//    Console.WriteLine("Date             : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author           : {0}", post.Author);
//    Console.WriteLine("Category         : {0}", post.Category);
//    Console.WriteLine("".PadRight(80, '-'));
//}
//-------------------------------------------------------------------------------------
//// tạo đối tượng dbcontext để quản lý phiên làm việc
//// với csdl và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Tạo đối tượng BlogRepository
//IBlogRepository blogRepo = new BlogRepository(context);
//// Lấy danh sách chuyên mục
//var categories = await blogRepo.GetCategoriesAsync();

//// Xuất ra màn hình
//foreach (var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}
//-------------------------------------------------------------------------------------
//// Tạo đối tượng DBContext để quản lý phiên làm việc
//// với CSDL và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Tạo đối tượng BlogRepository
//IBlogRepository blogRepo = new BlogRepository(context);
//var pagingParams = new PagingParams
//{
//    PageNumber = 1, //Lấy kết quả ở trang số 1
//    PageSize = 5, // Lấy 5 mẫu tin
//    SortColumn = "Name", // Sắp xếp theo tên
//    SortOrder = "DESC" // Theo chiều giảm dần
//};
//// Lấy danh sách từ khóa
//var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);
//// Xuất ra màn hình
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");
//foreach (var item in tagsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}
//-------------------------------------------------------------------------------------
//// Tạo đối tượng DBContext để quản lý phiên làm việc
//// với CSDL và trạng thái của đối tượng
//var context = new BlogDbContext();
//// Tạo đối tượng BlogRepository
//IBlogRepository blogRepo = new BlogRepository(context);

//var pagingParams = new PagingParams
//{
//    PageNumber = 1, //Lấy kết quả ở trang số 1
//    PageSize = 5, // Lấy 5 mẫu tin
//    SortColumn = "Name", // Sắp xếp theo tên
//    SortOrder = "DESC" // Theo chiều giảm dần
//};
//// Lấy danh sách từ khóa
//string slug = "Blazor.png";
//var tag = await blogRepo.GetTagBySlugAsync(slug);
//// Xuất ra màn hình
//Console.WriteLine("{0} {1} {2}", "Name", "Description", "UrlSlug");
//Console.WriteLine("{0} {1} {2}", tag.Id, tag.Name, tag.UrlSlug);
//-------------------------------------------------------------------------------------
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var tag = await blogRepo.GetTagListAsync();
//// Xuất ra màn hình
//foreach (var item in tag)
//{
//    Console.WriteLine("{0} {1} {2}", "Id", "Name", "PostCount");
//    Console.WriteLine("{0} {1} {2}", item.Id, item.Name, item.PostCount);
//}
//-------------------------------------------------------------------------------------
//// Xóa một thẻ theo mã cho trước.
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var tag = await blogRepo.RemoveTagById(1);
//-------------------------------------------------------------------------------------
////Tìm một chuyên mục (Category) theo tên định danh (slug).
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var pagingParams = new PagingParams
//{
//    PageNumber = 1, //Lấy kết quả ở trang số 1
//    PageSize = 5, // Lấy 5 mẫu tin
//    SortColumn = "Name", // Sắp xếp theo tên
//    SortOrder = "DESC" // Theo chiều giảm dần
//};
//// Lấy danh sách từ khóa
//string slug = "net.png";
//var category = await blogRepo.GetCategoryBySlugAsync(slug);
//// Xuất ra màn hình
//Console.WriteLine("{0} {1} {2}", "Name", "Description", "UrlSlug");
//Console.WriteLine("{0} {1} {2}", category.Id, category.Name, category.UrlSlug);
//-------------------------------------------------------------------------------------
////Tìm một chuyên mục (Category) theo tên định danh (slug).
//var context = new BlogDbContext();
//IBlogRepository blogRepo = new BlogRepository(context);
//var pagingParams = new PagingParams
//{
//    PageNumber = 1, //Lấy kết quả ở trang số 1
//    PageSize = 5, // Lấy 5 mẫu tin
//    SortColumn = "Name", // Sắp xếp theo tên
//    SortOrder = "DESC" // Theo chiều giảm dần
//};
//// Lấy danh sách từ khóa
//int id = 1;
//var category = await blogRepo.GetCategoryByIdAsync(id);
//// Xuất ra màn hình
//Console.WriteLine("{0} {1} {2}", "Name", "Description", "UrlSlug");
//Console.WriteLine("{0} {1} {2}", category.Id, category.Name, category.UrlSlug);
//-------------------------------------------------------------------------------------
//Tìm một chuyên mục (Category) theo tên định danh (slug).
var context = new BlogDbContext();
IBlogRepository blogRepo = new BlogRepository(context);
var pagingParams = new PagingParams
{
    PageNumber = 1, //Lấy kết quả ở trang số 1
    PageSize = 5, // Lấy 5 mẫu tin
    SortColumn = "Name", // Sắp xếp theo tên
    SortOrder = "DESC" // Theo chiều giảm dần
};
// Lấy danh sách từ khóa
var category = new Category();
category.Name = "Doan Quang Huy";
category.Name = "Huy can ne";
category.UrlSlug = "huy-can-ne-hi-hi";
// Name = ".NET Core", Description = ".NET Core", UrlSlug="asp-dot-net-core", ShowOnMenu=true
var categoryList = await blogRepo.AddOrUpdateCategory(category);
// Xuất ra màn hình
Console.WriteLine("{0} {1} {2}", "Name", "Description", "UrlSlug");
Console.WriteLine("{0} {1} {2}", category.Id, category.Name, category.UrlSlug);
//-------------------------------------------------------------------------------------
