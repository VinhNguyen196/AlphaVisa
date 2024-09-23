using AlphaVisa.Application.CustomerRegistrations.Commands.RegisterBySendEmail;
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
    public Task<int> RegisterBySendMail(ISender sender, RegisterBySendMailCommand command)
    {
        return sender.Send(command);
    }
}
