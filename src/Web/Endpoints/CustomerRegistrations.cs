using AlphaVisa.Application.CustomerRegistrations.Commands.RegisterBySendMail;
using Microsoft.AspNetCore.Authorization;

namespace AlphaVisa.Web.Endpoints;
public class CustomerRegistrations : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(RegisterBySendMail);
    }

    [AllowAnonymous]
    public async Task<IResult> RegisterBySendMail(ISender sender, RegisterBySendMailCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
}
