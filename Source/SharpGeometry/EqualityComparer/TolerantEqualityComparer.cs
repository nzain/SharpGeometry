using System;
using System.Collections.Generic;

namespace SharpGeometry.EqualityComparer
{
    /// <summary>
    /// Tolerant equality comparer based on a tolerance. Not useful for hashing. Tolerant equality
    /// is implemented for the following classes:
    /// <list type="bullet">
    /// <item><see cref="Vector3D"/></item>
    /// <item><see cref="Point3D"/></item>
    /// </list>
    /// </summary>
    public class TolerantEqualityComparer : IEqualityComparer<Vector3D>, IEqualityComparer<Point3D>
    {
        /// <summary>
        /// Creates a new <see cref="Vector3D"/> equality comparer.
        /// </summary>
        /// <param name="tolerance">The tolerance (per dimension) to accept.</param>
        public TolerantEqualityComparer(double tolerance)
        {
            if (tolerance <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance));
            }
            this.Tolerance = tolerance;
        }

        /// <summary>
        /// Gets the tolerance (per dimension).
        /// </summary>
        public double Tolerance { get; }

        /// <inheritdoc/>
        public bool Equals(Vector3D a, Vector3D b)
        {
            return this.Equals(a.X, b.X)
                && this.Equals(a.Y, b.Y)
                && this.Equals(a.Z, b.Z);
        }

        /// <inheritdoc/>
        public bool Equals(Point3D a, Point3D b)
        {
            return this.Equals(a.X, b.X)
                && this.Equals(a.Y, b.Y)
                && this.Equals(a.Z, b.Z);
        }

        private bool Equals(double a, double b)
        {
            return Math.Abs(a - b) <= this.Tolerance;
        }

        /// <inheritdoc/>
        public int GetHashCode(Vector3D v)
        {
            // Ok, this is really stupid. The interface forces us to implement something we cannot provide.
            // Return the exact same hashcode for everything => always collision => always fallback to Equals method.
            return 0;
            // Wrong: return v.GetHashCode()
        }

        /// <inheritdoc/>
        public int GetHashCode(Point3D p) => 0;
    }
}
