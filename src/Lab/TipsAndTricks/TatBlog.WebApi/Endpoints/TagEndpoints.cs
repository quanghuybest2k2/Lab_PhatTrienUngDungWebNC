using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class TagEndpoints
    {
        public static WebApplication MapTagEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/tags");

            routeGroupBuilder.MapGet("/", GetTags)
                             .WithName("GetTags")
                             .Produces<ApiResponse<PaginationResult<TagItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetTagDetails)
                     .WithName("GetTagById")
                     .Produces<ApiResponse<TagItem>>()
                     .Produces(404);

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByTagSlug)
                             .WithName("GetPostByTagSlug")
                             .Produces<ApiResponse<PaginationResult<PostDto>>>();

            routeGroupBuilder.MapPost("/", AddTag)
                             .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
                             .WithName("AddNewTag")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<TagItem>>();

            routeGroupBuilder.MapPut("/{id:int}", UpdateTag)
                             .WithName("UpdateTag")
                             .AddEndpointFilter<ValidatorFilter<TagFilterModel>>()
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteTag)
                             .WithName("DeleteTag")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            return app;
        }
        // get tags
        private static async Task<IResult> GetTags([AsParameters] TagFilterModel model, ITagRepository tagRepository)
        {
            var tagList = await tagRepository.GetPagedTagssAsync(model, model.Name);

            var paginationResult = new PaginationResult<TagItem>(tagList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        // get tag detail
        private static async Task<IResult> GetTagDetails(int id, ITagRepository tagRepository, IMapper mapper)
        {
            var tag = await tagRepository.GetCachedTagByIdAsync(id);

            return tag == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy thẻ có mã số {id}"))
                :
                Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag)));
        }
        // get post by slug
        private static async Task<IResult> GetPostByTagSlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                TagSlug = slug,
                PublishedOnly = true
            };

            var postsList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

            var paginationResult = new PaginationResult<PostDto>(postsList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        // add tag
        private static async Task<IResult> AddTag(TagEditModel model, IValidator<TagEditModel> validator, ITagRepository tagRepository, IMapper mapper)
        {
            if (await tagRepository.IsTagSlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            await tagRepository.AddOrUpdateAsync(tag);

            return Results.Ok(ApiResponse.Success(mapper.Map<TagItem>(tag), HttpStatusCode.Created));
        }
        // update tag
        private static async Task<IResult> UpdateTag(int id, TagEditModel model, IValidator<TagEditModel> validator, ITagRepository tagRepository, IMapper mapper)
        {
            if (await tagRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = id;

            return await tagRepository.AddOrUpdateAsync(tag) ? Results.Ok(ApiResponse.Success("Tag is updated", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Cound not find tag"));

        }
        // delete
        private static async Task<IResult> DeleteTag(int id, ITagRepository tagRepository)
        {
            return await tagRepository.DeleteTagAsync(id) ? Results.Ok(ApiResponse.Success("Tag is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find tag"));

        }
    }
}
