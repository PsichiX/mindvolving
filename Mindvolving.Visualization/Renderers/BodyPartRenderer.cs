using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using FarseerPhysics.Collision.Shapes;
using System.Linq;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyPartRenderer : IRenderable
    {
        private Body body;

        public MindvolvingVisualization Visualization { get; set; }
        public BodyPart Current { get; set; }

        public BodyPartRenderer(Body body)
        {
            this.body = body;
            Current = null;
        }

        public void Draw(GameTime gt)
        {
            CircleShape circle = Current.PhysicalBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle != null)
            {
                Visualization.Primitive2DRenderer.FillCircle(Current.Position, (int)circle.Radius, Color.Black, new Vector2(0.5f, 0.5f));
            }
        }

        public void Initialize()
        {

        }
    }
}