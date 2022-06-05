using IronstoneSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared
{
    public interface IProfile
    {
        string Id { get; }              

        string Org { get; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public MainSettings IronstoneSettings { get; set; }
    }
}
