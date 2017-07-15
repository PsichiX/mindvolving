using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class SkeletonRenderer : IRenderable
    {
        private BoneRenderer boneRenderer;

        public MindvolvingVisualization Visualization { get; set; }
        public Skeleton Current { get; set; }

        public SkeletonRenderer()
        {
            boneRenderer = new BoneRenderer();
        }

        public void Draw(GameTime gt)
        {
            foreach(Bone bone in Current.Bones)
            {
                boneRenderer.Current = bone;
                boneRenderer.Draw(gt);
            }
        }

        public void Initialize()
        {
            boneRenderer.Visualization = Visualization;

            boneRenderer.Initialize();
        }
    }
}