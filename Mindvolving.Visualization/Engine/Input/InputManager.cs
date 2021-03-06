﻿using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System;

namespace Mindvolving.Visualization.Engine.Input
{
    public class InputManager : IVisualizationComponent, IUpdateable
    {
        private Dictionary<string, Keys> mappings;
        private KeyboardState lastKeyboardState;
        private MouseState lastMouseState;

        public MindvolvingVisualization Visualization { get; set; }
        public Point MousePosition { get { return Mouse.GetState().Position; } }
        public MouseState MouseState { get { return Mouse.GetState(); } }
        public KeyboardState KeyboardState { get { return Keyboard.GetState(); } }

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;
        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;

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

        public void Update(GameTime gt)
        {
            KeyboardState keyboardState = KeyboardState;

            Keys[] last = lastKeyboardState.GetPressedKeys();
            Keys[] current = keyboardState.GetPressedKeys();

            foreach(Keys key in current.Except(last))
            {
                KeyDown?.Invoke(this, new KeyEventArgs(key, keyboardState));
            }

            foreach (Keys key in last.Except(current))
            {
                KeyUp?.Invoke(this, new KeyEventArgs(key, keyboardState));
            }

            lastKeyboardState = KeyboardState;

            MouseState mouseState = MouseState;

            if (lastMouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                MouseDown?.Invoke(this, new MouseEventArgs(MouseButtons.Left, mouseState));

            if (lastMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                MouseUp?.Invoke(this, new MouseEventArgs(MouseButtons.Left, mouseState));

            lastMouseState = mouseState;
        }
    }
}
