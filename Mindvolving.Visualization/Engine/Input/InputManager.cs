using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace Mindvolving.Visualization.Engine.Input
{
    public class InputManager : IVisualizationComponent
    {
        private Dictionary<string, Keys> mappings;

        public MindvolvingVisualization Visualization { get; set; }
        public Point MousePosition { get { return Mouse.GetState().Position; } }
        public MouseState MouseState { get { return Mouse.GetState(); } }
        public KeyboardState KeyboardState { get { return Keyboard.GetState(); } }

        public InputManager()
        {
            mappings = new Dictionary<string, Keys>();
        }

        public void Initialize()
        {
            mappings["debug-camera-right"] = Keys.Right;
            mappings["debug-camera-left"] = Keys.Left;
            mappings["debug-camera-up"] = Keys.Up;
            mappings["debug-camera-down"] = Keys.Down;
        }

        public bool IsKeyDown(string action)
        {
            return Keyboard.GetState().IsKeyDown(mappings[action]);
        }

        public bool IsKeyUp(string action)
        {
            return Keyboard.GetState().IsKeyUp(mappings[action]);
        }

    }
}
