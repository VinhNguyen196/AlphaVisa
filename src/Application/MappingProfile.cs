using System.Reflection;
using AlphaVisa.SharedKernel.Abstractions.Mappers;
using AutoMapper.Internal;

namespace AlphaVisa.Application;
public class MappingProfile : Profile
{
    private const string InvokeMappingMethodName = nameof(IMapFrom<object>.Mapping);

    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().ToList().Exists(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(InvokeMappingMethodName) ?? type.GetInterface("IMapFrom`1")?.GetMethod(InvokeMappingMethodName);

            methodInfo?.Invoke(instance, [this]);
        }
    }
}
