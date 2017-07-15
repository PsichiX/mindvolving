using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using Jitter.Collision.Shapes;

namespace Mindvolving.Visualization.Renderers
{
    public class MuscleRenderer : IRenderable
    {
        private Body body;

        public MindvolvingVisualization Visualization { get; set; }
        public Muscle Current { get; set; }

        public MuscleRenderer(Body body)
        {
            this.body = body;
        }

        public void Draw(GameTime gt)
        {
            if (Current.Part1.RigidBody.Shape is SphereShape && Current.Part2.RigidBody.Shape is SphereShape)
            {
                SphereShape sphere1 = (SphereShape)Current.Part1.RigidBody.Shape;
                SphereShape sphere2 = (SphereShape)Current.Part2.RigidBody.Shape;

                Vector2 from1into2Pos = Current.Part2.Position - Current.Part1.Position;
                Vector2 from2into1Pos = Current.Part1.Position - Current.Part2.Position;

                from1into2Pos.Normalize();
                from2into1Pos.Normalize();

                Visualization.Primitive2DRenderer.DrawLine(Current.Part1.Position + from1into2Pos * sphere1.Radius, Current.Part2.Position + from2into1Pos * sphere2.Radius, Color.Red);
            }
        }

        public void Initialize()
        {

        }
    }
}