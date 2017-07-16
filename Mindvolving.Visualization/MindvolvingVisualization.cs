using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FPCommon = FarseerPhysics.Common;
using Physics = FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using Mindvolving.Visualization.Engine;
using Mindvolving.Visualization.Engine.Input;
using Mindvolving.Visualization.Screens;
using System;
using Mindvolving.Visualization.Engine.Entities;

namespace Mindvolving.Visualization
{
    public class MindvolvingVisualization : Game
    {
        private GraphicsDeviceManager graphics;
        private Organism.Body body;
        private BodyRenderer bodyRenderer;
        private Screen currentScreen;

        public SpriteBatch SpriteBatch { get; private set; }
        public TextureManager Textures { get; private set; }
        public Primitive2DRenderer Primitive2DRenderer { get; private set; }
        public Engine.World World { get; private set; }
        public Camera Camera { get; private set; }
        public InputManager InputManager { get; private set; }

        public MindvolvingVisualization()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void ChangeScreen<T>() where T : Screen, new()
        {
            Screen screen = new T();
            screen.Visualization = this;
            screen.Initialize();

            if (currentScreen != null)
                currentScreen.Unload();

            currentScreen = screen;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Camera = new Camera(GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            InputManager = CreateVisualizationComponent<InputManager>();
            Textures = CreateVisualizationComponent<TextureManager>();
            Primitive2DRenderer = CreateVisualizationComponent<Primitive2DRenderer>();
            World = CreateVisualizationComponent<Engine.World>();

            PreparePhisycsTestScene();

            Textures.LoadContent();

            ChangeScreen<VisualizationScreen>();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            currentScreen.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void PreparePhisycsTestScene()
        {
            var organism = World.CreateEntity<OrganismEntity>();

            body = new Organism.Body();
            bodyRenderer = CreateVisualizationComponent<BodyRenderer>();
            organism.OrganicBody = body;

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

            // Physics simulation
            var rigidBody1 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(200, 200), 0, BodyType.Dynamic);
            rigidBody1.CreateFixture(new CircleShape(20, 1));
            var rigidBody2 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(120, 20), 0, BodyType.Dynamic);
            rigidBody2.CreateFixture(new CircleShape(30, 1));
            var rigidBody3 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(100, 100), 0, BodyType.Static);
            rigidBody3.CreateFixture(new CircleShape(40, 1));
            var rigidBody4 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(200, 100), 0, BodyType.Dynamic);
            rigidBody4.CreateFixture(new CircleShape(50, 1));

            a1.PhysicalBody = rigidBody1;
            a2.PhysicalBody = rigidBody2;
            a3.PhysicalBody = rigidBody3;
            a4.PhysicalBody = rigidBody4;

            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position, true) { DampingRatio = 1 });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody4, rigidBody2.Position, rigidBody4.Position, true) { DampingRatio = 1 });
            World.PhysicalWorld.AddJoint(new DistanceJoint(rigidBody4, rigidBody3, rigidBody4.Position, rigidBody3.Position, true) { DampingRatio = 1, Frequency = 0.9f, Length = 200 });

            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody4, rigidBody1.Position, rigidBody4.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody3, rigidBody2.Position, rigidBody3.Position, true) { DampingRatio = 0f, Length = 400 });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody3, rigidBody2.Position, rigidBody3.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody3, rigidBody4, rigidBody3.Position, rigidBody4.Position, true) { DampingRatio = 0f });

        }

        public T CreateVisualizationComponent<T>() where T : IVisualizationComponent, new()
        {
            T component = new T();
            component.Visualization = this;
            component.Initialize();

            return component;
        }

        public T CreateVisualizationComponent<T>(Func<T> creator) where T : IVisualizationComponent
        {
            T component = creator();
            component.Visualization = this;
            component.Initialize();

            return component;
        }

        public T CreateRenderer<T>() where T : Renderer, new()
        {
            return CreateVisualizationComponent<T>();
        }

        public T CreateRenderer<T>(Func<T> creator) where T : Renderer
        {
            return CreateVisualizationComponent(creator);
        }
    }
}
