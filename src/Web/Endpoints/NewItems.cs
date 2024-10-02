using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.NewItems.Commands;
using AlphaVisa.Application.NewItems.Queries;
using Microsoft.AspNetCore.Authorization;

namespace AlphaVisa.Web.Endpoints;

public class NewItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetNewItemWithPagination)
            .MapPost(CreateNewItem)
            .MapPut(UpdateNewItem, "{id:guid}")
            .MapDelete(DeleteNewItem, "{id:guid}");
    }

    [AllowAnonymous]
    public Task<PaginatedList<NewItemBriefDto>> GetNewItemWithPagination(ISender sender, [AsParameters] GetNewItemsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public Task<Guid> CreateNewItem(ISender sender, CreateNewItem command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateNewItem(ISender sender, Guid id, UpdateNewItem command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteNewItem(ISender sender, Guid id) 
    {
        await sender.Send(new DeleteNewItem(id));
        return Results.NoContent();
    }
}
