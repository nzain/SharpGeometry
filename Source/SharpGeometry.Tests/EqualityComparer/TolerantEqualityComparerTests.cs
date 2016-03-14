using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpGeometry.EqualityComparer;

namespace SharpGeometry.Tests.EqualityComparer
{
    [TestFixture]
    public class TolerantEqualityComparerTests
    {
        [Test]
        public void CtorThrowsForInvalidArgs()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TolerantEqualityComparer(-1), "negative tolerance");
            Assert.Throws<ArgumentOutOfRangeException>(() => new TolerantEqualityComparer(0), "zero tolerance doesn't make sense");
        }

        [Test]
        public void ComparerIsTolerant()
        {
            const double tolerance = 0.05;
            var sut = new TolerantEqualityComparer(tolerance);

            Vector3D a = new Vector3D(1, 2, 3);
            Vector3D b = new Vector3D(1.04999, 2.04999, 3.04999); // length difference > tolerance
            Assert.True(sut.Equals(a,b));
            Point3D p1 = new Point3D(1, 2, 3);
            Point3D p2 = new Point3D(1.04999, 2.04999, 3.04999);
            Assert.True(sut.Equals(p1, p2));
        }

        [Test]
        public void ComparerIsNotTooTolerant()
        {
            const double tolerance = 0.05;
            var sut = new TolerantEqualityComparer(tolerance);

            Vector3D a = new Vector3D(1, 2, 3);
            Vector3D b = new Vector3D(1.0500001, 2, 3);
            Vector3D c = new Vector3D(1, 2.0500001, 3);
            Vector3D d = new Vector3D(1, 2, 3.0500001);
            Assert.False(sut.Equals(a, b), "x differs");
            Assert.False(sut.Equals(a, c), "y differs");
            Assert.False(sut.Equals(a, d), "z differs");
        }

        [Test]
        public void NUnitCompareLists()
        {
            const double tolerance = 0.05;
            IEqualityComparer<Vector3D> sut = new TolerantEqualityComparer(tolerance);

            Vector3D[] src = new[] { new Vector3D(), Vector3D.XAxis, Vector3D.YAxis, Vector3D.ZAxis, };

            Random rnd = new Random(42);
            Func<Vector3D, double, Vector3D> randomize =
                (v, rng) => new Vector3D(
                    v.X + rnd.NextDouble() * rng - rng / 2,
                    v.Y + rnd.NextDouble() * rng - rng / 2,
                    v.Z + rnd.NextDouble() * rng - rng / 2);

            Vector3D[] ok = src.Select(s => randomize(s, 0.049)).ToArray();
            Vector3D[] bad = src.Select((s, i) => new Vector3D(s.X + 0.1, s.Y - 0.1, s.Z + 0.1)).ToArray();

            Assert.That(src, Is.EqualTo(ok).Using(sut));
            Assert.That(src, Is.Not.EqualTo(bad).Using(sut));
        }

        [Test]
        public void VectorHashTest()
        {
            Random rnd = new Random(42);
            Func<double, Vector3D> generator =
                rng => new Vector3D(
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2);


            const double tolerance = 0.1;
            var sut = new TolerantEqualityComparer(tolerance);
            HashSet<Vector3D> set1 = new HashSet<Vector3D>();
            HashSet<Vector3D> set2 = new HashSet<Vector3D>(sut);
            
            const int n = 100;
            for (int i = 0; i < n; i++)
            {
                Vector3D v = generator(1);
                set1.Add(v);
                set2.Add(v);
            }
            Console.WriteLine($"set with default comparer got {set1.Count} items");
            Console.WriteLine($"set with tolerant comparer got {set2.Count} items.");
            Assert.That(set2.Count, Is.LessThan(set1.Count));
        }

        [Test]
        public void PointHashTest()
        {
            Random rnd = new Random(42);
            Func<double, Point3D> generator =
                rng => new Point3D(
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2);


            const double tolerance = 0.1;
            var sut = new TolerantEqualityComparer(tolerance);
            HashSet<Point3D> set1 = new HashSet<Point3D>();
            HashSet<Point3D> set2 = new HashSet<Point3D>(sut);

            const int n = 100;
            for (int i = 0; i < n; i++)
            {
                Point3D v = generator(1);
                set1.Add(v);
                set2.Add(v);
            }
            Console.WriteLine($"set with default comparer got {set1.Count} items");
            Console.WriteLine($"set with tolerant comparer got {set2.Count} items.");
            Assert.That(set2.Count, Is.LessThan(set1.Count));
        }
    }
}
