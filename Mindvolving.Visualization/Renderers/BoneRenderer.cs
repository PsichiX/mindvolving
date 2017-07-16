using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BoneRenderer : IRenderable
    {
        public MindvolvingVisualization Visualization { get; set; }
        public Bone Bone { get; set; }

        public BoneRenderer()
        {

        }

        public BoneRenderer(Bone bone)
        {
            Bone = bone;
        }

        public void Draw(GameTime gt)
        {
            Visualization.Primitive2DRenderer.DrawLine(Bone.Part1.Position, Bone.Part2.Position, Color.Blue);
        }

        public void Initialize()
        {

        }
    }
}
