using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMvc().ConfigureServices();
}

var app = builder.Build();
{
    app.UseRequestPipeLine();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}
// trang số 8, đừng thêm dấu chấm hỏi
app.Run();
