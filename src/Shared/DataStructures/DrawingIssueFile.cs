using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class DrawingIssueFile
    {
        public string DrawingTitle { get; set; }
        public string DrawingNumber { get; set; }
        public string Filename { get; set; }   
        public string Checksum { get; set; }        
        public DateTime Created { get; set; }
    }
}
