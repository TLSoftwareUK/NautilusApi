using System.Numerics;

namespace TLS.Nautilus.Api.Shared
{
    public interface IGroundModel
    {
        double GetElevation(DoubleVector2 location);
    }
}
