using Microsoft.Xna.Framework;
using System.Linq;

namespace Mindvolving.Visualization
{
    public static class FarseerToMonogameExtensions
    {
        public static Vector2 ToMGVector2(this FarseerPhysics.Common.Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static FarseerPhysics.Common.Vector2 ToFPVector2(this Vector2 vector)
        {
            return new FarseerPhysics.Common.Vector2(vector.X, vector.Y);
        }

        public static Vector2[] ToMGVector2(this FarseerPhysics.Common.Vector2[] vector)
        {
            return vector.Select(v => new Vector2(v.X, v.Y)).ToArray();
        }

        public static Matrix ToMGMatrix(this FarseerPhysics.Common.Matrix matrix)
        {
            return new Matrix()
            {
                M11 = matrix.M11,
                M12 = matrix.M12,
                M13 = matrix.M13,
                M14 = matrix.M14,
                M21 = matrix.M21,
                M22 = matrix.M22,
                M23 = matrix.M23,
                M24 = matrix.M24,
                M31 = matrix.M31,
                M32 = matrix.M32,
                M33 = matrix.M33,
                M34 = matrix.M34,
                M41 = matrix.M41,
                M42 = matrix.M42,
                M43 = matrix.M43,
                M44 = matrix.M44
            };
        }
    }
}
