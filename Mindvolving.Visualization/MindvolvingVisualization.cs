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

            PreparePhysicsTestScene();

            Textures.LoadContent();

            ChangeScreen<DebugScreen>();
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

        private void PreparePhysicsTestScene()
        {
            // Physics simulation
            var rigidBody1 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(200, 200), 0, BodyType.Dynamic);
            rigidBody1.CreateFixture(new CircleShape(20, 1));
            var rigidBody2 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(120, 20), 0, BodyType.Dynamic);
            rigidBody2.CreateFixture(new CircleShape(30, 1));
            var rigidBody3 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(100, 100), 0, BodyType.Static);
            rigidBody3.CreateFixture(new CircleShape(40, 1));
            var rigidBody4 = BodyFactory.CreateBody(World.PhysicalWorld, new FPCommon.Vector2(200, 100), 0, BodyType.Dynamic);
            rigidBody4.CreateFixture(new CircleShape(50, 1));

            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position, true) { DampingRatio = 1 });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody4, rigidBody2.Position, rigidBody4.Position, true) { DampingRatio = 1 });
            World.PhysicalWorld.AddJoint(new DistanceJoint(rigidBody4, rigidBody3, rigidBody4.Position, rigidBody3.Position, true) { DampingRatio = 1, Frequency = 0.9f, Length = 200 });

            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody2, rigidBody1.Position, rigidBody2.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody1, rigidBody4, rigidBody1.Position, rigidBody4.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody3, rigidBody2.Position, rigidBody3.Position, true) { DampingRatio = 0f, Length = 400 });
            //world.AddJoint(new DistanceJoint(rigidBody2, rigidBody3, rigidBody2.Position, rigidBody3.Position, true) { DampingRatio = 0f });
            //world.AddJoint(new DistanceJoint(rigidBody3, rigidBody4, rigidBody3.Position, rigidBody4.Position, true) { DampingRatio = 0f });

            // Organism building
            OrganismEntityBuilder organismBuilder = new OrganismEntityBuilder();

            organismBuilder.BeginBuilding(World);

            organismBuilder.AddBodyPart(rigidBody1);
            organismBuilder.AddBodyPart(rigidBody2);
            organismBuilder.AddBodyPart(rigidBody3);
            organismBuilder.AddBodyPart(rigidBody4);

            organismBuilder.AddBone(0, 1, null);
            organismBuilder.AddBone(1, 3, null);
            organismBuilder.AddBone(3, 2, null);

            organismBuilder.AddMuscle(0, 1, null, 50, 100);
            organismBuilder.AddMuscle(0, 3, null, 50, 100);
            organismBuilder.AddMuscle(1, 2, null, 50, 100);
            organismBuilder.AddMuscle(2, 3, null, 50, 100);

            organismBuilder.Build();
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
