﻿using AlphaVisa.Infrastructure.Identity;

namespace AlphaVisa.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroupForIdentity(this)
            .MapIdentityApi<ApplicationUser>();
    }
}
