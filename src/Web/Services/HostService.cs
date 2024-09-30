using AlphaVisa.Application.Common.Interfaces;

namespace AlphaVisa.Web.Services;

public class HostService : IHostService
{
    private readonly IConfiguration _configuration;

    public HostService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    string IHostService.GetHostUrl()
    {
        var hostDomain = _configuration["Host:Domain"];
        var protocol = _configuration["Host:Protocol"];

        return $"{protocol}://{hostDomain}";
    }
}
