using System;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Matrix3DTests
    {
        [Test]
        public void Equality()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var b = new Matrix3D(
                1.0, 2.0, 3.0,
                4.0, 5.0, 6.0,
                7.0, 8.0, 9.0);
            Matrix3D c = Matrix3D.Identity;

            Assert.That(a, Is.EqualTo(b));
            Assert.That(a.Equals(b), Is.True);
            Assert.That((a == b), Is.True);
            Assert.That((a != b), Is.False);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));

            Assert.That(a, Is.Not.EqualTo(c));
            Assert.That(a.Equals(c), Is.False);
            Assert.That((a == c), Is.False);
            Assert.That((a != c), Is.True);
            Assert.That(a.GetHashCode(), Is.Not.EqualTo(c.GetHashCode()));

            for (int i = 0; i < 9; i++)
            {
                var rowmajor = new double[9];
                rowmajor[i] = 42;
                var m1 = new Matrix3D(rowmajor);
                rowmajor[i] = 256;
                var m2 = new Matrix3D(rowmajor);
                // m1 and m2 are the same except for the i'th element.
                Assert.That(m1, Is.Not.EqualTo(m2));
                Assert.That((m1 == m2), Is.False);
                Assert.That((m1 != m2), Is.True);
                int h1 = m1.GetHashCode();
                int h2 = m2.GetHashCode();
                //Console.WriteLine("{0} ? {1}", h1, h2); // and you realize, hashcodes are !§$@% f***in stupid
                Assert.That(h1, Is.Not.EqualTo(h2));
            }
        }

        [Test]
        public void Constructors()
        {
            var a = new Matrix3D(3);
            //Console.WriteLine(a);
            Assert.That(a.M11, Is.EqualTo(3));
            Assert.That(a.M22, Is.EqualTo(3));
            Assert.That(a.M33, Is.EqualTo(3));
            Assert.That(a.M12, Is.EqualTo(0));
            Assert.That(a.M13, Is.EqualTo(0));
            Assert.That(a.M21, Is.EqualTo(0));
            Assert.That(a.M23, Is.EqualTo(0));
            Assert.That(a.M31, Is.EqualTo(0));
            Assert.That(a.M32, Is.EqualTo(0));

            var b = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            //Console.WriteLine(b);
            Assert.That(b.M11, Is.EqualTo(1));
            Assert.That(b.M12, Is.EqualTo(2));
            Assert.That(b.M13, Is.EqualTo(3));
            Assert.That(b.M21, Is.EqualTo(4));
            Assert.That(b.M22, Is.EqualTo(5));
            Assert.That(b.M23, Is.EqualTo(6));
            Assert.That(b.M31, Is.EqualTo(7));
            Assert.That(b.M32, Is.EqualTo(8));
            Assert.That(b.M33, Is.EqualTo(9));

            var c = new Matrix3D(new double[]
            {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9
            });
            Assert.That(c, Is.EqualTo(b));

            var e = new Matrix3D(new[]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 },
                new double[] { 7, 8, 9 }
            });
            Assert.That(e, Is.EqualTo(b));
        }

        [Test]
        public void Determinant()
        {
            Assert.That(Matrix3D.Identity.Determinant(), Is.EqualTo(1));

            var sut = new Matrix3D(
                -2, +2, -3,
                -1, +1, +3,
                +2, +0, -1);
            Assert.That(sut.Determinant(), Is.EqualTo(18));
        }

        [Test]
        public void Transpose()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var expectedTranspose = new Matrix3D(
                1, 4, 7,
                2, 5, 8,
                3, 6, 9);
            Matrix3D sut = a.Transposed();
            //Console.WriteLine("transpose of\n{0}\n... is ...\n{1}", a, sut);
            Assert.That(sut, Is.EqualTo(expectedTranspose));

            // should be identical
            double[] rowmajor = a.ToRowMajorArray();
            Assert.That(new Matrix3D(rowmajor), Is.EqualTo(a));

            // should be the transpose
            double[] columnMajor = a.ToColumnMajorArray();
            Assert.That(new Matrix3D(columnMajor), Is.EqualTo(expectedTranspose));
        }

        [Test]
        public void RowsAndColumns()
        {
            var sut = new Matrix3D(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1);
            Vector3D c1 = sut.Column(1);
            //Console.WriteLine("c1 = {0}", c1);
            Assert.That(c1, Is.EqualTo(Vector3D.XAxis));

            Vector3D c2 = sut.Column(2);
            //Console.WriteLine("c2 = {0}", c2);
            Assert.That(c2, Is.EqualTo(Vector3D.YAxis));

            Vector3D c3 = sut.Column(3);
            //Console.WriteLine("c3 = {0}", c3);
            Assert.That(c3, Is.EqualTo(Vector3D.ZAxis));

            Vector3D r1 = sut.Row(1);
            //Console.WriteLine("r1 = {0}", r1);
            Assert.That(r1, Is.EqualTo(Vector3D.XAxis));

            Vector3D r2 = sut.Row(1);
            //Console.WriteLine("r2 = {0}", r2);
            Assert.That(r2, Is.EqualTo(Vector3D.XAxis));

            Vector3D r3 = sut.Row(1);
            //Console.WriteLine("r3 = {0}", r3);
            Assert.That(r3, Is.EqualTo(Vector3D.XAxis));

            sut = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            Assert.That(sut.Column(1), Is.EqualTo(new Vector3D(1, 4, 7)));
            Assert.That(sut.Column(2), Is.EqualTo(new Vector3D(2, 5, 8)));
            Assert.That(sut.Column(3), Is.EqualTo(new Vector3D(3, 6, 9)));
            Assert.That(sut.Row(1), Is.EqualTo(new Vector3D(1, 2, 3)));
            Assert.That(sut.Row(2), Is.EqualTo(new Vector3D(4, 5, 6)));
            Assert.That(sut.Row(3), Is.EqualTo(new Vector3D(7, 8, 9)));
        }

        [Test]
        public void UnaryOperators()
        {
            var a = new Matrix3D(
                +1, +2, +3,
                +4, +5, +6,
                +7, +8, +9);
            var expected = new Matrix3D(
                -1, -2, -3,
                -4, -5, -6,
                -7, -8, -9);
            Matrix3D sut = -a;
            Assert.That(sut, Is.EqualTo(expected));

            sut = +a; // reinforcement
            Assert.That(sut, Is.EqualTo(a));
        }

        [Test]
        public void BinaryOpScalarMultiplication()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var expectation = new Matrix3D(
                2, 4, 6,
                8, 10, 12,
                14, 16, 18);
            Assert.That((2*a), Is.EqualTo(expectation));
            Assert.That((a*2), Is.EqualTo(expectation));
        }

        [Test]
        public void BinaryOpPointMultiplication()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var x = new Point3D(9, 8, 7);
            var expectation = new Point3D(46, 118, 190);
            Point3D sut = a*x;
            //Console.WriteLine("A*{0} = {1}", x, sut);
            Assert.That(sut, Is.EqualTo(expectation));

            var t = new Point3D(100, 200, 300);
            expectation = new Point3D(146, 318, 490);
            sut = a*x + t;
            //Console.WriteLine("a*{0}+{1} = {2}", x, t, sut);
            Assert.That(sut, Is.EqualTo(expectation));
        }

        [Test]
        public void BinaryOpVectorMultiplication()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var x = new Vector3D(9, 8, 7);
            var expectation = new Vector3D(46, 118, 190);
            Assert.That((a*x), Is.EqualTo(expectation));

            var t = new Vector3D(100, 200, 300);
            expectation = new Vector3D(146, 318, 490);
            Assert.That((a*x + t), Is.EqualTo(expectation));
        }

        [Test]
        public void BinaryOpMatrixAddition()
        {
            var a = new Matrix3D(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var b = new Matrix3D(
                0.1, 0.2, 0.3,
                0.4, 0.5, 0.6,
                0.7, 0.8, 0.9);
            Matrix3D sut = a + b;
            var expectation = new Matrix3D(
                1.1, 2.2, 3.3,
                4.4, 5.5, 6.6,
                7.7, 8.8, 9.9);
            Assert.That(sut, Is.EqualTo(expectation));

            sut = a - b;
            expectation = new Matrix3D(
                0.9, 1.8, 2.7,
                3.6, 4.5, 5.4,
                6.3, 7.2, 8.1);
            Assert.That(sut, Is.EqualTo(expectation));
        }

        [Test]
        public void BinaryOpMatrixMultiplication()
        {
            var a = new Matrix3D(
                3, 12, 4,
                5, 6, 8,
                1, 0, 2);
            var b = new Matrix3D(
                7, 3, 8,
                11, 9, 5,
                6, 8, 4);
            Matrix3D sut = a*b;
            //Console.WriteLine(a);
            //Console.WriteLine(b);
            //Console.WriteLine(sut);
            // 177 149 100
            // 149 133 102
            // 19  19  16
            Assert.That(sut.M11, Is.EqualTo(177));
            Assert.That(sut.M12, Is.EqualTo(149));
            Assert.That(sut.M13, Is.EqualTo(100));
            Assert.That(sut.M21, Is.EqualTo(149));
            Assert.That(sut.M22, Is.EqualTo(133));
            Assert.That(sut.M23, Is.EqualTo(102));
            Assert.That(sut.M31, Is.EqualTo(19));
            Assert.That(sut.M32, Is.EqualTo(19));
            Assert.That(sut.M33, Is.EqualTo(16));
        }

        [Test]
        public void ScaleFactory()
        {
            var sut = Matrix3D.Scale(+2.0, -0.6, +4.0);
            Vector3D v = new Vector3D(10, 10, 10);
            Vector3D result = sut*v;
            //Console.WriteLine(sut);
            //Console.WriteLine(result);
            Assert.That(result.X, Is.EqualTo(10*+2.0).Within(1e-10), "X");
            Assert.That(result.Y, Is.EqualTo(10*-0.6).Within(1e-10), "Y");
            Assert.That(result.Z, Is.EqualTo(10*+4.0).Within(1e-10), "Z");
        }

        [Test]
        public void RotateFactory()
        {
            const double theta = 30.0/180.0*Math.PI;
            double sin = Math.Sin(theta);
            double cos = Math.Cos(theta);

            // 30° around X axis
            var sut = Matrix3D.Rotate(Vector3D.XAxis, theta);
            Matrix3D expected = new Matrix3D(
                1, 0, 0,
                0, cos, -sin,
                0, sin, cos);
            //Console.WriteLine(sut);
            Assert.That(sut, Is.EqualTo(expected));
            //Console.WriteLine();

            // 30° around Y axis
            sut = Matrix3D.Rotate(Vector3D.YAxis, theta);
            expected = new Matrix3D(
                cos, 0, sin,
                0, 1, 0,
                -sin, 0, cos);
            //Console.WriteLine(sut);
            Assert.That(sut, Is.EqualTo(expected));
            //Console.WriteLine();

            // 30° around Z axis
            sut = Matrix3D.Rotate(Vector3D.ZAxis, theta);
            expected = new Matrix3D(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1);
            //Console.WriteLine(sut);
            Assert.That(sut, Is.EqualTo(expected));
        }

        [Test]
        public void RotateFactoryFromTwoVectors()
        {
            Vector3D source = new Vector3D(1, 2, 3).Normalized();
            Vector3D target = new Vector3D(1, -1, 1).Normalized();
            Matrix3D sut = Matrix3D.Rotate(source, target);
            //Console.WriteLine(sut + "\n");

            Vector3D x = source*2.5;
            Vector3D expected = target*2.5;
            Vector3D result = sut*x;
            //Console.WriteLine("  x = {0}", x);
            //Console.WriteLine("A*x = {0}", result);
            //Console.WriteLine("   ?= {0}", expected);
            Assert.That(result.X, Is.EqualTo(expected.X).Within(1e-10));
            Assert.That(result.Y, Is.EqualTo(expected.Y).Within(1e-10));
            Assert.That(result.Z, Is.EqualTo(expected.Z).Within(1e-10));
        }

        [Test]
        public void RotateFactorySameVectors()
        {
            Vector3D source = new Vector3D(1, 2, 3);
            Vector3D target = new Vector3D(1, 2, 3);
            Matrix3D sut = Matrix3D.Rotate(source, target);
            //Console.WriteLine(sut);
            Assert.That(sut, Is.EqualTo(Matrix3D.Identity));
        }
    }
}