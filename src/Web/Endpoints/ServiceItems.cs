using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.ServiceItems.Commands;
using AlphaVisa.Application.ServiceItems.Queries;
using AlphaVisa.Application.TodoItems.Commands.UpdateTodoItem;

namespace AlphaVisa.Web.Endpoints;

public class ServiceItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetServiceItemWithPagination)
            .MapPost(CreateServiceItem)
            .MapPut(UpdateServiceItem, "{id:guid}")
            .MapDelete(DeleteServiceItem, "{id:guid}");
    }

    public Task<PaginatedList<ServiceItemBriefDto>> GetServiceItemWithPagination(ISender sender, [AsParameters] GetServiceItemsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public Task<Guid> CreateServiceItem(ISender sender, CreateServiceItemCommand command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateServiceItem(ISender sender, Guid id, UpdateServiceItemCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteServiceItem(ISender sender, Guid id)
    {
        await sender.Send(new DeleteServiceItemCommand(id));
        return Results.NoContent();
    }
}
