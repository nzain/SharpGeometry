using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace SharpGeometry
{
    /// <summary>The immutable 3D matrix is represented as:
    /// <code>
    /// [ M11 M12 M13 ]
    /// [ M21 M22 M23 ]
    /// [ M31 M32 M33 ]
    /// </code>
    /// </summary>
    [DataContract]
    public struct Matrix3D : IEquatable<Matrix3D>
    {
        /// <summary>The identity matrix.</summary>
        public static Matrix3D Identity = new Matrix3D(1); // immutable, so we don't need a method for the ctor

        /// <summary>An undefined Matrix3D.</summary>
        public static Matrix3D Undefined = new Matrix3D(
            double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN);

        #region Private Readonly Fields

        [DataMember(Order = 0, Name = "M11", IsRequired = true)]
        private readonly double _m11;

        [DataMember(Order = 1, Name = "M12", IsRequired = true)]
        private readonly double _m12;

        [DataMember(Order = 2, Name = "M13", IsRequired = true)]
        private readonly double _m13;

        [DataMember(Order = 3, Name = "M21", IsRequired = true)]
        private readonly double _m21;

        [DataMember(Order = 4, Name = "M22", IsRequired = true)]
        private readonly double _m22;

        [DataMember(Order = 5, Name = "M23", IsRequired = true)]
        private readonly double _m23;

        [DataMember(Order = 6, Name = "M31", IsRequired = true)]
        private readonly double _m31;

        [DataMember(Order = 7, Name = "M32", IsRequired = true)]
        private readonly double _m32;

        [DataMember(Order = 8, Name = "M33", IsRequired = true)]
        private readonly double _m33;

        #endregion

        #region Public CTORs, Factory Methods, and Properties

        /// <summary>Creates a diagonal (scaling) matrix.</summary>
        /// <param name="diagonalValue">The diagonal value, e.g. 1 for an identity matrix.</param>
        public Matrix3D(double diagonalValue)
        {
            this._m11 = this._m22 = this._m33 = diagonalValue;
            this._m12 = this._m13 = this._m21 = this._m23 = this._m31 = this._m32 = 0;
        }

        /// <summary>Creates a general matrix.</summary>
        /// <param name="m11">Value at (1,1).</param>
        /// <param name="m12">Value at (1,2).</param>
        /// <param name="m13">Value at (1,3).</param>
        /// <param name="m21">Value at (2,1).</param>
        /// <param name="m22">Value at (2,2).</param>
        /// <param name="m23">Value at (2,3).</param>
        /// <param name="m31">Value at (3,1).</param>
        /// <param name="m32">Value at (3,2).</param>
        /// <param name="m33">Value at (3,3).</param>
        public Matrix3D(
            double m11, double m12, double m13,
            double m21, double m22, double m23,
            double m31, double m32, double m33)
        {
            this._m11 = m11;
            this._m12 = m12;
            this._m13 = m13;
            this._m21 = m21;
            this._m22 = m22;
            this._m23 = m23;
            this._m31 = m31;
            this._m32 = m32;
            this._m33 = m33;
        }

        /// <summary>Creates a general matrix from a <c>3x3</c> jagged array.</summary>
        /// <param name="array">The two-dimensional quadratic array of length three.</param>
        public Matrix3D(double[][] array)
        {
            if (array.Length != 3 || array[0].Length != 3 || array[1].Length != 3 || array[2].Length != 3)
            {
                throw new ArgumentException("invalid array length");
            }
            this._m11 = array[0][0];
            this._m12 = array[0][1];
            this._m13 = array[0][2];
            this._m21 = array[1][0];
            this._m22 = array[1][1];
            this._m23 = array[1][2];
            this._m31 = array[2][0];
            this._m32 = array[2][1];
            this._m33 = array[2][2];
        }

        /// <summary>Creates a matrix from a row major array.</summary>
        /// <param name="rowMajor">Row major sorted array.</param>
        public Matrix3D(double[] rowMajor)
        {
            if (rowMajor.Length != 9)
            {
                throw new ArgumentException("invalid array length " + rowMajor.Length);
            }
            this._m11 = rowMajor[0];
            this._m12 = rowMajor[1];
            this._m13 = rowMajor[2];
            this._m21 = rowMajor[3];
            this._m22 = rowMajor[4];
            this._m23 = rowMajor[5];
            this._m31 = rowMajor[6];
            this._m32 = rowMajor[7];
            this._m33 = rowMajor[8];
        }

        /// <summary>Creates a scaling matrix.</summary>
        /// <param name="scaleX">Scale for <c>X</c> dimension.</param>
        /// <param name="scaleY">Scale for <c>Y</c> dimension.</param>
        /// <param name="scaleZ">Scale for <c>Z</c> dimension.</param>
        /// <returns>The scaling matrix.</returns>
        public static Matrix3D Scale(double scaleX, double scaleY, double scaleZ)
        {
            return new Matrix3D(
                scaleX, 0, 0,
                0, scaleY, 0,
                0, 0, scaleZ);
        }

        /// <summary>Creates a rotation matrix from axis and angle. Such a matrix leaves points along the rotation axis unchanged,
        /// while other points are rotated around the given axis using the right hand rule.</summary>
        /// <param name="axis">The rotation axis.</param>
        /// <param name="theta">The rotation angle about the given axis using the right hand rule.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix3D Rotate(Vector3D axis, double theta)
        {
            // https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
            axis = axis.Normalized();
            double ux = axis.X;
            double uy = axis.Y;
            double uz = axis.Z;
            double sin = Math.Sin(theta);
            double cos = Math.Cos(theta);
            double m11 = cos + ux*ux*(1 - cos);
            double m12 = ux*uy*(1 - cos) - uz*sin;
            double m13 = ux*uz*(1 - cos) + uy*sin;
            double m21 = uy*ux*(1 - cos) + uz*sin;
            double m22 = cos + uy*uy*(1 - cos);
            double m23 = uy*uz*(1 - cos) - ux*sin;
            double m31 = uz*ux*(1 - cos) - uy*sin;
            double m32 = uz*uy*(1 - cos) + ux*sin;
            double m33 = cos + uz*uz*(1 - cos);
            var rotation = new Matrix3D(
                m11, m12, m13,
                m21, m22, m23,
                m31, m32, m33);
            return rotation;
        }

        /// <summary>Creates a rotation matrix from two vectors. The rotation angle is the inner (unsigned) angle between
        /// <c>source</c> and <c>target</c> vector. The rotation direction is from <c>source</c> to <c>target</c>.</summary>
        /// <param name="source">The source vector.</param>
        /// <param name="target">The target vector.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix3D Rotate(Vector3D source, Vector3D target)
        {
            source = source.Normalized();
            target = target.Normalized();
            Vector3D axis = Vector3D.CrossProduct(source, target);
            double axisL2 = axis.SquaredLength();
            double theta = Vector3D.InnerAngle(source, target);
            if (axisL2 < 1e-20)
            {
                return Identity; // avoid division by zero, if source==target
            }
            return Rotate(axis, theta);
        }

        /// <summary>Value at row 1, column 1.</summary>
        public double M11 => this._m11;

        /// <summary>Value at row 1, column 2.</summary>
        public double M12 => this._m12;

        /// <summary>Value at row 1, column 3.</summary>
        public double M13 => this._m13;

        /// <summary>Value at row 2, column 1.</summary>
        public double M21 => this._m21;

        /// <summary>Value at row 2, column 2.</summary>
        public double M22 => this._m22;

        /// <summary>Value at row 2, column 3.</summary>
        public double M23 => this._m23;

        /// <summary>Value at row 3, column 1.</summary>
        public double M31 => this._m31;

        /// <summary>Value at row 3, column 2.</summary>
        public double M32 => this._m32;

        /// <summary>Value at row 3, column 3.</summary>
        public double M33 => this._m33;

        #endregion

        #region Public Methods

        /// <summary>Check if matrix is undefined.</summary>
        /// <returns><c>true</c>, if any entry is <c>NaN</c>.</returns>
        public bool IsUndefined()
        {
            return double.IsNaN(this.M11) || double.IsNaN(this.M12) || double.IsNaN(this.M13) ||
                   double.IsNaN(this.M21) || double.IsNaN(this.M22) || double.IsNaN(this.M23) ||
                   double.IsNaN(this.M31) || double.IsNaN(this.M32) || double.IsNaN(this.M33);
        }

        /// <summary>Computes the determinant of this matrix.</summary>
        /// <returns>The determinant.</returns>
        public double Determinant()
        {
            return this.M11*this.M22*this.M33
                   - this.M11*this.M23*this.M32
                   + this.M21*this.M32*this.M13
                   - this.M21*this.M12*this.M33
                   + this.M31*this.M12*this.M23
                   - this.M31*this.M22*this.M13;
        }

        /// <summary>Returns the transpose of this matrix.</summary>
        /// <returns>The transpose.</returns>
        public Matrix3D Transposed()
        {
            return new Matrix3D(
                this.M11, this.M21, this.M31,
                this.M12, this.M22, this.M32,
                this.M13, this.M23, this.M33);
        }

        /// <summary>Returns a row major array of this matrix.</summary>
        /// <returns>An array in row major order.</returns>
        public double[] ToRowMajorArray()
        {
            return new[]
            {
                this.M11, this.M12, this.M13,
                this.M21, this.M22, this.M23,
                this.M31, this.M32, this.M33
            };
        }

        /// <summary>Returns a column major array of this matrix.</summary>
        /// <returns>An array in column major order.</returns>
        public double[] ToColumnMajorArray()
        {
            return new[]
            {
                this.M11, this.M21, this.M31,
                this.M12, this.M22, this.M32,
                this.M13, this.M23, this.M33
            };
        }

        /// <summary>Returns a jagged <c>3x3</c> array of this matrix.</summary>
        /// <returns>A <c>3x3</c> array.</returns>
        public double[][] ToArray()
        {
            return new[]
            {
                new[] { this.M11, this.M12, this.M13 },
                new[] { this.M21, this.M22, this.M23 },
                new[] { this.M31, this.M32, this.M33 }
            };
        }

        /// <summary>Returns the i'th row, where <c>i=1..3</c>.</summary>
        /// <param name="index">The mathematical index (starting at one, not zero).</param>
        /// <returns>The i'th row.</returns>
        public Vector3D Row(int index)
        {
            switch (index)
            {
                case 1:
                    return new Vector3D(this.M11, this.M12, this.M13);
                case 2:
                    return new Vector3D(this.M21, this.M22, this.M23);
                case 3:
                    return new Vector3D(this.M31, this.M32, this.M33);
                default:
                    throw new IndexOutOfRangeException("index must be in [1,2,3], but got " + index);
            }
        }

        /// <summary>Returns the i'th column, where <c>i=1..3</c>.</summary>
        /// <param name="index">The mathematical index (starting at one, not zero).</param>
        /// <returns>The i'th column.</returns>
        public Vector3D Column(int index)
        {
            switch (index)
            {
                case 1:
                    return new Vector3D(this.M11, this.M21, this.M31);
                case 2:
                    return new Vector3D(this.M12, this.M22, this.M32);
                case 3:
                    return new Vector3D(this.M13, this.M23, this.M33);
                default:
                    throw new IndexOutOfRangeException("index must be in [1,2,3], but got " + index);
            }
        }

        /// <summary>Compares this matrix to an arbitrary object.</summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns><c>true</c>, if the given object is a matrix and the matrices represent exactly the same values.</returns>
        public override bool Equals(object obj)
        {
            return obj is Matrix3D && this.Equals((Matrix3D)obj);
        }

        /// <summary>Compares this matrix to the given matrix (exact value comparison).</summary>
        /// <param name="other">The other matrix.</param>
        /// <returns><c>true</c> if the matrices have the exact same values.</returns>
        public bool Equals(Matrix3D other)
        {
            return this == other;
        }

        /// <summary>Returns a simple hashcode.</summary>
        /// <returns>A hashcode.</returns>
        public override int GetHashCode()
        {
            int hash = 17*23 + this.M11.GetHashCode();
            hash = hash*23 + this.M12.GetHashCode();
            hash = hash*23 + this.M13.GetHashCode();
            hash = hash*23 + this.M21.GetHashCode();
            hash = hash*23 + this.M22.GetHashCode();
            hash = hash*23 + this.M23.GetHashCode();
            hash = hash*23 + this.M31.GetHashCode();
            hash = hash*23 + this.M32.GetHashCode();
            hash = hash*23 + this.M33.GetHashCode();
            return hash;
        }

        /// <summary>Returns a string representation (fixed point, three decimal places) of this matrix.</summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "[{0,6:F3} {1,6:F3} {2,6:F3}]\n" +
                "[{3,6:F3} {4,6:F3} {5,6:F3}]\n" +
                "[{6,6:F3} {7,6:F3} {8,6:F3}]",
                this.M11, this.M12, this.M13,
                this.M21, this.M22, this.M23,
                this.M31, this.M32, this.M33);
        }

        #endregion

        #region Unary Operators

        /// <summary>Negation <c>-M</c>.</summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The negative matrix.</returns>
        public static Matrix3D operator -(Matrix3D matrix)
        {
            return new Matrix3D(
                -matrix.M11, -matrix.M12, -matrix.M13,
                -matrix.M21, -matrix.M22, -matrix.M23,
                -matrix.M31, -matrix.M32, -matrix.M33);
        }

        /// <summary>Reinforcement <c>+M</c> does nothing.</summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The same matrix.</returns>
        public static Matrix3D operator +(Matrix3D matrix)
        {
            return matrix;
        }

        #endregion

        #region Binary Operators

        /// <summary>Compares two matrices for equality.</summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns><c>true</c> on equality.</returns>
        public static bool operator ==(Matrix3D a, Matrix3D b)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13
                   && a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23
                   && a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>Compares two matrices for inequality.</summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns><c>true</c> on inequality.</returns>
        public static bool operator !=(Matrix3D a, Matrix3D b)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return a.M11 != b.M11 || a.M12 != b.M12 || a.M13 != b.M13
                   || a.M21 != b.M21 || a.M22 != b.M22 || a.M23 != b.M23
                   || a.M31 != b.M31 || a.M32 != b.M32 || a.M33 != b.M33;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>Right scalar multiplication <c>M*s</c>.</summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="scalar">The scalar.</param>
        /// <returns>The multiplied matrix.</returns>
        public static Matrix3D operator *(Matrix3D matrix, double scalar)
        {
            return new Matrix3D(
                matrix.M11*scalar, matrix.M12*scalar, matrix.M13*scalar,
                matrix.M21*scalar, matrix.M22*scalar, matrix.M23*scalar,
                matrix.M31*scalar, matrix.M32*scalar, matrix.M33*scalar);
        }

        /// <summary>Left scalar multiplication <c>s*M</c>.</summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The multiplied matrix.</returns>
        public static Matrix3D operator *(double scalar, Matrix3D matrix)
        {
            // real values are commutative, so this is equivalent to right scalar multiplication.
            return new Matrix3D(
                scalar*matrix.M11, scalar*matrix.M12, scalar*matrix.M13,
                scalar*matrix.M21, scalar*matrix.M22, scalar*matrix.M23,
                scalar*matrix.M31, scalar*matrix.M32, scalar*matrix.M33);
        }

        /// <summary>Multiplies the given matrix and vector <c>M*v</c>.</summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="vector">The vector to transform.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector3D operator *(Matrix3D matrix, Vector3D vector)
        {
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;
            var result = new Vector3D(
                x*matrix.M11 + y*matrix.M12 + z*matrix.M13,
                x*matrix.M21 + y*matrix.M22 + z*matrix.M23,
                x*matrix.M31 + y*matrix.M32 + z*matrix.M33);
            return result;
        }

        /// <summary>Multiplies given matrix and point <c>M*p</c>.</summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="point">The point to transform.</param>
        /// <returns>The transformed point.</returns>
        public static Point3D operator *(Matrix3D matrix, Point3D point)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            var result = new Point3D(
                x*matrix.M11 + y*matrix.M12 + z*matrix.M13,
                x*matrix.M21 + y*matrix.M22 + z*matrix.M23,
                x*matrix.M31 + y*matrix.M32 + z*matrix.M33);
            return result;
        }

        /// <summary>Matrix addition <c>A+B</c>.</summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>The sum of the matrices.</returns>
        public static Matrix3D operator +(Matrix3D a, Matrix3D b)
        {
            return new Matrix3D(
                a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
                a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
                a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33);
        }

        /// <summary>Matrix subtraction <c>A-B</c>.</summary>
        /// <param name="a">The first matrix.</param>
        /// <param name="b">The second matrix.</param>
        /// <returns>The result of the subtraction.</returns>
        public static Matrix3D operator -(Matrix3D a, Matrix3D b)
        {
            return new Matrix3D(
                a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
                a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
                a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33);
        }

        /// <summary>Multiplies two matrices <c>A*B</c>.</summary>
        /// <param name="a">The left matrix to multiply.</param>
        /// <param name="b">The right matrix by which the first matrix is multiplied.</param>
        /// <returns>The resulting matrix.</returns>
        public static Matrix3D operator *(Matrix3D a, Matrix3D b)
        {
            // someone said the compiler can't properly reuse fields, so a local copy 
            // makes sense, since we access each one multiple times. Tedious, isn't it?
            double a11 = a.M11;
            double a12 = a.M12;
            double a13 = a.M13;
            double a21 = a.M21;
            double a22 = a.M22;
            double a23 = a.M23;
            double a31 = a.M31;
            double a32 = a.M32;
            double a33 = a.M33;

            double b11 = b.M11;
            double b12 = b.M12;
            double b13 = b.M13;
            double b21 = b.M21;
            double b22 = b.M22;
            double b23 = b.M23;
            double b31 = b.M31;
            double b32 = b.M32;
            double b33 = b.M33;

            // tried some optimized algorithms here (e.g. ladderman), but the simple one is the fastest.
            double c11 = a11*b11 + a12*b21 + a13*b31;
            double c12 = a11*b12 + a12*b22 + a13*b32;
            double c13 = a11*b13 + a12*b23 + a13*b33;
            double c21 = a21*b11 + a22*b21 + a23*b31;
            double c22 = a21*b12 + a22*b22 + a23*b32;
            double c23 = a21*b13 + a22*b23 + a23*b33;
            double c31 = a31*b11 + a32*b21 + a33*b31;
            double c32 = a31*b12 + a32*b22 + a33*b32;
            double c33 = a31*b13 + a32*b23 + a33*b33;
            return new Matrix3D(
                c11, c12, c13,
                c21, c22, c23,
                c31, c32, c33);
        }

        #endregion
    }
}