using AutoMapper;

namespace AlphaVisa.SharedKernel.Abstractions.Mappers;
public interface IMapFrom<TSource>
{
    public void Mapping(Profile profile) => profile.CreateMap(typeof(TSource), GetType());
}
