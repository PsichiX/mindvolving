using FarseerPhysics.Controllers;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
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
			foreach (Body body in World.BodyList)
			{
				var distance = (body.Position - Position).Length();
				var radius = Radius;

				if (body.UserData is OrganUserData)
					radius += (body.UserData as OrganUserData).Organ.Radius;

                if (distance <= radius)
                {
					var factor = distance / radius;
					body.ApplyForce(Direction * Strength * (1.0f - factor * factor));
				}
			}
		}
	}
}
