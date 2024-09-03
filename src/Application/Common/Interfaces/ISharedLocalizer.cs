using Microsoft.Extensions.Localization;

namespace AlphaVisa.Application.Common.Interfaces;
public interface ISharedLocalizer
{
    public LocalizedString this[string key]
    {
        get;
    }

    LocalizedString GetLocalizedString(string key);
}
