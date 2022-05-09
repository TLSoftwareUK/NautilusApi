using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class SupportFile
    {
        public string Filename { get; set; }   
        public string Checksum { get; set; }
        public SupportFileType Type { get; set; }
        public DateTime Created { get; set; }
    }

    public enum SupportFileType
    {
        Xref
    }
}
