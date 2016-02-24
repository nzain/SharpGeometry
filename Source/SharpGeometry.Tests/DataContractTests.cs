using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;

namespace SharpGeometry.Tests
{
    [TestFixture]
    public class DataContractTests
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 3)]
        [TestCase(long.MaxValue, Math.E, Math.PI)]
        [TestCase(double.MaxValue, double.MinValue, double.NegativeInfinity)]
        public void Vector3DIsSerialized(double x, double y, double z)
        {
            Vector3D sut = new Vector3D(x, y, z);
            Vector3D result = DataContractSerializationRoundTrip(sut);
            Assert.That(sut, Is.EqualTo(result), "serialization should provide exact results");
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(1, 2, 3)]
        [TestCase(long.MaxValue, Math.E, Math.PI)]
        [TestCase(double.MaxValue, double.MinValue, double.NegativeInfinity)]
        public void Point3DIsSerialized(double x, double y, double z)
        {
            Point3D sut = new Point3D(x, y, z);
            Point3D result = DataContractSerializationRoundTrip(sut);
            Assert.That(sut, Is.EqualTo(result), "serialization should provide exact results");
        }

        /// <summary>
        /// First serialize into a <see cref="MemoryStream"/> using a <see cref="DataContractSerializer"/>.
        /// Then print the content, and finally deserialize back into an object of type <typeparam name="T"/>.
        /// </summary>
        /// <param name="obj">The object (not null) to serialize and deserialize.</param>
        /// <returns>The deserialized result.</returns>
        private static T DataContractSerializationRoundTrip<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Console.WriteLine($"before serialization  : {obj.GetType().Name} {obj}");
            using (var memoryStream = new MemoryStream())
            {
                // 1. serialize
                var serializer = new DataContractSerializer(obj.GetType());
                using (var w = XmlWriter.Create(memoryStream, new XmlWriterSettings { Indent = true }))
                {
                    serializer.WriteObject(w, obj);
                }

                // 2. to string
                memoryStream.Position = 0;
                StreamReader sr = new StreamReader(memoryStream); // don't dispose this one
                Console.WriteLine("=====[Serialized]=====");
                Console.WriteLine(sr.ReadToEnd());
                Console.WriteLine("======================");

                // 3. deserialize
                memoryStream.Position = 0;
                obj = (T)serializer.ReadObject(memoryStream);
                Console.WriteLine($"after deserialization: {obj?.GetType().Name} {obj}");
                sr.Dispose();
                return obj;
            }
        }
    }
}
