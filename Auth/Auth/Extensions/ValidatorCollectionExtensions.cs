using FluentValidation;
using System.Reflection;

namespace Auth.Extensions;

public static class ValidatorCollectionExtensions
{
    public static void AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)));

        foreach (var validatorType in validatorTypes)
        {
            var validatorInterfaceType = validatorType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

            if (validatorInterfaceType != null)
            {
                services.AddTransient(validatorInterfaceType, validatorType);
            }
            else
            {
                throw new ArgumentException($"Type {validatorType.Name} does not implement IValidator<T>");
            }
        }
    }
}
