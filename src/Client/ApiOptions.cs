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
        public string SiteServiceBaseUrl { get; private set; }
        
        public bool AuthEnabled { get; private set; }
        
        public ApiOptions(IServiceCollection collection)
        {
            _collection = collection;
            _collection.AddSingleton<ApiOptions>(this);
            _collection.AddSingleton<ISiteUpdateNotificationService, NullUpdateService>();
            _collection.AddSingleton<SiteCache>();
        }

        public IApiBuilderMode UseProduction()
        {
            BaseUrl = "https://www.tlsoftware.co.uk/api";
            SiteServiceBaseUrl = "https://www.tlsoftware.co.uk/api";
            SiteDesignerUrl = "https://www.tlsoftware.co.uk/sitedesigner";
            return this;
        }

        public IApiBuilderMode UseStaging()
        {
            BaseUrl = "https://staging.tlsoftware.co.uk/api";
            SiteServiceBaseUrl = "https://staging.tlsoftware.co.uk/api";
            SiteDesignerUrl = "https://staging.tlsoftware.co.uk/sitedesigner";
            return this;
        }

        public IApiBuilderMode UseConfiguration(IConfiguration configuration)
        {
            BaseUrl = configuration["Api:SiteBaseUrl"];
            SiteDesignerUrl = configuration["Api:SiteDesignerUrl"];
            SiteServiceBaseUrl = configuration["Api:SiteServiceBaseUrl"];
            return this;
        }

        public INautilusApi UseGrpc()
        {
            throw new NotImplementedException();
        }

        public INautilusApi UseHttp()
        {
            //TODO: Move to one instance??
            _collection.AddScoped<ISiteClient, HttpSiteClient>();
            _collection.AddScoped<IFileClient, HttpSiteClient>();
            _collection.AddScoped<IProfileClient, HttpSiteClient>();
            AuthEnabled = true;
            return this;
        }
        
        public INautilusApi UseHttpNoAuth()
        {
            _collection.AddScoped<ISiteClient, HttpSiteClient>();
            _collection.AddScoped<IFileClient, HttpSiteClient>();
            AuthEnabled = false;
            return this;
        }

        public INautilusApi ReceiveUpdates()
        {
            _collection.AddScoped<ISiteClient, HttpSiteClient>();
            _collection.AddSingleton<ISiteUpdateNotificationService, SiteUpdateNotificationService>();
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
        
        INautilusApi UseHttpNoAuth();
    }
    
    public interface INautilusApi
    {
        public INautilusApi ReceiveUpdates();
    }
}
