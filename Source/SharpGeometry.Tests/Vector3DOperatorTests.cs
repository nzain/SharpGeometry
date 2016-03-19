using System;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Vector3DOperatorTests
    {
        [Test]
        public void UnaryOperators()
        {
            Vector3D a = new Vector3D(1, 2, 3);
            var sut = +a;
            Assert.That(a, Is.EqualTo(sut));
            sut = -a;
            Assert.That(sut.X, Is.EqualTo(-a.X));
            Assert.That(sut.Y, Is.EqualTo(-a.Y));
            Assert.That(sut.Z, Is.EqualTo(-a.Z));
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
            const double s = -Math.E;
            Vector3D expected = new Vector3D(s*v.X, s*v.Y, s*v.Z);

            // left multiplication
            Vector3D sut = s*v;
            Console.WriteLine($"{s} * {v} = {sut}");
            Assert.That(sut, Is.EqualTo(expected), "left multiply: s*v");

            // right multiplication
            sut = v*s;
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
            Assert.Throws<DivideByZeroException>(() => Console.WriteLine(v/0));
        }
    }
}