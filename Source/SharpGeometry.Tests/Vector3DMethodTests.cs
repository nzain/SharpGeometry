using System;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Vector3DMethodTests
    {
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
    }
}
