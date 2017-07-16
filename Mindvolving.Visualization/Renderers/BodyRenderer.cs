using System;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BodyRenderer : Renderer
    {
        private BodyPartRenderer bodyPartRenderer;
        private MuscleRenderer muscleRenderer;
        private SkeletonRenderer skeletonRenderer;

        public Body Body { get; set; }

        public BodyRenderer()
        {

        }

        public BodyRenderer(Body body)
            : this()
        {
            Body = body;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

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

        public override void Initialize()
        {
            base.Initialize();

            bodyPartRenderer = Visualization.CreateRenderer<BodyPartRenderer>();
            muscleRenderer = Visualization.CreateRenderer<MuscleRenderer>();
            skeletonRenderer = Visualization.CreateRenderer<SkeletonRenderer>();
        }
    }
}
