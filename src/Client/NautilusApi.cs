using System;
using Microsoft.Extensions.DependencyInjection;

namespace TLS.Nautilus.Api
{
    public static class NautilusApi
    {
        internal static string BearerToken;
        
        public static void AddNautilusApi(this IServiceCollection collection, Func<INautilusApiBuilder, INautilusApi> configureOptions)
        {
            ApiOptions ops = new ApiOptions(collection);
            configureOptions.Invoke(ops);
        }
        
        public static void SetToken(string token)
        {
            BearerToken = token;
        }
        
        public static bool AuthSet()
        {
            return !string.IsNullOrWhiteSpace(BearerToken);
        }
    }
}
