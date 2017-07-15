using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using Jitter.Collision.Shapes;

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
            if (Current.RigidBody.Shape is SphereShape)
            {
                SphereShape sphere = (SphereShape)Current.RigidBody.Shape;

                Visualization.Primitive2DRenderer.FillCircle(Current.Position, (int)sphere.Radius, Color.Black);
            }
        }

        public void Initialize()
        {

        }
    }
}