using Microsoft.Extensions.DependencyInjection;

namespace ScoBro.Guards;

public static class GuardRegistrationExtensions
{
    public static IServiceCollection AddValidatorsFromAssemblyContaining<T>(this IServiceCollection services)
    {
        var validatorBaseType = typeof(Validator<>);
        var validatorInterfaceType = typeof(IValidator<>);
        var assembly = typeof(T).Assembly;

        var validators = assembly.GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsGenericTypeDefinition &&
                t.BaseType != null &&
                t.BaseType.IsGenericType &&
                t.BaseType.GetGenericTypeDefinition() == validatorBaseType
            );

        foreach (var validatorType in validators)
        {
            var validatedType = validatorType.BaseType.GetGenericArguments()[0];
            var interfaceType = validatorInterfaceType.MakeGenericType(validatedType);

            services.AddScoped(interfaceType, validatorType);
            services.AddScoped(validatorType, validatorType);
        }

        return services;
    }
}