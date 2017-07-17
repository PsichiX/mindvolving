using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using System.Linq;
using Mindvolving.Organisms;

namespace Mindvolving.Visualization.Renderers.Organisms
{
    public class OrganRenderer : Renderer
    {
        public Organ Organ { get; set; }

        public OrganRenderer()
        {
            Organ = null;
        }

        public OrganRenderer(Organ organ)
        {
            Organ = organ;
        }

        public override void Draw(GameTime gt)
        {
            base.Draw(gt);

            CircleShape circle = Organ.Body.FixtureList.Select(p => p.Shape).Where(p => p is CircleShape).Cast<CircleShape>().FirstOrDefault();

            if (circle != null)
            {
                Visualization.Primitive2DRenderer.FillCircle(Organ.Body.Position.ToMGVector2(), (int)FarseerPhysics.ConvertUnits.ToDisplayUnits(circle.Radius), Color.Black, new Vector2(0.5f, 0.5f));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}