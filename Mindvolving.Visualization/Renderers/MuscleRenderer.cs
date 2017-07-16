using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;
using System.Linq;
using FarseerPhysics.Collision.Shapes;

namespace Mindvolving.Visualization.Renderers
{
    public class MuscleRenderer : Renderer
    {
        public Muscle Muscle { get; set; }

        public MuscleRenderer()
        {

        }

        public MuscleRenderer(Muscle muscle)
        {
            Muscle = muscle;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            CircleShape circle1 = Muscle.Part1.PhysicalBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();
            CircleShape circle2 = Muscle.Part2.PhysicalBody.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle1 != null && circle2 != null)
            {
                Vector2 from1into2Pos = Muscle.Part2.Position - Muscle.Part1.Position;
                Vector2 from2into1Pos = Muscle.Part1.Position - Muscle.Part2.Position;

                from1into2Pos.Normalize();
                from2into1Pos.Normalize();

                Visualization.Primitive2DRenderer.DrawLine(Muscle.Part1.Position + from1into2Pos * circle1.Radius, Muscle.Part2.Position + from2into1Pos * circle2.Radius, Color.Red);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}