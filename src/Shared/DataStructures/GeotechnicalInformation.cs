using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public partial class GeotechnicalInformation : ObservableObject
    {
        [ObservableProperty]
        private int _modifiedPlasticityIndex;

        [ObservableProperty]
        private double _safeGroundBearingPressure;
    }
}