using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGeometry.EqualityComparer
{
    /// <summary>
    /// Equality comparer for <see cref="Point3D"/> based on a tolerance.
    /// </summary>
    public class Point3DComparer : IEqualityComparer<Point3D>
    {
        /// <summary>
        /// Creates a new <see cref="Point3D"/> equality comparer.
        /// </summary>
        /// <param name="tolerance">The tolerance (per dimension) to accept.</param>
        public Point3DComparer(double tolerance)
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
        public bool Equals(Point3D a, Point3D b)
        {
            return Math.Abs(a.X - b.X) <= this.Tolerance
                && Math.Abs(a.Y - b.Y) <= this.Tolerance
                && Math.Abs(a.Z - b.Z) <= this.Tolerance;
        }

        /// <inheritdoc/>
        public int GetHashCode(Point3D p)
        {
            // The interface forces us to implement something we cannot provide.
            // Return the exact same hashcode for everything => always collision => always fallback to Equals method.
            return 0;

            // Wrong: return v.GetHashCode()
        }
    }
}
