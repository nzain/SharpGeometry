using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace SharpGeometry.Tests
{
    [TestClass]
    public class Vector3DTests
    {
        [TestMethod]
        public void VectorProperties()
        {
            var sut = new Vector3D(1,2,3);
            Console.WriteLine(sut);
            sut.IsUndefined.Should().BeFalse();
            sut.X.Should().BeApproximately(1, 1e-10, "x");
            sut.Y.Should().BeApproximately(2, 1e-10, "y");
            sut.Z.Should().BeApproximately(3, 1e-10, "z");
        }

        [TestMethod]
        public void VectorToStringLengthIsInvariant()
        {
            // this is only true for numbers of the same magnitude (not say X=1000 vs x=1)
            var vec = new Vector3D(1, 2, 3);
            var baseline = vec.ToString();
            Console.WriteLine("{0}: {1}", baseline.Length, baseline);

            Random rnd = new Random(42);
            const int n = 100;
            for (int i = 0; i < n; i++)
            {
                var rndVec = new Vector3D(
                    rnd.NextDouble() * 2 - 1,
                    rnd.NextDouble() * 2 - 1,
                    rnd.NextDouble() * 2 - 1);
                var sut = rndVec.ToString();
                Console.WriteLine("{0}: {1}", sut.Length, sut);
                sut.Should().HaveLength(baseline.Length);
            }
        }
    }
}
