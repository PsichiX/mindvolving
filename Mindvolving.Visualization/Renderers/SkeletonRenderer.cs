using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class SkeletonRenderer : IRenderable
    {
        private Skeleton skeleton;
        private BoneRenderer boneRenderer;

        public MindvolvingVisualization Visualization { get; set; }

        public SkeletonRenderer(Skeleton skeleton)
        {
            this.skeleton = skeleton;
            boneRenderer = new BoneRenderer(skeleton);
        }

        public void Draw(GameTime gt)
        {
            foreach(Bone bone in skeleton.Bones)
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