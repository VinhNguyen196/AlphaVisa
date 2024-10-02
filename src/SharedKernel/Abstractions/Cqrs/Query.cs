using MediatR;

namespace AlphaVisa.SharedKernel.Abstractions.Cqrs;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
