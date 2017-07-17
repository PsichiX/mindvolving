using Microsoft.Xna.Framework;
using Mindvolving.Organisms;

namespace Mindvolving.Visualization.Renderers
{
    public class BoneRenderer : Renderer
    {
        public Organ Organ { get; set; }

        public BoneRenderer()
        {

        }

        public BoneRenderer(Organ organ)
        {
            Organ = organ;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            if (Organ.Bone == null)
                return;

            Visualization.Primitive2DRenderer.DrawLine(Organ.Bone.BodyA.Position.ToMGVector2(), Organ.Bone.BodyB.Position.ToMGVector2(), Color.Blue);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
