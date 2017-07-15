using Jitter;
using Jitter.Collision.Shapes;
using Jitter.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;

namespace Mindvolving.Visualization
{
    public class MindvolvingVisualization : Game
    {
        private GraphicsDeviceManager graphics;
        private Organism.Body body;
        private World world;
        private BodyRenderer bodyRenderer;

        public SpriteBatch SpriteBatch { get; private set; }
        public TextureManager Textures { get; private set; }
        public Primitive2DRenderer Primitive2DRenderer { get; private set; }


        public MindvolvingVisualization()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Textures = new TextureManager(this);
            Primitive2DRenderer = new Primitive2DRenderer();
            Primitive2DRenderer.Visualization = this;

            body = new Organism.Body();
            bodyRenderer = new BodyRenderer(body);
            bodyRenderer.Visualization = this;
            bodyRenderer.Initialize();

            var a1 = body.CreateBodyPart();
            var a2 = body.CreateBodyPart();
            var a3 = body.CreateBodyPart();
            var a4 = body.CreateBodyPart();

            a1.AttachBone(a2);
            a2.AttachBone(a4);
            a4.AttachBone(a3);

            a1.AttachMuscle(a2);
            a1.AttachMuscle(a4);
            a2.AttachMuscle(a3);
            a3.AttachMuscle(a4);

            world = new World(new Jitter.Collision.CollisionSystemSAP());
            world.Gravity = new Jitter.LinearMath.JVector(0, 100, 0);

            var rigidBody1 = new RigidBody(new SphereShape(20));
            var rigidBody2 = new RigidBody(new SphereShape(30));
            var rigidBody3 = new RigidBody(new SphereShape(40));
            var rigidBody4 = new RigidBody(new SphereShape(50));

            rigidBody1.Position = new Jitter.LinearMath.JVector(200, 200, 0);
            rigidBody2.Position = new Jitter.LinearMath.JVector(100, 30, 0);
            rigidBody3.Position = new Jitter.LinearMath.JVector(100, 100, 0);
            rigidBody4.Position = new Jitter.LinearMath.JVector(200, 100, 0);

            rigidBody3.IsStatic = true;

            rigidBody1.EnableDebugDraw = true;
            rigidBody2.EnableDebugDraw = true;
            rigidBody3.EnableDebugDraw = true;
            rigidBody4.EnableDebugDraw = true;

            world.AddBody(rigidBody1);
            world.AddBody(rigidBody2);
            world.AddBody(rigidBody3);
            world.AddBody(rigidBody4);

            a1.RigidBody = rigidBody1;
            a2.RigidBody = rigidBody2;
            a3.RigidBody = rigidBody3;
            a4.RigidBody = rigidBody4;

            world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position) { Softness = 0f });
            world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody2, rigidBody4, rigidBody2.Position, rigidBody4.Position) { Softness = 0f });
            world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody4, rigidBody3, rigidBody4.Position, rigidBody3.Position) { Softness = 0f });


            Textures.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            world.Step(1 / 60f, false);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            //bodyRenderer.Draw(gameTime);

            Primitive2DRenderer.DebugPrintColor = Color.Green;

            foreach (RigidBody body in world.RigidBodies)
            {
                body.DebugDraw(Primitive2DRenderer);
            }

            Primitive2DRenderer.DebugPrintColor = Color.Yellow;

            foreach (Jitter.Dynamics.Constraints.Constraint constraint in world.Constraints)
            {
                constraint.DebugDraw(Primitive2DRenderer);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
