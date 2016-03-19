using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpGeometry.EqualityComparer;

namespace SharpGeometry.Tests.EqualityComparer
{
    [TestFixture]
    public class RasterEqualityComparerTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(0.01)]
        [TestCase(17)]
        public void RasterIsMaintainedAroundZero(double rasterSize)
        {
            var sut = new RasterEqualityComparer(rasterSize);
            Assert.That(sut.Centroid, Is.EqualTo(default(Point3D)));

            double range = rasterSize*10;
            double min = -range - rasterSize/2;
            double max = +range + rasterSize/2;
            Point3D[] source = Generate(min, max, rasterSize).ToArray();
            Point3D[] filtered = source.Distinct(sut).ToArray();
            Assert.AreEqual(source, filtered);

            HashSet<Point3D> hashed = new HashSet<Point3D>(source, sut);
            Assert.That(hashed.Count, Is.EqualTo(source.Length));
        }

        [Test]
        public void RasterFailsForLargeValues()
        {
            var sut = new RasterEqualityComparer(1);
            // hashcode is 32bit integer but
            // 3D Point is 3*64bit double => 6 times as much information

            Point3D p1 = new Point3D(0, 0, 1);
            Point3D p2 = new Point3D(0, 1024, 0); // 1<<10 = 2^10 = 1024
            Point3D p3 = new Point3D(1024*1024, 0, 0); // 1<<20 = 2^20 = 1024²
            Vector3D v1 = (Vector3D)p1;
            Vector3D v2 = (Vector3D)p2;
            Vector3D v3 = (Vector3D)p3;

            // equality works
            Assert.False(sut.Equals(p1, p2));
            Assert.False(sut.Equals(p1, p3));
            Assert.False(sut.Equals(v1, v2));
            Assert.False(sut.Equals(v1, v3));

            // but hashing collides
            Assert.That(sut.GetHashCode(p1), Is.EqualTo(sut.GetHashCode(p2)));
            Assert.That(sut.GetHashCode(p1), Is.EqualTo(sut.GetHashCode(p3)));
            Assert.That(sut.GetHashCode(v1), Is.EqualTo(sut.GetHashCode(v2)));
            Assert.That(sut.GetHashCode(v1), Is.EqualTo(sut.GetHashCode(v3)));
        }

        [Test]
        public void CentroidHelpsWithLargeValues()
        {
            Vector3D offset = new Vector3D(1024*1024, 1024, 0);
            Point3D[] source = Generate(-10.5, +10.5, 1)
                .Select(s => s + offset)
                .ToArray();
            int n = source.Length;

            Point3D centroid = new Point3D(0, 0, 0);
            foreach (Point3D p in source)
            {
                centroid += (Vector3D)p/n;
            }

            var sut = new RasterEqualityComparer(1, centroid);
            var hashed = new HashSet<Point3D>(source, sut);
            Assert.That(hashed.Count, Is.EqualTo(n));
        }

        [Test]
        public void CtorThrowsOnIllegalArgs()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new RasterEqualityComparer(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new RasterEqualityComparer(-1));
            // ReSharper restore ObjectCreationAsStatement
        }

        private static IEnumerable<Point3D> Generate(double min, double max, double rasterSize)
        {
            for (double x = min; x <= max; x += rasterSize)
            {
                for (double y = min; y <= max; y += rasterSize)
                {
                    for (double z = min; z <= max; z += rasterSize)
                    {
                        yield return new Point3D(x, y, z);
                    }
                }
            }
        }
    }
}