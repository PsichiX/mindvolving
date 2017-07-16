using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using FarseerPhysics.Collision.Shapes;
using System.Linq;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyPartRenderer : IRenderable
    {
        public MindvolvingVisualization Visualization { get; set; }
        public BodyPart BodyPart { get; set; }

        public BodyPartRenderer()
        {
            BodyPart = null;
        }

        public BodyPartRenderer(BodyPart bodyPart)
        {
            BodyPart = bodyPart;
        }

        public void Draw(GameTime gt)
        {
            CircleShape circle = BodyPart.PhysicalBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle != null)
            {
                Visualization.Primitive2DRenderer.FillCircle(BodyPart.Position, (int)circle.Radius, Color.White, new Vector2(0.5f, 0.5f));
            }
        }

        public void Initialize()
        {

        }
    }
}