using System;
using Microsoft.Xna.Framework;
using System.Linq;
using FarseerPhysics.Collision.Shapes;

namespace Mindvolving.Visualization.Renderers
{
    public class MuscleRenderer : Renderer
    {
        public Organisms.Muscle Muscle { get; set; }

        public MuscleRenderer()
        {

        }

        public MuscleRenderer(Organisms.Muscle muscle)
        {
            Muscle = muscle;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            CircleShape circleFrom = Muscle.From.Body.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();
            CircleShape circleTo = Muscle.To.Body.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circleFrom != null && circleTo != null)
            {
                var from1into2Pos = Muscle.To.Body.Position - Muscle.From.Body.Position;
                var from2into1Pos = Muscle.From.Body.Position - Muscle.To.Body.Position;

                from1into2Pos.Normalize();
                from2into1Pos.Normalize();

                Visualization.Primitive2DRenderer.DrawLine((Muscle.From.Body.Position + from1into2Pos * circleFrom.Radius).ToMGVector2(), (Muscle.To.Body.Position + from2into1Pos * circleTo.Radius).ToMGVector2(), Color.Red);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}