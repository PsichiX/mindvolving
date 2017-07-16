using Microsoft.Xna.Framework;
using Mindvolving.Visualization.Engine.Entities;

namespace Mindvolving.Visualization.Renderers.Entities
{
    public class FoodEntityRenderer : Renderer
    {
        public Food Food { get; private set; }

        public FoodEntityRenderer(Food food)
        {
            Food = food;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            Visualization.Primitive2DRenderer.FillCircle(Food.Position, Food.FoodSize, Color.Green);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
