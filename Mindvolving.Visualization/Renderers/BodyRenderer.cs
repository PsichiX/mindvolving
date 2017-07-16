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
        public Body Body { get; set; }

        public BodyRenderer()
        {
            bodyPartRenderer = new BodyPartRenderer();
            muscleRenderer = new MuscleRenderer();
            skeletonRenderer = new SkeletonRenderer();
        }

        public BodyRenderer(Body body)
            : this()
        {
            Body = body;
        }

        public void Draw(GameTime gt)
        {
            if (Body == null)
                return;

            foreach (BodyPart part in Body.BodyParts)
            {
                bodyPartRenderer.BodyPart = part;
                bodyPartRenderer.Draw(gt);
            }

            foreach (Muscle muscle in Body.Muscles)
            {
                muscleRenderer.Muscle = muscle;
                muscleRenderer.Draw(gt);
            }

            skeletonRenderer.Skeleton = Body.Skeleton;
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
