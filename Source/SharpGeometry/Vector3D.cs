using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGeometry
{
    public struct Vector3D
    {
        public Vector3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; }
        public double Y { get; }
        public double Z { get; }
    }
}
