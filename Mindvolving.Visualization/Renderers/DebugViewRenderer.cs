using System;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using MG = Microsoft.Xna.Framework;

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
            primitiveRenderer.FillCircle(center.ToMGVector2(), (int)radius, new MG.Color(new MG.Vector3(red, blue, green)));
        }

        public override void DrawPolygon(Vector2[] vertices, int count, float red, float blue, float green, bool closed = true)
        {
            primitiveRenderer.DrawLineStrip(new MG.Color(new MG.Vector3(red, blue, green)), closed, vertices.ToMGVector2());
        }

        public override void DrawSegment(Vector2 start, Vector2 end, float red, float blue, float green)
        {
            primitiveRenderer.DrawLine(start.ToMGVector2(), end.ToMGVector2(), new MG.Color(new MG.Vector3(red, blue, green)));
        }

        public override void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, float red, float blue, float green)
        {
            primitiveRenderer.FillCircle(center.ToMGVector2(), (int)radius, new MG.Color(new MG.Vector3(red, blue, green)));
        }

        public override void DrawSolidPolygon(Vector2[] vertices, int count, float red, float blue, float green)
        {
            primitiveRenderer.DrawLineStrip(new MG.Color(new MG.Vector3(red, blue, green)), true, vertices.ToMGVector2());
        }

        public override void DrawTransform(ref Transform transform)
        {
            
        }
    }
}
