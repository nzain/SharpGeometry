using System;
using System.Collections.Generic;
using System.Linq;

//using static System.Console;
using static System.Diagnostics.Debug;

namespace SharpGeometry.Demo
{
    internal class Program
    {
        private static void Main()
        {
            OperatorDemo();

            WriteLine("-- press any key --");
            Console.ReadKey();
        }

        public static void OperatorDemo()
        {
            Vector3D a = new Vector3D(1, 2, 3);
            Vector3D b = 2.5*a;
            WriteLine(b);
            // [2.500 5.000 7.500]

            Vector3D rotationAxis = Vector3D.ZAxis;
            double angle = Math.PI/2;
            Matrix3D m = Matrix3D.Rotate(rotationAxis, angle);
            WriteLine(m);
            // [ 0.000 -1.000  0.000]
            // [ 1.000  0.000  0.000]
            // [ 0.000  0.000  1.000]

            Vector3D c = m*b;
            WriteLine(c);
            // [-5.000 2.500 7.500]
        }
    }
}