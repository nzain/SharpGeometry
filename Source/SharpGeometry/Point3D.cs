using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace SharpGeometry
{
    /// <summary>This immutable struct represents a point in 3D space. A point does not have a length or direction but instead
    /// support the notion of distance to other points.</summary>
    [DataContract]
    public struct Point3D
    {
        #region Private Readonly Fields (required for serialization)

        [DataMember(Order = 0, Name = "X", IsRequired = true)]
        private readonly double _x;

        [DataMember(Order = 1, Name = "Y", IsRequired = true)]
        private readonly double _y;

        [DataMember(Order = 2, Name = "Z", IsRequired = true)]
        private readonly double _z;

        #endregion

        #region Public CTOR and Properties

        /// <summary>Creates a new vector.</summary>
        /// <param name="x">The <see cref="X" /> coordinate.</param>
        /// <param name="y">The <see cref="Y" /> coordinate.</param>
        /// <param name="z">The <see cref="Z" /> coordinate.</param>
        public Point3D(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        /// <summary>Creates a new vector from an array with three elements <c>[x, y, z]</c>.</summary>
        /// <param name="values">An array of size three.</param>
        public Point3D(double[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (values.Length != 3)
            {
                throw new ArgumentException($"Expected an array of length 3, but actual length is {values.Length}",
                    nameof(values));
            }
            this._x = values[0];
            this._y = values[1];
            this._z = values[2];
        }

        /// <summary>Gets the <c>X</c> coordinate (immutable) of this point.</summary>
        public double X => this._x;

        /// <summary>Gets the <c>Y</c> coordinate (immutable) of this point.</summary>
        public double Y => this._y;

        /// <summary>Gets the <c>Z</c> coordinate (immutable) of this point.</summary>
        public double Z => this._z;

        #endregion

        #region Public Methods

        /// <summary>Computes the squared distance to the given point (cheap operation, no square root required).</summary>
        /// <param name="other">The other point.</param>
        /// <returns>The squared distance.</returns>
        public double SquaredDistance(Point3D other)
        {
            return SquaredDistance(this, other);
        }

        /// <summary>Computes the euclidean distance to the given point.</summary>
        /// <param name="other">The other point.</param>
        /// <returns>The euclidean distance.</returns>
        public double Distance(Point3D other)
        {
            return Math.Sqrt(SquaredDistance(this, other));
        }

        /// <summary>Checks if this vector has some invalid value (infinity or nan).</summary>
        /// <returns><c>true</c> if at least one value is invalid.</returns>
        public bool IsUndefined()
        {
            return double.IsInfinity(this.X) || double.IsNaN(this.X)
                   || double.IsInfinity(this.Y) || double.IsNaN(this.Y)
                   || double.IsInfinity(this.Z) || double.IsNaN(this.Z);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "[{0:F3} {1:F3} {2:F3}]", this.X, this.Y, this.Z);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is Point3D && this.Equals((Point3D)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // less than 1% collisions over 100000 random points
            unchecked // overflow is expected, explicitely unchecked
            {
                int hash = 17;
                hash = hash*23 + this.X.GetHashCode();
                hash = hash*23 + this.Y.GetHashCode();
                hash = hash*23 + this.Z.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc />
        public bool Equals(Point3D other)
        {
            return this == other; // delegate to operator
        }

        #endregion

        #region Static Methods

        /// <summary>Computes the squared distance between two points (cheap computation, no square root needed).</summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The squared distance.</returns>
        public static double SquaredDistance(Point3D a, Point3D b)
        {
            double delta = a.X - b.X;
            double sumOfSquares = delta*delta;
            delta = a.Y - b.Y;
            sumOfSquares += delta*delta;
            delta = a.Z - b.Z;
            sumOfSquares += delta*delta;
            return sumOfSquares;
        }

        /// <summary>Computes the euclidean distance between two points.</summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The euclidean distance.</returns>
        public static double Distance(Point3D a, Point3D b)
        {
            return Math.Sqrt(SquaredDistance(a, b));
        }

        #endregion

        #region Unary Operators and Casts

        /// <summary>Unary minus is a mirror operation.</summary>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator -(Point3D r)
        {
            return new Point3D(-r.X, -r.Y, -r.Z);
        }

        /// <summary>Unary positive reinforcement does nothing.</summary>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator +(Point3D r)
        {
            return r;
        }

        /// <summary>Cast a point into a vector.</summary>
        /// <param name="r">The point to cast.</param>
        /// <returns>The equivalent vector.</returns>
        public static explicit operator Vector3D(Point3D r)
        {
            return new Vector3D(r.X, r.Y, r.Z);
        }

        #endregion

        #region Binary Operators

        /// <summary>Binary exact equality operator.</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns><c>true</c> if all components are exactly the same.</returns>
        public static bool operator ==(Point3D l, Point3D r)
        {
            // do not use a tolerance here, although some people prefer this
            // ReSharper disable CompareOfFloatsByEqualityOperator - intended exact equality
            return l.X == r.X && l.Y == r.Y && l.Z == r.Z;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>Binary inequality operator.</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns><c>true</c> if any component is not exactly the same.</returns>
        public static bool operator !=(Point3D l, Point3D r)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator - intended exact inequality
            return l.X != r.X || l.Y != r.Y || l.Z != r.Z;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>Binary plus offsets the given point by a vector.</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator +(Point3D l, Vector3D r)
        {
            return new Point3D(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        /// <summary>Binary plus adds two points (which is mathematically questionable, but sometimes convenient).</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator +(Point3D l, Point3D r)
        {
            return new Point3D(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        /// <summary>Binary minus offsets the given point by the complement vector.</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator -(Point3D l, Vector3D r)
        {
            return new Point3D(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        /// <summary>Binary minus subtracts a point from another one (which is mathematically questionable, but sometimes
        /// convenient).</summary>
        /// <param name="l">Left operand.</param>
        /// <param name="r">Right operand.</param>
        /// <returns>The result.</returns>
        public static Point3D operator -(Point3D l, Point3D r)
        {
            return new Point3D(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        #endregion
    }
}