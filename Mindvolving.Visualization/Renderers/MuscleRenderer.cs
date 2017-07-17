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

            Visualization.Primitive2DRenderer.DrawLine(Muscle.Joint.WorldAnchorA.ToMGVector2(), Muscle.Joint.WorldAnchorB.ToMGVector2(), Color.Red);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}