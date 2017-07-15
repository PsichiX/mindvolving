using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Mindvolving.Visualization.Engine.Input
{
    public class InputManager : IVisualizationComponent
    {
        private Dictionary<string, Keys> mappings;

        public MindvolvingVisualization Visualization { get; set; }

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
