using FluentValidation;
using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class CommentEndpoints
    {
        public static WebApplication MapCommentsEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/comments");
            routeGroupBuilder.MapGet("/", GetComments)
                             .WithName("GetComments")
                             .Produces<ApiResponse<PaginationResult<Comment>>>();

            routeGroupBuilder.MapGet("/{id:int}", GetCommentByPostId)
                             .WithName("GetCommentByPostId")
                             .Produces<ApiResponse<PaginationResult<Comment>>>();

            routeGroupBuilder.MapPost("/", AddComment)
                             .WithName("AddNewComment")
                             .AddEndpointFilter<ValidatorFilter<CommentEditModel>>()
                             .Produces(401)
                             .Produces<ApiResponse<Comment>>();

            routeGroupBuilder.MapDelete("/{id:int}", DeleteComment)
                             .WithName("DeleteComment")
                             .Produces(401)
                             .Produces<ApiResponse<string>>();

            return app;
        }
        private static async Task<IResult> GetComments([AsParameters] CommentFilterModel model, IBlogRepository blogRepository, IMapper mapper)
        {
            var commentQuery = mapper.Map<CommentQuery>(model);
            var commentList = await blogRepository.GetCommentByQueryAsync(commentQuery, model);

            var paginationResult = new PaginationResult<Comment>(commentList);

            return Results.Ok(ApiResponse.Success(paginationResult));
        }
        private static async Task<IResult> GetCommentByPostId(int id, ICommentRepository commentRepository, IMapper mapper)
        {
            var comment = await commentRepository.GetCachedCommentByIdAsync(id);

            return comment == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
                $"Không tìm thấy bình luận có mã số {id}"))
                :
                Results.Ok(ApiResponse.Success(mapper.Map<Comment>(comment)));
        }
        private static async Task<IResult> AddComment(CommentEditModel model, IValidator<CommentEditModel> validator, ICommentRepository commentRepository, IMapper mapper)
        {
            if (await commentRepository.IsCommentExistedAsync(0, model.Content))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Nội dung '{model.Content}' đã tồn tại"));
            }

            var comment = mapper.Map<Comment>(model);
            await commentRepository.AddOrUpdateAsync(comment);

            return Results.Ok(ApiResponse.Success(mapper.Map<Comment>(comment), HttpStatusCode.Created));
        }

        private static async Task<IResult> DeleteComment(int id, ICommentRepository commentRepository)
        {
            return await commentRepository.DeleteCommentAsync(id) ? Results.Ok(ApiResponse.Success("Comment is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find comment"));

        }

    }
}
