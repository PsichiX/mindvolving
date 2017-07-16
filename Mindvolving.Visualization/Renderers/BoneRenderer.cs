using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BoneRenderer : Renderer
    {
        public Bone Bone { get; set; }

        public BoneRenderer()
        {

        }

        public BoneRenderer(Bone bone)
        {
            Bone = bone;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Visualization.Primitive2DRenderer.DrawLine(Bone.Part1.Position, Bone.Part2.Position, Color.Blue);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
