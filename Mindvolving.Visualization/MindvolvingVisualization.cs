using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FPCommon = FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;

namespace Mindvolving.Visualization
{
    public class MindvolvingVisualization : Game
    {
        private GraphicsDeviceManager graphics;
        private Organism.Body body;
        private World world;
        private BodyRenderer bodyRenderer;
        private DebugViewRenderer debugViewRenderer;

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
            Primitive2DRenderer = new Primitive2DRenderer()
            {
                Visualization = this
            };

            body = new Organism.Body();
            bodyRenderer = new BodyRenderer(body)
            {
                Visualization = this
            };

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

            //world = new World(new Jitter.Collision.CollisionSystemSAP())
            //{
            //    Gravity = new Jitter.LinearMath.JVector(0, 100, 0)
            //};

            world = new World(new FPCommon.Vector2(0, 100));
            debugViewRenderer = new DebugViewRenderer(world);
            debugViewRenderer.LoadContent(GraphicsDevice, Content);
            
            var rigidBody1 = BodyFactory.CreateBody(world, new FPCommon.Vector2(200, 200), 0, BodyType.Dynamic);
            rigidBody1.CreateFixture(new CircleShape(20, 1));
            var rigidBody2 = BodyFactory.CreateBody(world, new FPCommon.Vector2(120, 20), 0, BodyType.Dynamic);
            rigidBody2.CreateFixture(new CircleShape(30, 1));
            var rigidBody3 = BodyFactory.CreateBody(world, new FPCommon.Vector2(100, 100), 0, BodyType.Static);
            rigidBody3.CreateFixture(new CircleShape(40, 1));
            var rigidBody4 = BodyFactory.CreateBody(world, new FPCommon.Vector2(200, 100), 0, BodyType.Dynamic);
            rigidBody4.CreateFixture(new CircleShape(50, 1));

            a1.RigidBody = rigidBody1;
            a2.RigidBody = rigidBody2;
            a3.RigidBody = rigidBody3;
            a4.RigidBody = rigidBody4;

            world.AddJoint(new DistanceJoint(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position, true) { DampingRatio = 1 });
            world.AddJoint(new DistanceJoint(rigidBody2, rigidBody4, rigidBody2.Position, rigidBody4.Position, true) { DampingRatio = 1 });
            world.AddJoint(new DistanceJoint(rigidBody4, rigidBody3, rigidBody4.Position, rigidBody3.Position, true) { DampingRatio = 1 });

            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position) { Softness = 0f });
            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody2, rigidBody4, rigidBody2.Position, rigidBody4.Position) { Softness = 0f });
            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody4, rigidBody3, rigidBody4.Position, rigidBody3.Position) { Softness = 0f });

            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position) { Softness = 0.01f });
            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody1, rigidBody4, rigidBody1.Position, rigidBody4.Position) { Softness = 0.01f });
            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody2, rigidBody3, rigidBody2.Position, rigidBody3.Position) { Softness = 0.01f });
            //world.AddConstraint(new Jitter.Dynamics.Constraints.PointPointDistance(rigidBody3, rigidBody4, rigidBody3.Position, rigidBody4.Position) { Softness = 0.01f });


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

            world.Step(1 / 60f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix proj = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            Matrix view = Matrix.Identity;

            debugViewRenderer.RenderDebugData(proj, view);

            SpriteBatch.Begin();

            //bodyRenderer.Draw(gameTime);

            //Primitive2DRenderer.DebugPrintColor = Color.Green;

            //foreach (var body in world.BodyList)
            //{
            //    body.DebugDraw(Primitive2DRenderer);
            //}

            //Primitive2DRenderer.DebugPrintColor = Color.Yellow;

            //foreach (var constraint in world.ContactList)
            //{
            //    constraint.DebugDraw(Primitive2DRenderer);
            //}

            SpriteBatch.End();


            //GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
            //                                  new VertexPositionColor[] {
            //                                      new VertexPositionColor(new Vector3(20, 20, 0), Color.Red),
            //                                      new VertexPositionColor(new Vector3(20, 20, 0), Color.Red),
            //                                      new VertexPositionColor(new Vector3(20, 20, 0), Color.Red) },
            //                                  0, 1);

            base.Draw(gameTime);
        }
    }
}
