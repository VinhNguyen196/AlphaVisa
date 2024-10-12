using AlphaVisa.Application.AVConfigurations.Commands;
using AlphaVisa.Application.AVConfigurations.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaVisa.Web.Endpoints;

public class AVConfigurations : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetAVConfiguration)
            .MapPut(UpdateAVConfiguration);
    }

    [AllowAnonymous]
    public Task<AVConfigurationBriefDto> GetAVConfiguration(ISender sender, [AsParameters] GetAVConfigurationQuery query)
    {
        return sender.Send(query);
    }

    public async Task<IResult> UpdateAVConfiguration(ISender sender, UpdateAVConfigurationCommand command)
    {
        await sender.Send(command);
        return Results.Ok();
    }
}
