using System;
using NUnit.Framework.Constraints;

namespace SharpGeometry.Tests.NUnitExtensions.Constraints
{
    public class ApproximatelyConstraint : Constraint
    {
        private double _tolerance = -1;

        public ApproximatelyConstraint(object expected, double tolerance)
            : base(expected)
        {
            if (tolerance <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tolerance));
            }
            this._tolerance = tolerance;
            this.Description = $"{expected} +/- {tolerance}";
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Vector3D? v = actual as Vector3D?; // generics and value types don't work well together.
            if (v.HasValue)
            {
                Vector3D actualVec = v.Value;
                Vector3D expected = (Vector3D)this.Arguments[0];
                double dx = Math.Abs(expected.X - actualVec.X);
                double dy = Math.Abs(expected.Y - actualVec.Y);
                double dz = Math.Abs(expected.Z - actualVec.Z);
                bool success = dx <= this._tolerance && dy <= this._tolerance && dz <= this._tolerance;
                return new ConstraintResult(this, actual, success);
            }
            // TODO how to display extra info here? With nunit 2.x we could write custom messages...
            return new ConstraintResult(this, actual, false);
        }
    }
}
