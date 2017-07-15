using System;
using Jitter.LinearMath;
using Jitter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Renderers
{
    public class Primitive2DRenderer : IVisualizationHolder, IDebugDrawer
    {
        public MindvolvingVisualization Visualization { get; set; }

        public void Initialize()
        {
            
        }

        public void DrawLine(Vector2 start, Vector2 end)
        {
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.Y);

            Visualization.SpriteBatch.Draw(
                Visualization.Textures.BlankTexture,
                new Rectangle(start.ToPoint(), new Point((int)(end - start).Length(), 0)),
                null,
                Color.Black, 
                rotation,
                Vector2.Zero,
                SpriteEffects.None,
                0f);
        }

        public void DrawPoint(Vector2 pos)
        {
            Visualization.SpriteBatch.Draw(Visualization.Textures.BlankTexture, new Rectangle(pos.ToPoint(), new Point(1, 1)), Color.Black);
        }

        public void DrawTriangle(Vector2 pos1, Vector2 pos2, Vector2 pos3)
        {
            DrawLine(pos1, pos2);
            DrawLine(pos2, pos3);
            DrawLine(pos3, pos1);
        }

        void IDebugDrawer.DrawLine(JVector start, JVector end)
        {
            DrawLine(start.ToVector2(), end.ToVector2());
        }

        void IDebugDrawer.DrawPoint(JVector pos)
        {
            DrawPoint(pos.ToVector2());
        }

        void IDebugDrawer.DrawTriangle(JVector pos1, JVector pos2, JVector pos3)
        {
            DrawTriangle(pos1.ToVector2(), pos2.ToVector2(), pos3.ToVector2());
        }
    }
}
