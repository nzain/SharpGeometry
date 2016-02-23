using System;
using System.Collections.Generic;
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
            const double s = -Math.E;
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

            Assert.That(sut.ScaledTo(0), Is.EqualTo(default(Vector3D)), "scale to zero is somewhat useless, but valid");

            Assert.Throws<InvalidOperationException>(() => new Vector3D(0, 0, 0).ScaledTo(targetLength), "cannot normalize zero vector");
            Assert.Throws<ArgumentOutOfRangeException>(() => v.ScaledTo(-1), "negative length is invalid");
        }

        [Test]
        public void VectorEquality()
        {
            Vector3D a = new Vector3D(1, 2, 3);
            Vector3D a2 = new Vector3D(1.0, 2.0, 3.0);

            Assert.That(a.Equals(a2), Is.True);
            Assert.That(a == a2, Is.True);
            Assert.That(a != a2, Is.False);
            // symmetry
            Assert.That(a2.Equals(a), Is.True);
            Assert.That(a2 == a, Is.True);
            Assert.That(a2 != a, Is.False);

            Vector3D b = new Vector3D(1, 1, 1);
            Assert.That(a.Equals(b), Is.False);
            Assert.That(a == b, Is.False);
            Assert.That(a != b, Is.True);
            // symmetry
            Assert.That(b.Equals(a), Is.False);
            Assert.That(b == a, Is.False);
            Assert.That(b != a, Is.True);
        }

        [Test]
        public void VectorHashCodeCollisions()
        {
            const int n = 100000;
            Random rnd = new Random(42);
            HashSet<int> hashcodes = new HashSet<int>();
            for (int i = 0; i < n; i++)
            {
                Vector3D v = new Vector3D(rnd.NextDouble() * 2 - 1, 
                    rnd.NextDouble() * 100 - 50, 
                    rnd.NextDouble() * 20 - 10);
                int hash = v.GetHashCode();
                hashcodes.Add(hash);
            }
            int collisions = n - hashcodes.Count;
            Console.WriteLine($"{n} random vectors produced {hashcodes.Count} different hashcodes => {n-hashcodes.Count} collisions");
            var percent = (double)collisions / (double)n;
            Console.WriteLine($"collisions: {percent:P3} (raw {percent:F5})");
            Assert.That(percent, Is.LessThan(0.01));
        }
    }
}
