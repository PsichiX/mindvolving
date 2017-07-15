using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using System.Linq;
using FarseerPhysics.Collision.Shapes;

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
            CircleShape circle1 = Current.Part1.RigidBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();
            CircleShape circle2 = Current.Part2.RigidBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle1 != null && circle2 != null)
            {
                Vector2 from1into2Pos = Current.Part2.Position - Current.Part1.Position;
                Vector2 from2into1Pos = Current.Part1.Position - Current.Part2.Position;

                from1into2Pos.Normalize();
                from2into1Pos.Normalize();

                Visualization.Primitive2DRenderer.DrawLine(Current.Part1.Position + from1into2Pos * circle1.Radius, Current.Part2.Position + from2into1Pos * circle2.Radius, Color.Red);
            }
        }

        public void Initialize()
        {
            
        }
    }
}