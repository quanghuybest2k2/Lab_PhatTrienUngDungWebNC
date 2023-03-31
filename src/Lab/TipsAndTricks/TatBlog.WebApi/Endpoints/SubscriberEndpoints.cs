using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
    public static class SubscriberEndpoints
    {
        public static WebApplication MapSubscriberEndpoints(this WebApplication app)
        {
            var routeGroupBuilder = app.MapGroup("/api/subscribers");

            //routeGroupBuilder.MapGet("/", GetSubscribers)
            //                 .WithName("GetSubscribers")
            //                 .Produces<ApiResponse<PaginationResult<Subscriber>>>();

            //routeGroupBuilder.MapGet("/{id:int}", GetSubscriberDetails)
            //                 .WithName("GetSubscriberById")
            //                 .Produces<ApiResponse<Subscriber>>();

            //routeGroupBuilder.MapGet("/email/{email}", GetSubscriberByEmailDetails)
            //                 .WithName("GetSubscriberByEmail")
            //                 .Produces<ApiResponse<Subscriber>>();

            //routeGroupBuilder.MapPost("/", Subscribe)
            //                 .WithName("NewSubscriber")
            //                 .AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
            //                 .Produces(401)
            //                 .Produces<ApiResponse<Subscriber>>();

            //routeGroupBuilder.MapPost("/unsub/{email}", Unsubscribe)
            //                 .WithName("NewUnsubscriber")
            //                 .AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapDelete("/{id:int}", DeleteSubscriber)
            //                 .WithName("DeleteSubscriber")
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();

            //routeGroupBuilder.MapPost("/{id:int}", BlockSubscriber)
            //                 .WithName("BlockSubscriber")
            //                 .AddEndpointFilter<ValidatorFilter<SubscriberEditModel>>()
            //                 .Produces(401)
            //                 .Produces<ApiResponse<string>>();

            return app;
        }
        // get sub
        //private static async Task<IResult> GetSubscribers([AsParameters] SubscriberFilterModel model, ISubscriberRepository subscriberRepository)
        //{
        //    var subscriberList = await subscriberRepository.GetPagedAuthorsAsync(model, model.Name);

        //    var paginationResult = new PaginationResult<Subscriber>(subscriberList);

        //    return Results.Ok(ApiResponse.Success(paginationResult));
        //}

    }
}
