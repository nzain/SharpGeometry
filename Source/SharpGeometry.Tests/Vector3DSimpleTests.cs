using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Vector3DSimpleTests
    {
        [Test]
        public void VectorCtor()
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
        public void VectorCtorFromArray()
        {
            double[] arr = { 0.1, 0.2, 0.3 };
            var sut = new Vector3D(arr);
            Assert.That(sut.X, Is.EqualTo(arr[0]));
            Assert.That(sut.Y, Is.EqualTo(arr[1]));
            Assert.That(sut.Z, Is.EqualTo(arr[2]));
        }

        [Test]
        public void VectorCtorFromInvalidArrays()
        {
            double[] empty = { };
            double[] one = { 0.2 };
            double[] two = { 1, 2 };
            double[] many = new double[4];
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new Vector3D(null));
            Assert.Throws<ArgumentException>(() => new Vector3D(empty));
            Assert.Throws<ArgumentException>(() => new Vector3D(one));
            Assert.Throws<ArgumentException>(() => new Vector3D(two));
            Assert.Throws<ArgumentException>(() => new Vector3D(many));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ZeroVector()
        {
            var zero = new Vector3D(0, 0, 0);
            Assert.That(new Vector3D(), Is.EqualTo(zero));
            Assert.That(default(Vector3D), Is.EqualTo(zero));
            Assert.That(zero.IsZero(), Is.True);
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

            // obj
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(a.Equals("cat"), Is.False);
            Assert.That(a.Equals(null), Is.False);
        }

        [Test]
        public void VectorHashCodeCollisions()
        {
            const int n = 100000;
            Random rnd = new Random(42);
            HashSet<int> hashcodes = new HashSet<int>();
            for (int i = 0; i < n; i++)
            {
                Vector3D v = new Vector3D(rnd.NextDouble()*2 - 1,
                    rnd.NextDouble()*100 - 50,
                    rnd.NextDouble()*20 - 10);
                int hash = v.GetHashCode();
                hashcodes.Add(hash);
            }
            int collisions = n - hashcodes.Count;
            Console.WriteLine(
                $"{n} random vectors produced {hashcodes.Count} different hashcodes => {n - hashcodes.Count} collisions");
            double percent = (double)collisions/n;
            Console.WriteLine($"collisions: {percent:P3} (raw {percent:F5})");
            Assert.That(percent, Is.LessThan(0.01));
        }
    }
}