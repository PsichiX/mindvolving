using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Organism;

namespace Mindvolving.Visualization.Renderers
{
    public class BoneRenderer : IRenderable
    {
        private Skeleton skeleton;

        public MindvolvingVisualization Visualization { get; set; }

        public BoneRenderer(Skeleton skeleton)
        {
            this.skeleton = skeleton;
        }

        public void Draw(GameTime gt)
        {

        }

        public void Initialize()
        {

        }
    }
}
