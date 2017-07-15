using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyRenderer : IRenderable
    {
        private BodyPartRenderer bodyPartRenderer;
        private MuscleRenderer muscleRenderer;
        private SkeletonRenderer skeletonRenderer;

        public MindvolvingVisualization Visualization { get; set; }
        public Body Current { get; set; }

        public BodyRenderer()
        {
            bodyPartRenderer = new BodyPartRenderer();
            muscleRenderer = new MuscleRenderer();
            skeletonRenderer = new SkeletonRenderer();
        }
    
        public void Draw(GameTime gt)
        {
            foreach (BodyPart part in Current.BodyParts)
            {
                bodyPartRenderer.Current = part;
                bodyPartRenderer.Draw(gt);
            }

            foreach (Muscle muscle in Current.Muscles)
            {
                muscleRenderer.Current = muscle;
                muscleRenderer.Draw(gt);
            }

            skeletonRenderer.Current = Current.Skeleton;
            skeletonRenderer.Draw(gt);
        }

        public void Initialize()
        {
            bodyPartRenderer.Visualization = Visualization;
            muscleRenderer.Visualization = Visualization;
            skeletonRenderer.Visualization = Visualization;

            bodyPartRenderer.Initialize();
            muscleRenderer.Initialize();
            skeletonRenderer.Initialize();
        }
    }
}
