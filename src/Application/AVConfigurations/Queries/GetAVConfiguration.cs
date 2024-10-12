using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Domain.Entities;
using AlphaVisa.SharedKernel.Abstractions.Mappers;

namespace AlphaVisa.Application.AVConfigurations.Queries;
public record AVConfigurationBriefDto : IAuditableDto, IMapFrom<AVConfiguration>
{
    public string? Language { get; set; } = string.Empty;

    public string? Phone { get; set; } = string.Empty;

    public string? Email { get; set; } = string.Empty;

    public string? Address { get; set; } = string.Empty;

    public string? AddressLink { get; set; } = string.Empty;

    public string? SocialLink { get; set; } = string.Empty;

    public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? LastModified {  get; set; } = DateTimeOffset.UtcNow;
}

public record GetAVConfigurationQuery : IRequest<AVConfigurationBriefDto>;

public class GetAVConfigurationQueryHandler : IRequestHandler<GetAVConfigurationQuery, AVConfigurationBriefDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAVConfigurationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AVConfigurationBriefDto> Handle(GetAVConfigurationQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AVConfigurations
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<AVConfigurationBriefDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return entity ?? new AVConfigurationBriefDto();
    }
}
