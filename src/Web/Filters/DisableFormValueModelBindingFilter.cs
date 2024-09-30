using Microsoft.AspNetCore.Mvc.ModelBinding;

public class DisableFormValueModelBindingFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var valueProviderFactories = context.HttpContext.RequestServices
            .GetRequiredService<IModelBinderFactory>()
            .GetType()
            .GetProperty("ValueProviderFactories")
            ?.GetValue(null) as IList<IValueProviderFactory>;

        if (valueProviderFactories != null)
        {
            // Remove specific form value providers, such as FormFile, Form, and JQuery providers
            valueProviderFactories.RemoveType<FormValueProviderFactory>();
            valueProviderFactories.RemoveType<FormFileValueProviderFactory>();
            valueProviderFactories.RemoveType<JQueryFormValueProviderFactory>();
        }

        // Proceed to the next middleware or filter
        return await next(context);
    }
}
