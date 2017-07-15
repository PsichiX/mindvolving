using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyRenderer : IRenderable
    {
        private Body body;
        private BodyPartRenderer bodyPartRenderer;
        private MuscleRenderer muscleRenderer;
        private SkeletonRenderer skeletonRenderer;

        public MindvolvingVisualization Visualization { get; set; }

        public BodyRenderer(Body body)
        {
            this.body = body;
            bodyPartRenderer = new BodyPartRenderer(body);
            muscleRenderer = new MuscleRenderer(body);
            skeletonRenderer = new SkeletonRenderer(body.Skeleton);
        }
    
        public void Draw(GameTime gt)
        {
            bodyPartRenderer.Draw(gt);
            muscleRenderer.Draw(gt);
            skeletonRenderer.Draw(gt);
        }

        public void Initialize()
        {
            bodyPartRenderer.Visualization = Visualization;
            muscleRenderer.Visualization = Visualization;
            skeletonRenderer.Visualization = Visualization;
        }
    }
}
