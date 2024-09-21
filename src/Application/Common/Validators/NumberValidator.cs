using AlphaVisa.Application.Common.Interfaces;

namespace AlphaVisa.Application.Common.Validators;

public static class NumberValidator
{
    public static IRuleBuilderOptions<T, int> GreaterOrEqualTo<T>(this IRuleBuilder<T, int> ruleBuilder, ISharedLocalizer localizer, int threshold = 1)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(threshold).WithMessage("'{PropertyName}' " + localizer["GreaterOrEqualTo", threshold]);
    }
}
