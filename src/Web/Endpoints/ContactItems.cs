using AlphaVisa.Application.Common.Models;
using AlphaVisa.Application.ContactItems.Commands;
using AlphaVisa.Application.ContactItems.Dtos;
using AlphaVisa.Application.ContactItems.Queries;
using Microsoft.AspNetCore.Authorization;

namespace AlphaVisa.Web.Endpoints;

public class ContactItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetContactItemById, "{id:guid}")
            .MapGet(GetContactItemWithPagination)
            .MapPost(CreateContactItem)
            .MapPut(UpdateContactItem, "{id:guid}")
            .MapDelete(DeleteContactItem, "{id:guid}");
    }

    [AllowAnonymous]
    public Task<ContactItemBriefDto> GetContactItemById(ISender sender, Guid id)
    {
        var query = new GetContactItemByIdQuery(id);
        return sender.Send(query);
    }

    [AllowAnonymous]
    public Task<PaginatedList<ContactItemBriefDto>> GetContactItemWithPagination(ISender sender, [AsParameters] GetContactItemsWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public Task<Guid> CreateContactItem(ISender sender, CreateContactItem command)
    {
        return sender.Send(command);
    }

    public async Task<IResult> UpdateContactItem(ISender sender, Guid id, UpdateContactItem command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteContactItem(ISender sender, Guid id)
    {
        await sender.Send(new DeleteContactItem(id));
        return Results.NoContent();
    }
}
