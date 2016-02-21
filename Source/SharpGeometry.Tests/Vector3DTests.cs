using System;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Vector3DTests
    {
        [Test]
        public void VectorProperties()
        {
            var sut = new Vector3D(1, 2, 3);
            Console.WriteLine(sut);
            Assert.That(sut.IsUndefined(), Is.False);
            Assert.That(sut.X, Is.EqualTo(1), "x");
            Assert.That(sut.Y, Is.EqualTo(2), "y");
            Assert.That(sut.Z, Is.EqualTo(3), "z");
            Assert.That(sut.SquaredLength(), Is.EqualTo(14), "L2");
            Assert.That(sut.Length(), Is.EqualTo(Math.Sqrt(14)), "Length");
        }

        [Test]
        public void ZeroVector()
        {
            var zero = new Vector3D(0, 0, 0);
            Assert.That(zero.X, Is.EqualTo(0));
            Assert.That(zero.Y, Is.EqualTo(0));
            Assert.That(zero.Z, Is.EqualTo(0));

            Assert.That(Vector3D.Zero, Is.EqualTo(zero));
            Assert.That(new Vector3D(), Is.EqualTo(zero));
            Assert.That(default(Vector3D), Is.EqualTo(zero));
        }

        [Test]
        public void VectorAddition()
        {
            Vector3D left = new Vector3D(1, 2, 3);
            Vector3D right = new Vector3D(Math.PI, 4, 5);
            Vector3D expected = new Vector3D(1 + Math.PI, 2 + 4, 3 + 5);
            Vector3D sut = left + right;

            Console.WriteLine($"{left} + {right} = {sut}");
            Assert.That(sut, Is.EqualTo(expected));
        }

        [Test]
        public void VectorSubtraction()
        {
            Vector3D left = new Vector3D(1, 2, 3);
            Vector3D right = new Vector3D(Math.PI, 4, 5);
            Vector3D expected = new Vector3D(1 - Math.PI, 2 - 4, 3 - 5);
            Vector3D sut = left - right;

            Console.WriteLine($"{left} - {right} = {sut}");
            Assert.That(sut, Is.EqualTo(expected));
        }

        [Test]
        public void VectorScalarMultiplication()
        {
            Vector3D v = new Vector3D(1, 2, 3);
            double s = -Math.E;
            Vector3D expected = new Vector3D(s * v.X, s * v.Y, s * v.Z);

            // left multiplication
            Vector3D sut = s * v;
            Console.WriteLine($"{s} * {v} = {sut}");
            Assert.That(sut, Is.EqualTo(expected), "left multiply: s*v");

            // right multiplication
            sut = v * s;
            Console.WriteLine($"{v} * {s} = {sut}");
            Assert.That(sut, Is.EqualTo(expected), "right multiply: v*s");
        }

        [Test]
        public void VectorScalarDivision()
        {
            Vector3D v = new Vector3D(1, 2, 3);
            double s = 10;
            Vector3D expected = new Vector3D(0.1, 0.2, 0.3);
            
            Vector3D sut = v/s;
            Console.WriteLine($"{v} / {s} = {sut}");
            Assert.That(sut, Is.EqualTo(expected));
        }

        [Test]
        public void VectorScalarDivisionByZero()
        {
            Vector3D v = new Vector3D(1, 2, 3);
            Assert.Throws<DivideByZeroException>(delegate { var sut = v / 0; });
        }

        [Test]
        public void VectorNormalized()
        {
            Vector3D v = new Vector3D(1, 2, 3);
            Vector3D sut = v.Normalized();
            Console.WriteLine(sut);
            Assert.That(v, Is.EqualTo(new Vector3D(1, 2, 3)), "immutable type is unchanged");
            Assert.That(sut.Length(), Is.EqualTo(1).Within(1e-10), "normalized length is one");
            Assert.That(sut.X / sut.Y, Is.EqualTo(v.X / v.Y).Within(1e-10), "x-y ratio unchanged");
            Assert.That(sut.X / sut.Z, Is.EqualTo(v.X / v.Z).Within(1e-10), "x-z ratio unchanged");
            Assert.That(sut.Y / sut.Z, Is.EqualTo(v.Y / v.Z).Within(1e-10), "y-z ratio unchanged");

            Assert.Throws<InvalidOperationException>(() => new Vector3D(0, 0, 0).Normalized(), "cannot normalize zero vector");
        }

        [Test]
        public void VectorScaledTo()
        {
            Vector3D v = new Vector3D(1, 2, 3);
            const double targetLength = 14;
            Vector3D sut = v.ScaledTo(targetLength);
            Console.WriteLine(sut);
            Assert.That(v, Is.EqualTo(new Vector3D(1, 2, 3)), "immutable type is unchanged");
            Assert.That(sut.Length(), Is.EqualTo(targetLength).Within(1e-10), $"scaled length is {targetLength}");
            Assert.That(sut.X / sut.Y, Is.EqualTo(v.X / v.Y).Within(1e-10), "x-y ratio unchanged");
            Assert.That(sut.X / sut.Z, Is.EqualTo(v.X / v.Z).Within(1e-10), "x-z ratio unchanged");
            Assert.That(sut.Y / sut.Z, Is.EqualTo(v.Y / v.Z).Within(1e-10), "y-z ratio unchanged");

            Assert.That(sut.ScaledTo(0), Is.EqualTo(Vector3D.Zero), "scale to zero is somewhat useless, but valid");

            Assert.Throws<InvalidOperationException>(() => new Vector3D(0, 0, 0).ScaledTo(targetLength), "cannot normalize zero vector");
            Assert.Throws<ArgumentOutOfRangeException>(() => v.ScaledTo(-1), "negative length is invalid");
        }
    }
}
