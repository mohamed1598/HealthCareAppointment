using FluentValidation.Internal;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using System.Text;
using Shared.Result;

namespace Auth.Middlewares;

public class ValidatorMiddleware(RequestDelegate next, Assembly assembly)
{
    private readonly RequestDelegate _next = next;
    private readonly Assembly _assembly = assembly;

    public async Task Invoke(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await _next(context);
            return;
        }
        var endpoint = context.GetEndpoint();
        var actionDescriptor = endpoint?.Metadata.GetMetadata<ActionDescriptor>();
        string? requestTypeName = "";
        // Ensure the action descriptor is for a controller action
        if (actionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
             requestTypeName = controllerActionDescriptor.MethodInfo.GetParameters().FirstOrDefault()?.ToString().Split(" ").FirstOrDefault();
        }
            var requestType = _assembly.GetTypes()
            .FirstOrDefault(t => t.FullName == $"{requestTypeName}");

        if (requestType == null)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Invalid request type: {requestTypeName}");
            return;
        }

        var validatorInterfaceType = typeof(IValidator<>).MakeGenericType(requestType);

        var serviceProvider = context.RequestServices;

        // Use Activator.CreateInstance to create an instance of the validator
        var validator = serviceProvider.GetRequiredService(validatorInterfaceType!);
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true) ;
        var requestBody = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        var body = JsonConvert.DeserializeObject(requestBody, requestType);
        var validationContext = new ValidationContext<object>(body! , new PropertyChain(), new RulesetValidatorSelector(new[] {RulesetValidatorSelector.DefaultRuleSetName}));
        var validationResult = await ((IValidator)validator).ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(Result.Failure<string>(new Error("Validation",string.Join(", ", errors))));
            await context.Response.CompleteAsync();

        }
        else
        {
            await _next(context);
        }
    }
}