using Microsoft.Extensions.Localization;

namespace AlphaVisa.Application.Common.Interfaces;
public interface ISharedLocalizer
{
    public LocalizedString this[string key]
    {
        get;
    }

    public string this[string key, params object[] args] 
    { 
        get;
    }

    LocalizedString GetLocalizedString(string key);
}
