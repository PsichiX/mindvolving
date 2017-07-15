using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Renderers
{
    public class Primitive2DRenderer : IVisualizationHolder
    {
        public MindvolvingVisualization Visualization { get; set; }

        public Color DebugPrintColor { get; set; }

        public Primitive2DRenderer()
        {
            DebugPrintColor = Color.Black;
        }

        public void Initialize()
        {
            
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            Visualization.SpriteBatch.Draw(
                Visualization.Textures.BlankTexture,
                new Rectangle(start.ToPoint(), new Point((int)Math.Ceiling((end - start).Length()), 1)),
                null,
                color, 
                rotation,
                Vector2.Zero,
                SpriteEffects.None,
                0f);
        }

        public void DrawLineStrip(Color color, bool closed, params Vector2[] points)
        {
            if (points.Length < 2)
                throw new Exception("Can't draw a line strip with less than two points!");

            for (int i = 1; i < points.Length; i++)
            {
                DrawLine(points[i - 1], points[i], color);
            }

            if (closed)
                DrawLine(points[points.Length - 1], points[0], color);
        }

        public void FillCircle(Vector2 pos, int radius, Color color)
        {
            FillCircle(pos, radius, color, Vector2.Zero);
        }

        public void FillCircle(Vector2 pos, int radius, Color color, Vector2 origin)
        {
            Vector2 textureSize = new Vector2(Visualization.Textures.Circle.Width, Visualization.Textures.Circle.Height);

            Visualization.SpriteBatch.Draw(Visualization.Textures.Circle, new Rectangle(pos.ToPoint(), new Point(radius * 2)), null, color, 0f, origin * textureSize, SpriteEffects.None, 0f);
        }

        public void DrawPoint(Vector2 pos, Color color)
        {
            Visualization.SpriteBatch.Draw(Visualization.Textures.BlankTexture, new Rectangle(pos.ToPoint(), new Point(1, 1)), color);
        }

        public void DrawTriangle(Vector2 pos1, Vector2 pos2, Vector2 pos3, Color color)
        {
            DrawLine(pos1, pos2, color);
            DrawLine(pos2, pos3, color);
            DrawLine(pos3, pos1, color);
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            DrawLineStrip(color, true,
                          new Vector2(rectangle.Left, rectangle.Top),
                          new Vector2(rectangle.Right, rectangle.Top),
                          new Vector2(rectangle.Right, rectangle.Bottom),
                          new Vector2(rectangle.Left, rectangle.Bottom));
        }

        //void IDebugDrawer.DrawLine(JVector start, JVector end)
        //{
        //    DrawLine(start.ToVector2(), end.ToVector2(), DebugPrintColor);
        //}

        //void IDebugDrawer.DrawPoint(JVector pos)
        //{
        //    DrawPoint(pos.ToVector2(), DebugPrintColor);
        //}

        //void IDebugDrawer.DrawTriangle(JVector pos1, JVector pos2, JVector pos3)
        //{
        //    DrawTriangle(pos1.ToVector2(), pos2.ToVector2(), pos3.ToVector2(), DebugPrintColor);
        //}
    }
}
