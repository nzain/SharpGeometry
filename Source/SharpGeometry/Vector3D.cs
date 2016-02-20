using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharpGeometry
{
    /// <summary>
    /// This immutable struct represents a vector in 3D space. Vectors always start at the origin <c>[0,0,0]</c> and only represent a direction, not a true coordinate.
    /// </summary>
    public struct Vector3D
    {
        /// <summary>The zero vector.</summary>
        public static readonly Vector3D Zero = default(Vector3D);

        /// <summary>The <c>X</c> axis unit vector.</summary>
        public static readonly Vector3D XAxis = new Vector3D(1, 0, 0);

        /// <summary>The <c>Y</c> axis unit vector.</summary>
        public static readonly Vector3D YAxis = new Vector3D(1, 0, 0);

        /// <summary>The <c>Z</c> axis unit vector.</summary>
        public static readonly Vector3D ZAxis = new Vector3D(1, 0, 0);
        
        /// <summary>
        /// Creates a new vector.
        /// </summary>
        /// <param name="x">The <see cref="X"/> coordinate.</param>
        /// <param name="y">The <see cref="Y"/> coordinate.</param>
        /// <param name="z">The <see cref="Z"/> coordinate.</param>
        public Vector3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new vector from an array with three elements <c>[x, y, z]</c>.
        /// </summary>
        /// <param name="values">An array of size three.</param>
        public Vector3D(double[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Length != 3)
            {
                throw new ArgumentException($"Expected an array of length 3, but actual length is {values.Length}", nameof(values));
            }
            this.X = values[0];
            this.Y = values[1];
            this.Z = values[2];
        }

        /// <summary>
        /// Gets the <c>X</c> coordinate of this vector.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the <c>Y</c> coordinate of this vector.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Gets the <c>Z</c> coordinate of this vector.
        /// </summary>
        public double Z { get; }

        /// <summary>Gets a flag indicating this vector has some invalid value (infinity, nan).</summary>
        public bool IsUndefined => double.IsInfinity(this.X) || double.IsNaN(this.X)
            || double.IsInfinity(this.Y) || double.IsNaN(this.Y)
            || double.IsInfinity(this.Z) || double.IsNaN(this.Z);
            
        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "[{0,6:F3} {1,6:F3} {2,6:F3}]", this.X, this.Y, this.Z);
        }
    }
}
