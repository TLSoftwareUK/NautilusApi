using System;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api
{
    class NullUpdateService : ISiteUpdateNotificationService
    {
        public async Task Start()
        {
            return;
        }

        public async Task OpenSite(Guid id)
        {
        }
    }
}
