using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
    public static class AuthorEndpoints
    {
        public static WebApplication MapAuthorEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/authors");

            routeGroupBuilder.MapGet("/", GetAuthors)
                            .WithName("GetAuthors")
                            .Produces<ApiResponse<PaginationResult<AuthorItem>>>();
            routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
                         .WithName("GetAuthorById")
                         .Produces<ApiResponse<AuthorItem>>()
                         .Produces(404);

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByAuthorSlug)
                             .WithName("GetPostByAuthorSlug")
                             .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddAuthor)
                             .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                             .WithName("AddNewAuthor")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<AuthorItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
                             .WithName("UpdateAuthor")
                             .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
                             .WithName("DeleteAuthor")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapPost("/{id:int}/picture", SetAuthorPicture)
                             .WithName("SetAuthorPicture")
                             .RequireAuthorization()
                             .Accepts<IFormFile>("multipart/form-data")
                             .Produces(401)
                             .Produces<ApiResponse<string>>();
            routeGroupBuilder.MapGet("/best/{limit:int}", GetBestAuthorsAsync)
                         .WithName("GetBestAuthors")
                         .Produces<PagedList<Author>>();

            return app;
        }
        private static async Task<IResult> GetAuthors([AsParameters] AuthorFilterModel model, IAuthorRepository authorRepository)
        {
            var authorList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

            var paginationResult = new PaginationResult<AuthorItem>(authorList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> GetAuthorDetails(int id, IAuthorRepository authorRepository, IMapper mapper)
        {
            var author = await authorRepository.GetCachedAuthorByIdAsync(id);

            return author == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy tác giả có mã số {id}"))
                :
                Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
        }
        private static async Task<IResult> GetPostByAuthor(int id,
            [AsParameters] PagingModel pagingModel,
            IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                AuthorId = id,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> GetPostByAuthorSlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                AuthorSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }

        private static async Task<IResult> AddAuthor(AuthorEditModel model, IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            await authorRepository.AddOrUpdateAsync(author);

            return Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
        }

        private static async Task<IResult> UpdateAuthor(int id, AuthorEditModel model, IValidator<AuthorEditModel> validator, IAuthorRepository authorRepository, IMapper mapper)
        {
            if (await authorRepository.IsAuthorSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var author = mapper.Map<Author>(model);
            author.Id = id;

            return await authorRepository.AddOrUpdateAsync(author) ? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Cound not find author"));

        }

        private static async Task<IResult> DeleteAuthor(int id, IAuthorRepository authorRepository)
        {
            return await authorRepository.DeleteAuthorAsync(id) ? Results.Ok(ApiResponse.Success("Author is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find author"));

        }

        private static async Task<IResult> SetAuthorPicture(int id, IFormFile imageFile, IAuthorRepository authorRepository, IMediaManager mediaManager)
        {
            var imageUrl = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Results.Ok(ApiResponse.Fail(
                    HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            await authorRepository.SetImageUrlAsync(id, imageUrl);

            return Results.Ok(ApiResponse.Success(imageUrl));
        }
        // lay limit
        private static async Task<IResult> GetBestAuthorsAsync(int limit, IAuthorRepository authorRepository)
        {
            var author = await authorRepository.TopAuthorPostAsync(limit);

            return Results.Ok(ApiResponse.Success(author));
        }
        //

    }
}
