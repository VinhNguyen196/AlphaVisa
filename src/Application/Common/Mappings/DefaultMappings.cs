using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlphaVisa.Application.Common.Mappings;

public class ComplextObject : Dictionary<string, object> {
    public ComplextObject() : base()
    {
    }

    public static implicit operator string(ComplextObject? complexObject) => JsonSerializer.Serialize(complexObject) ?? string.Empty;
}

public class DefaultMappings : Profile
{
    public DefaultMappings()
    {
        CreateMap<string, ComplextObject>()
                .ConvertUsing(value => DeserializeDescription(value));
    }

    private static ComplextObject DeserializeDescription(string? descriptionJson)
    {
        return JsonSerializer.Deserialize<ComplextObject>(descriptionJson ?? "") ?? new();
    }
}
