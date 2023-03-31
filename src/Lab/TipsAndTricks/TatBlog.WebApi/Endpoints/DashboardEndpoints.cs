using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class DashboardEndpoints
    {
        public static WebApplication MapDashboardEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/dashboard");
            routeGroupBuilder.MapGet("/posts", GetTotalPosts)
                             .WithName("GetTotalPosts")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/posts/unpublished", UnpublishedPosts)
                             .WithName("UnpublishedPosts")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/categories", GetTotalCategory)
                             .WithName("GetTotalCategory")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/authors", GetTotalAuthors)
                             .WithName("GetTotalAuthors")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/comments/waitingforapprove", CommentWaitingForApprove)
                             .WithName("CommentWaitingForApprove")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/subscribers", GetTotalSubscriber)
                             .WithName("GetTotalSubscriber")
                             .Produces<ApiResponse<int>>();

            routeGroupBuilder.MapGet("/subscribers/newer", TotalNewerSubscribeDay)
                             .WithName("TotalNewerSubscribeDay")
                             .Produces<ApiResponse<int>>();

            return app;
        }
        private static async Task<IResult> GetTotalPosts(IBlogRepository blogRepository)
        {
            var totalPosts = await blogRepository.TotalPostsAsync();

            return Results.Ok(ApiResponse.Success(totalPosts));
        }
        private static async Task<IResult> UnpublishedPosts(IBlogRepository blogRepository)
        {
            var totalPosts = await blogRepository.TotalUnpublishedPostsAsync();

            return Results.Ok(ApiResponse.Success(totalPosts));
        }
        private static async Task<IResult> GetTotalCategory(IBlogRepository blogRepository)
        {
            var totalCategory = await blogRepository.TotalCategoriesAsync();

            return Results.Ok(ApiResponse.Success(totalCategory));
        }
        private static async Task<IResult> GetTotalAuthors(IBlogRepository blogRepository)
        {
            var totalAuthors = await blogRepository.TotalAuthorsAsync();

            return Results.Ok(ApiResponse.Success(totalAuthors));
        }
        private static async Task<IResult> CommentWaitingForApprove(IBlogRepository blogRepository)
        {
            var totalComments = await blogRepository.TotalWaitingApprovalCommentAsync();

            return Results.Ok(ApiResponse.Success(totalComments));
        }
        private static async Task<IResult> GetTotalSubscriber(IBlogRepository blogRepository)
        {
            var totalSubscriber = await blogRepository.TotalSubscriberAsync();

            return Results.Ok(ApiResponse.Success(totalSubscriber));
        }
        private static async Task<IResult> TotalNewerSubscribeDay(IBlogRepository blogRepository)
        {
            var totalNewerSubscribeDay = await blogRepository.TotalNewerSubscribeDayAsync();

            return Results.Ok(ApiResponse.Success(totalNewerSubscribeDay));
        }
    }
}
