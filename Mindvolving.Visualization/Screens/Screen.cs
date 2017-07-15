using System;
using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization.Screens
{
    public abstract class Screen : IUpdateable, IRenderable
    {
        public MindvolvingVisualization Visualization { get; set; }

        public virtual void Draw(GameTime gt)
        {

        }

        public virtual void Initialize()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Unload()
        {

        }
    }
}
