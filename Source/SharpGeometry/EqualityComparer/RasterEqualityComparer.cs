using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGeometry.EqualityComparer
{
    /// <summary>
    /// Equality comparer based on a rasterization of the input space. Good for fast hashing and data reduction,
    /// but collisions are expected for large values (or very small raster size)
    /// </summary>
    public class RasterEqualityComparer : IEqualityComparer<Vector3D>, IEqualityComparer<Point3D>
    {
        private readonly double _centroidX, _centroidY, _centroidZ;

        /// <summary>
        /// Creates a new equality comparer based on a rasterization of the input space.
        /// </summary>
        /// <param name="rasterSize">The raster size. All points within one raster cube are considered equal and receive the same hash code.</param>
        public RasterEqualityComparer(double rasterSize)
        {
            if (rasterSize <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.RasterSize = rasterSize;
        }

        /// <summary>
        /// Creates a new equality comparer based on a rasterization of the input space around a given centroid.
        /// </summary>
        /// <param name="rasterSize">The raster size. All points within one raster cube are considered equal and receive the same hash code.</param>
        /// <param name="centroid">The expected centroid of the incoming data.</param>
        public RasterEqualityComparer(double rasterSize, Point3D centroid)
            : this(rasterSize)
        {
            this._centroidX = centroid.X;
            this._centroidY = centroid.Y;
            this._centroidZ = centroid.Z;
        }

        /// <summary>
        /// Gets the raster size.
        /// </summary>
        public double RasterSize { get; }

        /// <summary>
        /// Gets the centroid of the expected data to reduce collisions.
        /// </summary>
        public Point3D Centroid => new Point3D(this._centroidX, this._centroidY, this._centroidZ);

        /// <inheritdoc/>
        public bool Equals(Vector3D a, Vector3D b)
        {
            return this.RasterIndexX(a.X) == this.RasterIndexX(b.X)
                && this.RasterIndexY(a.Y) == this.RasterIndexY(b.Y)
                && this.RasterIndexZ(a.Z) == this.RasterIndexZ(b.Z);
        }

        /// <inheritdoc/>
        public bool Equals(Point3D a, Point3D b)
        {
            return this.RasterIndexX(a.X) == this.RasterIndexX(b.X)
                && this.RasterIndexY(a.Y) == this.RasterIndexY(b.Y)
                && this.RasterIndexZ(a.Z) == this.RasterIndexZ(b.Z);
        }

        /// <inheritdoc/>
        public int GetHashCode(Vector3D a)
        {
            return GetHashCode(a.X, a.Y, a.Z);
        }

        /// <inheritdoc/>
        public int GetHashCode(Point3D a)
        {
            return GetHashCode(a.X, a.Y, a.Z);
        }

        private int GetHashCode(double x, double y, double z)
        {
            int rasterX = this.RasterIndexX(x);
            int rasterY = this.RasterIndexY(y);
            int rasterZ = this.RasterIndexZ(z);
            return rasterX + (rasterY << 10) + (rasterZ << 20);
        }

        private int RasterIndexX(double value)
        {
            return (int)((value - this._centroidX) / this.RasterSize);
        }

        private int RasterIndexY(double value)
        {
            return (int)((value - this._centroidY) / this.RasterSize);
        }

        private int RasterIndexZ(double value)
        {
            return (int)((value - this._centroidZ) / this.RasterSize);
        }
    }
}
