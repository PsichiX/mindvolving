using System;
using FarseerPhysics.Controllers;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Mindvolving.Organisms.Physics;

namespace Mindvolving.Visualization.Engine.Controllers
{
    public class SeaCurrentsController : Controller
    {
        public float Radius { get; set; }

        public Vector2 Direction { get; set; }

        public float Strength { get; set; }

        public Vector2 Position { get; set; }

        public SeaCurrentsController() 
            : base((ControllerType)(1 << 4))
        {
            
        }

        public override void Update(float dt)
        {
            foreach(Body body in World.BodyList)
            {
                if (body.UserData is OrganUserData)
                {
                    OrganUserData organData = (OrganUserData)body.UserData;

                    ApplyForce(organData.Organ.Body);

                    continue;
                }

                if (body.FixtureList.Count == 1 && body.FixtureList[0].Shape is CircleShape)
                {
                    ApplyForce(body);
                }
            }
        }

        private void ApplyForce(Body body)
        {
            CircleShape circle = (CircleShape)body.FixtureList[0].Shape;

            if ((Position - body.Position).Length() < Radius + circle.Radius)
            {
                float distanceMultiplier = 1 - (Position - body.Position).Length() / (Radius + circle.Radius);
                body.ApplyForce(Direction * Strength * distanceMultiplier, body.Position);
            }
        }
    }
}
