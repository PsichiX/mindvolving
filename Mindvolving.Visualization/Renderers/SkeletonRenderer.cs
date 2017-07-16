using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class SkeletonRenderer : Renderer
    {
        private BoneRenderer boneRenderer;

        public Skeleton Skeleton { get; set; }

        public SkeletonRenderer()
        {

        }

        public SkeletonRenderer(Skeleton skeleton)
        {
            Skeleton = skeleton;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            foreach(Bone bone in Skeleton.Bones)
            {
                boneRenderer.Bone = bone;
                boneRenderer.Draw(gt);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            boneRenderer = Visualization.CreateRenderer<BoneRenderer>();
        }
    }
}