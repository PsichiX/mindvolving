using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BoneRenderer : IRenderable
    {
        private Skeleton skeleton;

        public MindvolvingVisualization Visualization { get; set; }
        public Bone Current { get; internal set; }

        public BoneRenderer(Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }

        public void Draw(GameTime gt)
        {
            Visualization.Primitive2DRenderer.DrawLine(Current.Part1.Position, Current.Part2.Position, Color.Blue);
        }

        public void Initialize()
        {

        }
    }
}
