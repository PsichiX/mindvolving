using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization
{
    public interface IRenderable : IVisualizationHolder
    {
        void Draw(GameTime gt);
    }
}
