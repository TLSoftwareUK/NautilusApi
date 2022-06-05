using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLS.Nautilus.Api.Shared;

namespace TLS.Nautilus.Api
{
    public interface IProfileClient
    {
        Task<IProfile?> GetProfileAsync();

        Task UpdateProfileAsync(IProfile profile);
        Task CreateProfileAsync(IProfile profile);
    }
}
