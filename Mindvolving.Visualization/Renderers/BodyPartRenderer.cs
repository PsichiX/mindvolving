using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using FarseerPhysics.Collision.Shapes;
using System.Linq;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyPartRenderer : Renderer
    {
        public BodyPart BodyPart { get; set; }

        public BodyPartRenderer()
        {
            BodyPart = null;
        }

        public BodyPartRenderer(BodyPart bodyPart)
        {
            BodyPart = bodyPart;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            CircleShape circle = BodyPart.PhysicalBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle != null)
            {
                Visualization.Primitive2DRenderer.FillCircle(BodyPart.Position, (int)circle.Radius, Color.Black, new Vector2(0.5f, 0.5f));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}