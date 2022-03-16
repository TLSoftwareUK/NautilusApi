using System;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api
{
    public interface ISiteUpdateNotificationService
    {
        public Task Start();

        public Task OpenSite(Guid id);
    }
}
