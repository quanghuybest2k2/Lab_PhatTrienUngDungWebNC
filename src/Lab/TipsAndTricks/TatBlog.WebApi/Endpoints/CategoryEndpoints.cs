using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CategoryEndpoints
    {
        public static WebApplication MapCategoryEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/categories");

            routeGroupBuilder.MapGet("/", GetCategory)
                            .WithName("GetCategory")
                            .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
                         .WithName("GetCategoryById")
                         .Produces<ApiResponse<CategoryItem>>()
                         .Produces(404);

            routeGroupBuilder.MapGet("/{slug::regex(^[a-z0-9_-]+$)}/posts", GetPostByCategorySlug)
                             .WithName("GetPostByCategorySlug")
                             .Produces<ApiResponse<PaginationResult<CategoryDto>>>();

            routeGroupBuilder.MapPost("/", AddCategory)
                             .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
                             .WithName("AddNewCategory")
                             .RequireAuthorization()
                             .Produces(401)
                             .Produces<ApiResponse<CategoryItem>>();

            //routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
            //                 .WithName("UpdateAuthor")
            //                 .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
            //                 .RequireAuthorization()
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
            //                 .WithName("DeleteAuthor")
            //                 .RequireAuthorization()
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapPost("/{id:int}/picture", SetAuthorPicture)
            //                 .WithName("SetAuthorPicture")
            //                 .RequireAuthorization()
            //                 .Accepts<IFormFile>("multipart/form-data")
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();
            //routeGroupBuilder.MapGet("/best/{limit:int}", GetBestAuthorsAsync)
            //             .WithName("GetBestAuthors")
            //             .Produces<PagedList<Author>>();

            return app;
        }
        // get category
        private static async Task<IResult> GetCategory([AsParameters] CategoryFilterModel model, ICategoryRepository categoryRepository)
        {
            var categoryList = await categoryRepository.GetPagedCategoryAsync(model, model.Name);

            var paginationResult = new PaginationResult<CategoryItem>(categoryList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        // get category detail
        private static async Task<IResult> GetCategoryDetails(int id, ICategoryRepository categoryRepository, IMapper mapper)
        {
            var category = await categoryRepository.GetCachedCategoryByIdAsync(id);

            return category == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy chuyên mục có mã số {id}"))
                :
                Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
        }
        // get post by category slug
        private static async Task<IResult> GetPostByCategorySlug([FromRoute] string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
        {
            var postQuery = new PostQuery
            {
                CategorySlug = slug,
                PublishedOnly = true
            };

            var categoryList = await blogRepository.GetPagedPostsAsync(postQuery, pagingModel, posts => posts.ProjectToType<CategoryDto>());

            var paginationResult = new PaginationResult<CategoryDto>(categoryList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        // them chuyen muc
        private static async Task<IResult> AddCategory(CategoryEditModel model, IValidator<CategoryEditModel> validator, ICategoryRepository categoryRepository, IMapper mapper)
        {
            if (await categoryRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
            }

            var category = mapper.Map<Category>(model);
            await categoryRepository.AddOrUpdateAsync(category);

            return Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
        }
    }
}
