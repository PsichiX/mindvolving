using System;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization.Renderers
{
    public class DebugViewRenderer : DebugViewBase
    {
        private Primitive2DRenderer primitiveRenderer;

        public DebugViewRenderer(World world, Primitive2DRenderer primitiveRenderer) 
            : base(world)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        public override void DrawCircle(Vector2 center, float radius, float red, float blue, float green)
        {
            primitiveRenderer.FillCircle(center, (int)radius, new Color(new Vector3(red, blue, green)));
        }

        public override void DrawPolygon(Vector2[] vertices, int count, float red, float blue, float green, bool closed = true)
        {
            primitiveRenderer.DrawLineStrip(new Color(new Vector3(red, blue, green)), closed, vertices);
        }

        public override void DrawSegment(Vector2 start, Vector2 end, float red, float blue, float green)
        {
            primitiveRenderer.DrawLine(start, end, new Color(new Vector3(red, blue, green)));
        }

        public override void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, float red, float blue, float green)
        {
            primitiveRenderer.FillCircle(center, (int)radius, new Color(new Vector3(red, blue, green)));
        }

        public override void DrawSolidPolygon(Vector2[] vertices, int count, float red, float blue, float green)
        {
            primitiveRenderer.DrawLineStrip(new Color(new Vector3(red, blue, green)), true, vertices);
        }

        public override void DrawTransform(ref Transform transform)
        {
            
        }
    }
}
