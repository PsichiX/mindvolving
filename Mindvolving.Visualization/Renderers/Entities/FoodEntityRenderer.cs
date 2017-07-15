using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Entities;

namespace Mindvolving.Visualization.Renderers.Entities
{
    public class FoodEntityRenderer : IRenderable
    {
        public MindvolvingVisualization Visualization { get; set; }
        public Food Food { get; private set; }

        public FoodEntityRenderer(Food food)
        {
            Food = food;
        }

        public void Draw(GameTime gt)
        {
            Visualization.Primitive2DRenderer.FillCircle(Food.Position, 20, Color.Green);
        }

        public void Initialize()
        {

        }
    }
}
