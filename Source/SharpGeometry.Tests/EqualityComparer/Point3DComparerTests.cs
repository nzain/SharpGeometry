using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpGeometry.EqualityComparer;

namespace SharpGeometry.Tests.EqualityComparer
{

    [TestFixture]
    public class Point3DComparerTests
    {
        [Test]
        public void CtorThrowsForInvalidArgs()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Point3DComparer(-1), "negative tolerance");
            Assert.Throws<ArgumentOutOfRangeException>(() => new Point3DComparer(0), "zero tolerance doesn't make sense");
        }

        [Test]
        public void ComparerIsTolerant()
        {
            const double tolerance = 0.05;
            var sut = new Point3DComparer(tolerance);

            Point3D a = new Point3D(1, 2, 3);
            Point3D b = new Point3D(1.04999, 2.04999, 3.04999); // length difference > tolerance
            Assert.True(sut.Equals(a, b));
        }

        [Test]
        public void ComparerIsNotTooTolerant()
        {
            const double tolerance = 0.05;
            var sut = new Point3DComparer(tolerance);

            Point3D a = new Point3D(1, 2, 3);
            Point3D b = new Point3D(1.0500001, 2, 3);
            Point3D c = new Point3D(1, 2.0500001, 3);
            Point3D d = new Point3D(1, 2, 3.0500001);
            Assert.False(sut.Equals(a, b), "x differs");
            Assert.False(sut.Equals(a, c), "y differs");
            Assert.False(sut.Equals(a, d), "z differs");
        }

        [Test]
        public void HashTest()
        {
            Random rnd = new Random(42);
            Func<double, Point3D> generator =
                rng => new Point3D(
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2,
                    rnd.NextDouble() * rng - rng / 2);


            const double tolerance = 0.1;
            var sut = new Point3DComparer(tolerance);
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
