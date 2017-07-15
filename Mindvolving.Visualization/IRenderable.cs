using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization
{
    public interface IRenderable : IVisualizationComponent
    {
        void Draw(GameTime gt);
    }
}
