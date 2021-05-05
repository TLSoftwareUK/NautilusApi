using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TLS.Nautilus.Api
{
    public static class NautilusApi
    {
        public static void AddNautilusApi(this IServiceCollection collection, Func<INautilusApiBuilder, INautilusApi> configureOptions)
        {
            ApiOptions ops = new ApiOptions(collection);
            configureOptions.Invoke(ops);
        }
    }
}
