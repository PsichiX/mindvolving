using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class Food : Entity
    {
        public Food()
        {
            Renderer = new Renderers.Entities.FoodEntityRenderer(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            PhysicalBody = BodyFactory.CreateBody(World.PhysicalWorld, Position.ToFPVector2(), 0, BodyType.Static);
        }
    }
}
