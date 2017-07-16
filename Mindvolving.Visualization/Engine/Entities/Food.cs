using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using Mindvolving.Visualization.Renderers.Entities;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class Food : Entity
    {
        public const int FoodSize = 15;

        public Food()
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Renderer = World.Visualization.CreateRenderer(() => new FoodEntityRenderer(this));
            PhysicalBody = BodyFactory.CreateBody(World.PhysicalWorld, Position.ToFPVector2(), 0, FarseerPhysics.Dynamics.BodyType.Static);
            PhysicalBody.CreateFixture(new CircleShape(FoodSize, 1));
        }
    }
}
