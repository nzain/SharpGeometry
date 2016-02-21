using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace SharpGeometry
{
    /// <summary>
    /// This immutable struct represents a vector in 3D space. Vectors always start at the origin <c>[0,0,0]</c> and only represent a direction, not a true coordinate.
    /// </summary>
    [DataContract]
    public struct Vector3D
    {
        #region Public Constants

        /// <summary>The zero vector.</summary>
        public static readonly Vector3D Zero = default(Vector3D);

        /// <summary>The <c>X</c> axis unit vector.</summary>
        public static readonly Vector3D XAxis = new Vector3D(1, 0, 0);

        /// <summary>The <c>Y</c> axis unit vector.</summary>
        public static readonly Vector3D YAxis = new Vector3D(1, 0, 0);

        /// <summary>The <c>Z</c> axis unit vector.</summary>
        public static readonly Vector3D ZAxis = new Vector3D(1, 0, 0);

        #endregion

        #region Public CTOR and Properties

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
        [DataMember(Order = 0, IsRequired = true)]
        public double X { get; }

        /// <summary>
        /// Gets the <c>Y</c> coordinate of this vector.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public double Y { get; }

        /// <summary>
        /// Gets the <c>Z</c> coordinate of this vector.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public double Z { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Computes the squared length of this vector (cheap computation, no square root needed).
        /// </summary>
        /// <returns>The squared length of this vector.</returns>
        public double SquaredLength()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        /// <summary>
        /// Computes the euclidean length of this vector, that is, <c>Math.Sqrt(X²+Y²+Z²)</c>.
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(this.SquaredLength());
        }

        /// <summary>
        /// Returns a scaled copy of the desired length.
        /// </summary>
        /// <param name="length">The desired length.</param>
        public Vector3D ScaledTo(double length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            double l2 = this.SquaredLength();
            if (l2 <= 0)
            {
                throw new InvalidOperationException("Cannot scale a vector of zero length.");
            }
            double actualLength = Math.Sqrt(l2);
            Debug.Assert(actualLength > 0);
            return this * (length / actualLength);
        }

        /// <summary>
        /// Returns a normalized (unit length) copy of this vector.
        /// </summary>
        /// <returns>A vector of unit length pointing in the same direction.</returns>
        public Vector3D Normalized()
        {
            double l2 = this.SquaredLength();
            if (l2 <= 0)
            {
                throw new InvalidOperationException("Cannot scale a vector of zero length.");
            }
            double actualLength = Math.Sqrt(l2);
            Debug.Assert(actualLength > 0);
            return this / actualLength;
        }

        /// <summary>
        /// Checks if this vector has some invalid value (infinity or nan).
        /// </summary>
        /// <returns><c>true</c> if at least one value is invalid.</returns>
        public bool IsUndefined()
        {
            return double.IsInfinity(this.X) || double.IsNaN(this.X)
                || double.IsInfinity(this.Y) || double.IsNaN(this.Y)
                || double.IsInfinity(this.Z) || double.IsNaN(this.Z);
        }

        /// <summary>
        /// Checks if all components are zero.
        /// </summary>
        /// <returns><c>true</c> if all components are zero.</returns>
        public bool IsZero()
        {
            return this.SquaredLength() <= 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "[{0:F3} {1:F3} {2:F3}]", this.X, this.Y, this.Z);
        }

        #endregion

        #region Unary Operators

        /// <summary>
        /// Unary minus returns the complement.
        /// </summary>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator -(Vector3D r)
        {
            return new Vector3D(-r.X, -r.Y, -r.Z);
        }

        /// <summary>
        /// Unary plus does nothing.
        /// </summary>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator +(Vector3D r)
        {
            return r;
        }

        #endregion

        #region Binary Operators

        /// <summary>
        /// Binary plus adds the given vectors.
        /// </summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator +(Vector3D l, Vector3D r)
        {
            return new Vector3D(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        /// <summary>
        /// Binary minus subtracts the given vectors.
        /// </summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator -(Vector3D l, Vector3D r)
        {
            return new Vector3D(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        /// <summary>
        /// Left multiplcation with a scalar value.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="v">The vector.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator *(double scalar, Vector3D v)
        {
            return new Vector3D(scalar * v.X, scalar * v.Y, scalar * v.Z);
        }

        /// <summary>
        /// Right multiplcation with a scalar value.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator *(Vector3D v, double scalar)
        {
            return new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        /// <summary>
        /// Right division by a scalar value.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The result.</returns>
        public static Vector3D operator /(Vector3D v, double scalar)
        {
            if (scalar == 0)
            {
                throw new DivideByZeroException();
            }
            return new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        #endregion
    }
}
