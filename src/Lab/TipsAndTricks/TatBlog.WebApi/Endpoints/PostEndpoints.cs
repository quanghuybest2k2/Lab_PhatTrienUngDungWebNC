using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
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
                            .Produces<ApiResponse<PaginationResult<Post>>>();

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
                       .Produces<ApiResponse<AuthorItem>>()
                       .Produces(404);
            routeGroupBuilder.MapPost("/", AddPost)
                             .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                             .WithName("AddPost")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<PostItem>>();
            routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
                            .WithName("SetPostPicture")
                            .RequireAuthorization()
                            .Accepts<IFormFile>("multipart/form-data")
                            .Produces(401)
                            .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdatePost)
                             .WithName("UpdatePost")
                             .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();
            routeGroupBuilder.MapGet("/{id:int}/comments", GetCommentPost)
                       .WithName("GetCommentPost")
                       .Produces(401)
                       .Produces<ApiResponse<PaginationResult<Comment>>>();

            return app;
        }
        private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            var postQuery = mapper.Map<PostQuery>(model);
            var postList = await blogRepository.GetPagedPostsAsync(postQuery, model, post => post.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        // lay top n bai viet xem nhieu nhat
        private static async Task<IResult> GetPopularArticles(int limit, IBlogRepository blogRepository)
        {
            var postsFeatured = await blogRepository.GetPopularArticlesAsync(limit);

            return Results.Ok(ApiResponse.Success(postsFeatured));
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
        private static async Task<IResult> AddPost(PostEditModel model, IValidator<PostEditModel> validator, IBlogRepository blogRepository, IMapper mapper)
        {
            if (await blogRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }
            var post = mapper.Map<Post>(model);
            await blogRepository.AddOrUpdatePost(post);

            return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
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
    }
}
