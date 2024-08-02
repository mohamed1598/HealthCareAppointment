using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers;

public class HandlerChecker(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public bool HasHandler(string requestTypeName)
    {
        var requestType = Type.GetType(requestTypeName) ?? throw new ArgumentException($"Type {requestTypeName} not found.");
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(bool));
        var handlers = _serviceProvider.GetServices(handlerType);
        return handlers.Any();
    }
}
