using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpGeometry.EqualityComparer;

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
            Assert.That(sut.X/sut.Y, Is.EqualTo(v.X/v.Y).Within(1e-10), "x-y ratio unchanged");
            Assert.That(sut.X/sut.Z, Is.EqualTo(v.X/v.Z).Within(1e-10), "x-z ratio unchanged");
            Assert.That(sut.Y/sut.Z, Is.EqualTo(v.Y/v.Z).Within(1e-10), "y-z ratio unchanged");

            Assert.Throws<InvalidOperationException>(() => new Vector3D(0, 0, 0).Normalized(),
                "cannot normalize zero vector");
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
            Assert.That(sut.X/sut.Y, Is.EqualTo(v.X/v.Y).Within(1e-10), "x-y ratio unchanged");
            Assert.That(sut.X/sut.Z, Is.EqualTo(v.X/v.Z).Within(1e-10), "x-z ratio unchanged");
            Assert.That(sut.Y/sut.Z, Is.EqualTo(v.Y/v.Z).Within(1e-10), "y-z ratio unchanged");

            Assert.That(sut.ScaledTo(0), Is.EqualTo(default(Vector3D)), "scale to zero is somewhat useless, but valid");

            Assert.Throws<InvalidOperationException>(() => new Vector3D(0, 0, 0).ScaledTo(targetLength),
                "cannot normalize zero vector");
            Assert.Throws<ArgumentOutOfRangeException>(() => v.ScaledTo(-1), "negative length is invalid");
        }

        [Test]
        public void DotProduct()
        {
            Vector3D a = new Vector3D(2, 4, 6);
            Vector3D b = new Vector3D(-3, -5, -7);
            var sut = Vector3D.DotProduct(a, b);
            Assert.That(sut, Is.EqualTo(-6 - 20 - 42));
            Assert.That(sut, Is.EqualTo(a.DotProduct(b)));
            Assert.That(sut, Is.EqualTo(b.DotProduct(a)));

            Assert.That(Vector3D.DotProduct(a.Normalized(), a.Normalized()), Is.EqualTo(1).Within(1e-10));
            Assert.That(Vector3D.DotProduct(b.Normalized(), b.Normalized()), Is.EqualTo(1).Within(1e-10));
            Assert.That(Vector3D.DotProduct(Vector3D.XAxis, Vector3D.XAxis), Is.EqualTo(1));
            Assert.That(Vector3D.DotProduct(Vector3D.YAxis, Vector3D.YAxis), Is.EqualTo(1));
            Assert.That(Vector3D.DotProduct(Vector3D.ZAxis, Vector3D.ZAxis), Is.EqualTo(1));

            Assert.That(Vector3D.DotProduct(Vector3D.XAxis, +Vector3D.YAxis), Is.EqualTo(0));
            Assert.That(Vector3D.DotProduct(Vector3D.XAxis, +Vector3D.ZAxis), Is.EqualTo(0));
            Assert.That(Vector3D.DotProduct(Vector3D.YAxis, +Vector3D.ZAxis), Is.EqualTo(0));
            Assert.That(Vector3D.DotProduct(Vector3D.XAxis, -Vector3D.YAxis), Is.EqualTo(0));
            Assert.That(Vector3D.DotProduct(Vector3D.XAxis, -Vector3D.ZAxis), Is.EqualTo(0));
            Assert.That(Vector3D.DotProduct(Vector3D.YAxis, -Vector3D.ZAxis), Is.EqualTo(0));
        }

        [Test]
        public void CrossProduct()
        {
            var xa = Vector3D.XAxis;
            var ya = Vector3D.YAxis;
            var za = Vector3D.ZAxis;
            // verify right hand rule for unit vectors
            Assert.That(Vector3D.CrossProduct(xa, +ya), Is.EqualTo(+za));
            Assert.That(Vector3D.CrossProduct(xa, +za), Is.EqualTo(-ya));
            Assert.That(Vector3D.CrossProduct(ya, +za), Is.EqualTo(+xa));
            // symmetry
            Assert.That(xa.CrossProduct(-ya), Is.EqualTo(-za));
            Assert.That(xa.CrossProduct(-za), Is.EqualTo(+ya));
            Assert.That(ya.CrossProduct(-za), Is.EqualTo(-xa));
        }

        [Test]
        public void InnerAngle()
        {
            Vector3D a = Vector3D.XAxis;
            Vector3D b = Vector3D.XAxis;
            double angle = Vector3D.InnerAngle(a, b);
            Assert.That(angle, Is.EqualTo(0));

            b = 3*Vector3D.YAxis; // 90°, length must not affect the InnerAngle
            angle = Vector3D.InnerAngle(a, b);
            Assert.That(angle, Is.EqualTo(Math.PI/2));

            b = -Vector3D.XAxis; // linearly dependent, facing in opposite directions
            angle = Vector3D.InnerAngle(a, b);
            Assert.That(angle, Is.EqualTo(Math.PI));

            a = new Vector3D(1, 2, 3);
            b = new Vector3D(-3, -7, -1);
            angle = Vector3D.InnerAngle(a, b);
            Assert.That(a.InnerAngle(b), Is.EqualTo(b.InnerAngle(a))); // commutative, it's always the inner InnerAngle
            Assert.That(angle, Is.EqualTo(2.34045339128917).Within(1e-10));
        }

        [Test]
        public void Rotated()
        {
            var v = new Vector3D(1, 2, 3);
            var rotated = v.Rotated(Vector3D.YAxis, Math.PI);
            //Console.WriteLine(rotated);
            Assert.That(rotated.X, Is.EqualTo(-1).Within(1e-10));
            Assert.That(rotated.Z, Is.EqualTo(-3).Within(1e-10));

            v = new Vector3D(1, 0, 0);
            rotated = v.Rotated(Vector3D.YAxis, 10.0/180.0*Math.PI);
            //Console.WriteLine("{0} => {1}", v, rotated);
            Assert.That(rotated.X, Is.EqualTo(+0.985).Within(0.001));
            Assert.That(rotated.Y, Is.EqualTo(+0.000).Within(1e-10));
            Assert.That(rotated.Z, Is.EqualTo(-0.174).Within(0.001));
        }

        [Test]
        public void Rotated2()
        {
            IEqualityComparer<Vector3D> tolerantComparer = new TolerantEqualityComparer(1e-10);
            const double angle = Math.PI/2;
            var vec = new Vector3D(1, 2, 3);
            var axis = Vector3D.ZAxis;
            var sut = vec.Rotated(axis, angle);
            //Console.WriteLine("rotate {0} about {1} by {2}° --> {3}", vec, axis, angle * 180 / Math.PI, sut);
            Assert.That(sut, Is.EqualTo(new Vector3D(-2, +1, 3)).Using(tolerantComparer));

            axis = Vector3D.XAxis;
            sut = vec.Rotated(axis, angle);
            //Console.WriteLine("rotate {0} about {1} by {2}° --> {3}", vec, axis, angle * 180 / Math.PI, sut);
            Assert.That(sut, Is.EqualTo(new Vector3D(1, -3, +2)).Using(tolerantComparer));

            axis = Vector3D.YAxis;
            sut = vec.Rotated(axis, angle);
            //Console.WriteLine("rotate {0} about {1} by {2}° --> {3}", vec, axis, angle * 180 / Math.PI, sut);
            Assert.That(sut, Is.EqualTo(new Vector3D(+3, 2, -1)).Using(tolerantComparer));
        }
    }
}