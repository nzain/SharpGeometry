
# SharpGeometry 
[![Build status](https://ci.appveyor.com/api/projects/status/5f3pmxvs73hd22x1?svg=true)](https://ci.appveyor.com/project/nzain/sharpgeometry)
[![codecov.io](https://codecov.io/github/nzain/SharpGeometry/coverage.svg?branch=master)](https://codecov.io/github/nzain/SharpGeometry?branch=master)

This is a 3D geometry package for .NET, written in C#. The three core structs

* Vector3D
* Point3D
* Matrix3D

support intuitive mathematical notation by operator overloading. This allows you to write things like

    Vector3D a = new Vector3D(1, 2, 3);
    Vector3D b = 2.5 * a;
    WriteLine(b);
    // [2.500 5.000 7.500]
    
    Vector3D rotationAxis = Vector3D.ZAxis;
    double angle = Math.PI/2;
    Matrix3D m = Matrix3D.Rotate(rotationAxis, angle);
    WriteLine(m);
    // [ 0.000 -1.000  0.000]
    // [ 1.000  0.000  0.000]
    // [ 0.000  0.000  1.000]
    
    Vector3D c = m * b;
    WriteLine(c);
    // [-5.000 2.500 7.500]
