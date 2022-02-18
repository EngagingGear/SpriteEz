using System;
using System.Collections.Generic;

namespace SpriteEz
{
    public class KnownAutoSizes
    {
        public const string Xs = "xs";
        public const string Sm = "sm";
        public const string Md = "md";
        public const string Lg = "lg";
        public const string Xl = "xl";
        public const string Xxl = "xxl";
        private static readonly IDictionary<string, MediaBreakpointConstraint> _constraints;

        static KnownAutoSizes()
        {
            _constraints = new Dictionary<string, MediaBreakpointConstraint>();
            _constraints[Xs] = new MediaBreakpointConstraint(0, 575);
            _constraints[Sm] = new MediaBreakpointConstraint(576);
            _constraints[Md] = new MediaBreakpointConstraint(768);
            _constraints[Lg] = new MediaBreakpointConstraint(992);
            _constraints[Xl] = new MediaBreakpointConstraint(1200);
            _constraints[Xxl] = new MediaBreakpointConstraint(1400);
        }

        public static MediaBreakpointConstraint GetByCode(string key)
        {
            if (_constraints.TryGetValue(key, out var constraint))
            {
                return constraint;
            }

            throw new InvalidOperationException($"No defined constraint for key: {key}");
        }
    }
}