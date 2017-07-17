using FarseerPhysics.Dynamics;
using Physics = FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mindvolving.Visualization.Renderers;
using Mindvolving.Visualization.Engine;
using Mindvolving.Visualization.Engine.Input;
using Mindvolving.Visualization.Screens;
using System;
using Mindvolving.Visualization.Engine.Entities;
using Mindvolving.Visualization.Engine.Enviroment;

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

        public static Random Random { get; private set; } = new Random();

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

            Physics.ConvertUnits.SetDisplayUnitToSimUnitRatio(50);

            InputManager = CreateVisualizationComponent<InputManager>();
            InputManager.KeyUp += InputManager_KeyUp;

            base.Initialize();
        }

        private void InputManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Keys.F12)
            {
                if (currentScreen is VisualizationScreen)
                    ChangeScreen<DebugScreen>();
                else
                    ChangeScreen<VisualizationScreen>();
            }
        }

        protected override void LoadContent()
        {
            Camera = new Camera(GraphicsDevice);
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Textures = CreateVisualizationComponent<TextureManager>();
            Primitive2DRenderer = CreateVisualizationComponent<Primitive2DRenderer>();
            World = CreateVisualizationComponent<Engine.World>();

            PreparePhysicsTestScene();

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

            InputManager.Update(gameTime);

            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(30, 50, 90));

            currentScreen.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void PreparePhysicsTestScene()
        {
            // Organism building
            var dna = new Organisms.DNA();
            dna.Root = new Organisms.DNA.Organ() { Radius = 1 };
            var left = new Organisms.DNA.Organ() { RadialOrientation = -90, Radius = 1 };
            var right = new Organisms.DNA.Organ() { RadialOrientation = 90, Radius = 1 };
            dna.Root.Children.Add(left);
            dna.Root.Children.Add(right);
            dna.Muscles.Add(new Organisms.DNA.Muscle() { From = dna.Root, To = left, ContractionFactor = 0.5f });
            dna.Muscles.Add(new Organisms.DNA.Muscle() { From = dna.Root, To = right, ContractionFactor = 0.5f });
            
            World.CreateOrganism(dna);

            World.PhysicalWorld.AddController(new Engine.Controllers.SeaCurrentsController()
            {
                Direction = new Physics.Common.Vector2(1, 0),
                Position = new Physics.Common.Vector2(0, 0),
                Radius = 4,
                Strength = 2
            });

            World.PhysicalWorld.AddController(new Engine.Controllers.SeaCurrentsController()
            {
                Direction = new Physics.Common.Vector2(-1, 0),
                Position = new Physics.Common.Vector2(6, 0),
                Radius = 4,
                Strength = 2
            });

            var food = World.CreateEntity<Food>();
            food.Position = new Vector2(400, 200);

            for (int i = 0; i < 30; i++)
            {
                var bubble = World.CreateDecal<Bubble>();
                bubble.Position = new Vector2(Random.Next(0, 700), Random.Next(0, 500));
                bubble.Size = (float)(Random.NextDouble() * 0.8 + 0.2);
            }
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
