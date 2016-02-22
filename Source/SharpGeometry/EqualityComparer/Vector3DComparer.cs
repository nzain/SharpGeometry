using System;
using System.Collections.Generic;

namespace SharpGeometry.EqualityComparer
{
    /// <summary>
    /// Equality comparer for <see cref="Vector3D"/> based on a tolerance.
    /// </summary>
    public class Vector3DComparer : IEqualityComparer<Vector3D>
    {
        /// <summary>
        /// Creates a new <see cref="Vector3D"/> equality comparer.
        /// </summary>
        /// <param name="tolerance">The tolerance (per dimension) to accept.</param>
        public Vector3DComparer(double tolerance)
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
            return Math.Abs(a.X - b.X) <= this.Tolerance
                && Math.Abs(a.Y - b.Y) <= this.Tolerance
                && Math.Abs(a.Z - b.Z) <= this.Tolerance;
        }

        /// <inheritdoc/>
        public int GetHashCode(Vector3D v)
        {
            return v.GetHashCode();
        }
    }
}
