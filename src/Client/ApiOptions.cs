using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TLS.Nautilus.Api
{
    class ApiOptions : INautilusApiBuilder, IApiBuilderMode, INautilusApi
    {
        private IServiceCollection _collection;

        public string BaseUrl { get; private set; }
        public string SiteDesignerUrl { get; private set; }
        
        public ApiOptions(IServiceCollection collection)
        {
            _collection = collection;
            _collection.AddSingleton<ApiOptions>(this);
        }

        public IApiBuilderMode UseProduction()
        {
            BaseUrl = "https://www.tlsoftware.co.uk/api";
            SiteDesignerUrl = "https://www.tlsoftware.co.uk/sitedesigner";
            return this;
        }

        public IApiBuilderMode UseStaging()
        {
            BaseUrl = "https://staging.tlsoftware.co.uk/api";
            SiteDesignerUrl = "https://staging.tlsoftware.co.uk/sitedesigner";
            return this;
        }

        public IApiBuilderMode UseConfiguration(IConfiguration configuration)
        {
            BaseUrl = configuration["Api:SiteBaseUrl"];
            SiteDesignerUrl = configuration["Api:SiteDesignerUrl"];
            return this;
        }

        public INautilusApi UseGrpc()
        {
            throw new NotImplementedException();
        }

        public INautilusApi UseHttp()
        {
            _collection.AddScoped<ISiteClient, HttpSiteClient>();
            return this;
        }
    }

    public interface INautilusApiBuilder
    {
        IApiBuilderMode UseProduction();
        
        IApiBuilderMode UseStaging();
        
        IApiBuilderMode UseConfiguration(IConfiguration configuration);
        
    }
    
    public interface IApiBuilderMode
    {
        INautilusApi UseGrpc();
        
        INautilusApi UseHttp();
    }
    
    public interface INautilusApi
    {
        
    }
}
