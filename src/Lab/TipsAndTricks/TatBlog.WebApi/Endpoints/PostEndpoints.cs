using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlugGenerator;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class PostEndpoints
    {
        public static WebApplication MapPostEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/posts");

            routeGroupBuilder.MapGet("/", GetPosts)
                  .WithName("GetPosts")
                  .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapGet("/featured/{limit:int}", GetPopularArticles)
                         .WithName("GetPopularArticles")
                         .Produces<ApiResponse<IList<Post>>>()
                         .Produces(404);
            routeGroupBuilder.MapGet("/random/{limit:int}", GetPostRandoms)
                      .WithName("GetPostRandoms")
                      .Produces<ApiResponse<IList<Post>>>()
                      .Produces(404);
            routeGroupBuilder.MapGet("/archives/{limit:int}", GetPostArchives)
                     .WithName("GetPostArchives")
                     .Produces<ApiResponse<IList<MonthlyPostCountItem>>>()
                     .Produces(404);
            routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                       .WithName("GetPostDetails")
                       .Produces<ApiResponse<PostItem>>()
                       .Produces(404);
            routeGroupBuilder.MapPost("/", AddPost)
                      .WithName("AddNewPost")
                      .Accepts<PostEditModel>("multipart/form-data")
                      .Produces(401)
                      .Produces<ApiResponse<PostItem>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
                            .WithName("SetPostPicture")
                            .Accepts<IFormFile>("multipart/form-data")
                            .Produces(401)
                            .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                             .WithName("UpdatePost")
                             .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();
            routeGroupBuilder.MapGet("/{id:int}/comments", GetCommentPost)
                       .WithName("GetCommentPost")
                       .Produces(401)
                       .Produces<ApiResponse<PaginationResult<Comment>>>();
            routeGroupBuilder.MapGet("/get-posts-filter", GetFilteredPosts)
                .WithName("GetFilteredPost")
                .Produces<ApiResponse<PostDto>>();

            routeGroupBuilder.MapGet("/get-filter", GetFilter)
                .WithName("GetFilter")
                .Produces<ApiResponse<PostFilterModel>>();

            return app;
        }

        private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postsList = await blogRepository.GetPostByQueryAsync(postQuery, model, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        // lay top n bai viet xem nhieu nhat
        private static async Task<IResult> GetPopularArticles(int limit, IBlogRepository blogRepository, IMapper mapper)
        {
            var postsFeatured = await blogRepository.GetPopularArticlesAsync(limit);
            var postsDto = postsFeatured.Select(p => mapper.Map<PostDto>(p)).ToList();

            return Results.Ok(ApiResponse.Success(postsDto));
        }
        // lay ngau nhien
        private static async Task<IResult> GetPostRandoms(int limit, IBlogRepository blogRepository)
        {
            var postsRandom = await blogRepository.GetPostRandomsAsync(limit);

            return Results.Ok(ApiResponse.Success(postsRandom));
        }
        // thong ke
        private static async Task<IResult> GetPostArchives(int limit, IBlogRepository blogRepository)
        {
            var postsArchives = await blogRepository.CountMonthlyPostsAsync(limit);

            return Results.Ok(ApiResponse.Success(postsArchives));
        }
        // lay post chi tiet
        private static async Task<IResult> GetPostDetails(int id, IBlogRepository blogRepository, IMapper mapper)
        {
            var post = await blogRepository.GetCachedPostByIdAsync(id);

            return post == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy bài viết có mã số {id}"))
                :
                Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post)));
        }
        // lay post by slug
        //private static async Task<IResult> GetPostBySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        //{

        //}
        // them post
        private static async Task<IResult> AddPost(
              HttpContext context,
              IBlogRepository blogRepository,
              IMapper mapper,
              IMediaManager mediaManager)
        {
            var model = await PostEditModel.BindAsync(context);
            var slug = model.Title.GenerateSlug();
            if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
            {
                return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
            }
            var post = model.Id > 0 ? await
            blogRepository.GetPostByIdAsync(model.Id) : null;
            if (post == null)
            {
                post = new Post()
                {
                    PostedDate = DateTime.Now
                };
            }
            post.Title = model.Title;
            post.AuthorId = model.AuthorId;
            post.CategoryId = model.CategoryId;
            post.ShortDescription = model.ShortDescription;
            post.Description = model.Description;
            post.Meta = model.Meta;
            post.Published = model.Published;
            post.ModifiedDate = DateTime.Now;
            post.UrlSlug = model.Title.GenerateSlug();
            if (model.ImageFile?.Length > 0)
            {
                string hostname =
                $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/",
                uploadedPath = await
                mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(),
                model.ImageFile.FileName,
                model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(uploadedPath))
                {
                    post.ImageUrl = hostname + uploadedPath;
                }
            }
            await blogRepository.CreateOrUpdatePostAsync(post,
            model.GetSelectedTags());
            return Results.Ok(ApiResponse.Success(
            mapper.Map<PostItem>(post), HttpStatusCode.Created));
        }
        // set avatar
        private static async Task<IResult> SetPostPicture(int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await blogRepository.SetImageUrlAsync(id, imageUrl);

            return Results.Ok(ApiResponse.Success(imageUrl));
        }
        // cap nhat
        private static async Task<IResult> UpdatePost(int id, PostEditModel model, IValidator<PostEditModel> validator, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var post = mapper.Map<Post>(model);
            post.Id = id;

            return await blogRepository.AddOrUpdatePost(post) ? Results.Ok(ApiResponse.Success("Post is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Cound not find post"));

        }
        // get comments
        private static async Task<IResult> GetCommentPost(int id, ICommentRepository commentRepository)
        {
            var comments = await commentRepository.GetCommentPostIdAsync(id);

            var paginationResult = new PaginationResult<Comment>(comments);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        //
        private static async Task<IResult> GetFilter(
            IAuthorRepository authorRepository,
            IBlogRepository blogRepository)
        {
            var model = new PostFilterModel()
            {
                AuthorList = (await authorRepository.GetAuthorsAsync())
            .Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            }),
                CategoryList = (await blogRepository.GetCategoriesAsync())
            .Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            };
            return Results.Ok(ApiResponse.Success(model));
        }
        private static async Task<IResult> GetFilteredPosts(
        [AsParameters] PostFilterModel model,
        IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery()
            {
                Keyword = model.Keyword,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                Year = model.Year,
                Month = model.Month,
            };
            var postsList = await blogRepository.GetPagedPostsAsync(
            postQuery, model, posts =>
            posts.ProjectToType<PostDto>());
            var paginationResult = new PaginationResult<PostDto>(postsList);
            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        //

    }
}
