using Jitter.LinearMath;
using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization
{
    public static class JVectorExtensions
    {
        public static Vector2 ToVector2(this JVector jvec)
        {
            return new Vector2(jvec.X, jvec.Y);
        }
    }
}
