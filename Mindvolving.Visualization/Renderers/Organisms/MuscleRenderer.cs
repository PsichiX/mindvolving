using Microsoft.Xna.Framework;
using Mindvolving.Organisms;

namespace Mindvolving.Visualization.Renderers.Organisms
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

            Visualization.Primitive2DRenderer.DrawLine(Muscle.Joint.WorldAnchorA.ToMGVector2(), Muscle.Joint.WorldAnchorB.ToMGVector2(), Color.Red);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}