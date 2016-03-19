using System;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Point3DTests
    {

        [Test]
        public void PointCtorFromArray()
        {
            double[] arr = { 0.1, 0.2, 0.3 };
            var sut = new Point3D(arr);
            Assert.That(sut.IsUndefined(), Is.False);
            Assert.That(sut.X, Is.EqualTo(arr[0]));
            Assert.That(sut.Y, Is.EqualTo(arr[1]));
            Assert.That(sut.Z, Is.EqualTo(arr[2]));
        }

        [Test]
        public void PointCtorFromInvalidArrays()
        {
            double[] empty = { };
            double[] one = { 0.2 };
            double[] two = { 1, 2 };
            double[] many = new double[4];
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new Point3D(null));
            Assert.Throws<ArgumentException>(() => new Point3D(empty));
            Assert.Throws<ArgumentException>(() => new Point3D(one));
            Assert.Throws<ArgumentException>(() => new Point3D(two));
            Assert.Throws<ArgumentException>(() => new Point3D(many));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void UndefinedPoint()
        {
            Func<int, double, double[]> factory = (i, invalidValue) =>
            {
                double[] arr = { 1, 1, 1 };
                arr[i] = invalidValue;
                return arr;
            };
            for (int i = 0; i < 3; i++)
            {
                double[] arr = factory(i, double.NaN);
                var sut = new Point3D(arr);
                Assert.That(sut.IsUndefined());
                arr = factory(i, double.NegativeInfinity);
                sut = new Point3D(arr);
                Assert.That(sut.IsUndefined());
            }
        }

        [Test]
        public void Distance()
        {
            var a = new Point3D(1, 2, 3);
            var b = new Point3D(5, 5, 5);
            var dist2 = Point3D.SquaredDistance(a, b);
            var dist = Point3D.Distance(a, b);

            double expectedSq = 4 * 4 + 3 * 3 + 2 * 2;
            Assert.That(dist2, Is.EqualTo(expectedSq).Within(1e-10));
            Assert.That(dist, Is.EqualTo(Math.Sqrt(expectedSq)).Within(1e-10));

            Assert.That(a.SquaredDistance(b), Is.EqualTo(dist2));
            Assert.That(b.SquaredDistance(a), Is.EqualTo(dist2));
            Assert.That(a.Distance(b), Is.EqualTo(dist));
            Assert.That(a.Distance(b), Is.EqualTo(dist));
        }

        [Test]
        public void PointEquality()
        {
            Point3D a = new Point3D(1, 2, 3);
            Point3D a2 = new Point3D(1.0, 2.0, 3.0);

            Assert.That(a.Equals(a2), Is.True);
            Assert.That(a == a2, Is.True);
            Assert.That(a != a2, Is.False);
            // symmetry
            Assert.That(a2.Equals(a), Is.True);
            Assert.That(a2 == a, Is.True);
            Assert.That(a2 != a, Is.False);

            Point3D b = new Point3D(1, 1, 1);
            Assert.That(a.Equals(b), Is.False);
            Assert.That(a == b, Is.False);
            Assert.That(a != b, Is.True);
            // symmetry
            Assert.That(b.Equals(a), Is.False);
            Assert.That(b == a, Is.False);
            Assert.That(b != a, Is.True);

            // object
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(a.Equals("cat"), Is.False);
            Assert.That(a.Equals(null), Is.False);
        }

        [Test]
        public void UnaryOperators()
        {
            Point3D p = new Point3D(1, 2, 3);
            var sut = +p;
            Assert.That(sut, Is.EqualTo(p));
            sut = -p;
            Assert.That(sut.X, Is.EqualTo(-p.X));
            Assert.That(sut.Y, Is.EqualTo(-p.Y));
            Assert.That(sut.Z, Is.EqualTo(-p.Z));
        }

        [Test]
        public void PointPlusVector()
        {
            var p = new Point3D(1, 2, 3);
            var vec = new Vector3D(-5, -4, -3);

            var sut = p + vec;
            Assert.That(sut, Is.EqualTo(new Point3D(-4, -2, 0)));
        }

        [Test]
        public void PointPlusPoint()
        {
            var p = new Point3D(1, 2, 3);
            var other = new Point3D(-5, -4, -3);

            var sut = p + other;
            Assert.That(sut, Is.EqualTo(new Point3D(-4, -2, 0)));
        }

        [Test]
        public void PointMinusVector()
        {
            var p = new Point3D(1, -2, 3);
            var vec = new Vector3D(-4, 8, -12);

            var sut = p - vec;
            Assert.That(sut, Is.EqualTo(new Point3D(5, -10, 15)));
        }

        [Test]
        public void PointMinusPoint()
        {
            var p = new Point3D(1, 2, 3);
            var other = new Point3D(-5, -4, -3);

            var sut = p - other;
            Assert.That(sut, Is.EqualTo(new Point3D(6, 6, 6)));
        }
    }
}