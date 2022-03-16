using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api
{
    public interface IFileClient
    {
        Task<Guid> UploadFile(Stream content, string filename);
    }
}
