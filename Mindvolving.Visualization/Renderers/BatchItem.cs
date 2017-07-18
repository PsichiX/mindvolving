using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mindvolving.Visualization.Renderers
{
    internal class BatchItem
    {
#if DEBUG
        public bool Breakpoint { get; set; }
#endif

        public BatchItemType Type { get; internal set; }
        public Color Color { get; private set; }
        public Vector2 TriangleV1 { get; private set; }
        public Vector2 TriangleV3 { get; private set; }
        public Vector2 TriangleV2 { get; private set; }
        public Texture2D SpriteTexture { get; private set; }
        public Rectangle SpriteDestination { get; private set; }
        public Rectangle? SpriteSource { get; private set; }
        public float SpriteRotation { get; private set; }
        public SpriteEffects SpriteEffects { get; private set; }
        public SpriteFont StringFont { get; private set; }
        public string StringText { get; private set; }

        public Vector2 LineStart { get { return TriangleV1; } set { TriangleV1 = value; } }
        public Vector2 LineEnd { get { return TriangleV2; } set { TriangleV2 = value; } }
        public Vector2 SpritePosition { get { return TriangleV1; } set { TriangleV1 = value; } }
        public Vector2 SpriteScale { get { return TriangleV2; } set { TriangleV2 = value; } }
        public Vector2 SpriteOrigin { get { return TriangleV3; } set { TriangleV3 = value; } }

        internal void SetLine(Vector2 start, Vector2 end, Color color)
        {
#if DEBUG
            Breakpoint = Batch.Breakpoint;
#endif
            Type = BatchItemType.Line;

            LineStart = start;
            LineEnd = end;
            Color = color;
        }

        internal void SetTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
#if DEBUG
            Breakpoint = Batch.Breakpoint;
#endif
            Type = BatchItemType.Triangle;

            TriangleV1 = v1;
            TriangleV2 = v2;
            TriangleV3 = v3;
            Color = color;
        }

        internal void SetSprite(Texture2D texture, Vector2 position, Rectangle? source, Vector2 origin, float rotation, Vector2 scale, Color color, SpriteEffects effects)
        {
#if DEBUG
            Breakpoint = Batch.Breakpoint;
#endif
            Type = BatchItemType.Sprite;

            SpriteTexture = texture;
            SpritePosition = position;
            SpriteSource = source;
            SpriteOrigin = origin;
            SpriteRotation = rotation;
            SpriteScale = scale;
            Color = color;
            SpriteEffects = effects;
        }

        internal void SetSprite(Texture2D texture, Rectangle destination, Rectangle? source, Vector2 origin, float rotation, Color color, SpriteEffects effects)
        {
#if DEBUG
            Breakpoint = Batch.Breakpoint;
#endif
            Type = BatchItemType.SpriteRectangle;

            SpriteTexture = texture;
            SpriteDestination = destination;
            SpriteSource = source;
            SpriteOrigin = origin;
            SpriteRotation = rotation;
            Color = color;
            SpriteEffects = effects;
        }

        internal void SetString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
#if DEBUG
            Breakpoint = Batch.Breakpoint;
#endif
            Type = BatchItemType.String;

            StringFont = font;
            StringText = text;
            SpritePosition = position;
            Color = color;
            SpriteRotation = rotation;
            SpriteOrigin = origin;
            SpriteScale = scale;
            SpriteEffects = effects;
        }
    }
}