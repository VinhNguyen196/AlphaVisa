using System.Reflection;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Web.Resources.ValidationMessages;
using Microsoft.Extensions.Localization;

namespace AlphaVisa.Web.Services;

public class SharedLocalizer : ISharedLocalizer
{
    private readonly IStringLocalizer _sharedResourceLocalizer;
    private readonly IStringLocalizer _validationMessagesLocalizer;

    public SharedLocalizer(IStringLocalizerFactory factory)
    {
        // Create localizer for SharedResource
        _sharedResourceLocalizer = factory.Create("SharedResource", "AlphaVisa.Web");

        // Create localizer for ValidationMessages
        _validationMessagesLocalizer = factory.Create("ValidationMessages.ValidationMessages", "AlphaVisa.Web");

    }

    public LocalizedString this[string key]
    {
        get
        {
            // Attempt to get the localized string from ValidationMessages
            var localizedString = _validationMessagesLocalizer[key];
            if (!localizedString.ResourceNotFound)
            {
                return localizedString;
            }

            // If not found, fallback to SharedResource
            return _sharedResourceLocalizer[key];
        }
    }

    public LocalizedString GetLocalizedString(string key)
    {
        return this[key];
    }
}
