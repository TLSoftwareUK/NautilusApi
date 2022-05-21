using System;

namespace TLS.Nautilus.Api.Shared
{
    public struct DoubleVector2
    {
        double X { get; set; }
        double Y { get; set; }

        public DoubleVector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static DoubleVector2 Zero { get { return new DoubleVector2 { X = 0, Y = 0 }; } }

        public static DoubleVector2 One { get { return new DoubleVector2 { X = 1, Y = 1 }; } }

        public static DoubleVector2 operator -(DoubleVector2 left, DoubleVector2 right)
        {
            return new DoubleVector2 { X = left.X - right.X, Y = left.Y - right.Y };
        }

        public static DoubleVector2 operator +(DoubleVector2 left, DoubleVector2 right)
        {
            return new DoubleVector2 { X = left.X + right.X, Y = left.Y + right.Y };
        }

        public static DoubleVector2 operator /(DoubleVector2 left, DoubleVector2 right)
        {
            throw new NotImplementedException();
        }

        public static DoubleVector2 operator /(DoubleVector2 value1, double value2)
        {
            return new DoubleVector2 { X = value1.X / value2, Y = value1.Y / value2 };
        }

        public static DoubleVector2 operator *(DoubleVector2 left, float right)
        {
            return new DoubleVector2 { X = left.X * right, Y = left.Y * right };
        }
    }
}
