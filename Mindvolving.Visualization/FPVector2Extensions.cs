using Microsoft.Xna.Framework;
using System.Linq;

namespace Mindvolving.Visualization
{
    public static class FPVector2Extensions
    {
        public static Vector2 ToMGVector2(this FarseerPhysics.Common.Vector2 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2[] ToMGVector2(this FarseerPhysics.Common.Vector2[] vector)
        {
            return vector.Select(v => new Vector2(v.X, v.Y)).ToArray();
        }
    }
}
