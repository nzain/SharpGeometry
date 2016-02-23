using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class Point3DTests
    {
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
        public void PointPlusVector()
        {
            var p = new Point3D(1, 2, 3);
            var vec = new Vector3D(-5, -4, -3);

            var sut = p + vec;
            Console.WriteLine($"{p} + {vec} = {sut}");
            Assert.That(sut, Is.EqualTo(new Point3D(-4, -2, 0)));
        }

        [Test]
        public void PointMinusVector()
        {
            var p = new Point3D(1, -2, 3);
            var vec = new Vector3D(-4, 8, -12);

            var sut = p - vec;
            Console.WriteLine($"{p} - {vec} = {sut}");
            Assert.That(sut, Is.EqualTo(new Point3D(5, -10, 15)));
        }
    }
}
