using System;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class SiteActivity
    {
        public Activity Action { get; set; }
        
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        
        public string ActionBy { get; set; }
        
        public DateTime Time { get; set; }
    }

    public enum Activity
    {
        SiteCreated,
        //Geo
        ModifiedPlasticityIndexChanged,
        GroundBearingPressureChanged,
    }
}
