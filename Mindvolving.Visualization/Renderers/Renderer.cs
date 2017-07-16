using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization.Renderers
{
    public abstract class Renderer : IRenderable
    {
        private bool initialized;

        public MindvolvingVisualization Visualization { get; set; }

        public Renderer()
        {
            initialized = false;
        }

        public virtual void Draw(GameTime gt)
        {
            System.Diagnostics.Debug.Assert(initialized, string.Format("{0} needs to be initialized before drawing", GetType().Name));
            System.Diagnostics.Debug.Assert(Visualization != null, string.Format("{0}'s Visualization must be set before drawing", GetType().Name));
        }

        public virtual void Initialize()
        {
            System.Diagnostics.Debug.Assert(Visualization != null, string.Format("{0}'s Visualization must be set before initializing", GetType().Name));

            initialized = true;
        }
    }
}
