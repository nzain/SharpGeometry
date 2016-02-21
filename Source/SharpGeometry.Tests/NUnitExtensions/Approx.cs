using SharpGeometry.Tests.NUnitExtensions.Constraints;

namespace SharpGeometry.Tests.NUnitExtensions
{
    // Extending NUnit is a pain
    public static class Approx
    {
        public static ApproximatelyConstraint EqualTo(Vector3D expected, double tolerance = 1e-10)
        {
            return new ApproximatelyConstraint(expected, tolerance);
        }
    }
}
