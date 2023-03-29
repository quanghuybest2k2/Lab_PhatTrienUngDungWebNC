using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    // add services to container
    builder
        .ConfigureCors()
        .ConfigureNlog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}
var app = builder.Build();
{
    // configure the http request pipeline
    app.SetupRequestPipeline();
    // configure API endpoints
    app.MapAuthorEndpoints();
    app.Run();
}