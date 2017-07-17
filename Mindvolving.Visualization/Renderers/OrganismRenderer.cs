using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Mindvolving.Organisms;

namespace Mindvolving.Visualization.Renderers
{
    public class OrganismRenderer : Renderer
    {
        private OrganRenderer organRenderer;
        private MuscleRenderer muscleRenderer;
        private BoneRenderer boneRenderer;

        public Organisms.Organism Organism { get; set; }

        public OrganismRenderer()
        {

        }

        public OrganismRenderer(Organisms.Organism organism)
            : this()
        {
            Organism = organism;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            if (Organism == null)
                return;

            DrawOrgansAndBones(gt);

            foreach (var muscle in Organism.Muscles)
            {
                muscleRenderer.Muscle = muscle;
                muscleRenderer.Draw(gt);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            organRenderer = Visualization.CreateRenderer<OrganRenderer>();
            muscleRenderer = Visualization.CreateRenderer<MuscleRenderer>();
            boneRenderer = Visualization.CreateRenderer<BoneRenderer>();
        }

        private void DrawOrgansAndBones(GameTime gt)
        {
            Stack<Organ> organsStack = new Stack<Organ>();
            organsStack.Push(Organism.Root);

            while (organsStack.Count != 0)
            {
                Organ organ = organsStack.Pop();
                organRenderer.Organ = organ;
                organRenderer.Draw(gt);
                boneRenderer.Organ = organ;
                boneRenderer.Draw(gt);

                for (int i = 0; i < organ.Children.Count; i++)
                {
                    organsStack.Push(organ.Children[i]);
                }
            }
        }
    }
}
