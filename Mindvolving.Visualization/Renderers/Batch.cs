using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Mindvolving.Visualization.Renderers
{
    public class Batch
    {
#if DEBUG
        public static bool Breakpoint { get; set; }
        public int FlushCount { get; private set; }
        public int LastFlushCount { get; private set; }
#endif

        private GraphicsDevice device;
        private PrimitiveBatch primitiveBatch;
        private SpriteBatch spriteBatch;

        private BatchMode batchMode;
        private int batchItemsCount;
        private List<BatchItem> batchItems;

        private Matrix view;

        public Batch(GraphicsDevice device)
        {
            this.device = device;
            batchItems = new List<BatchItem>();
            primitiveBatch = new PrimitiveBatch(device);
            spriteBatch = new SpriteBatch(device);
            batchItemsCount = 0;
        }

        public void Begin(BatchMode batchMode, Matrix projection, Matrix view)
        {
            this.batchMode = batchMode;
            this.view = view;

            if (batchMode == BatchMode.FlatPrimitivesTrianglesLines)
                primitiveBatch.Mode = PrimitiveBatchMode.TrianglesLinesOrder;
            else if (batchMode == BatchMode.FlatPrimitivesLinesTriangles)
                primitiveBatch.Mode = PrimitiveBatchMode.LinesTrianglesOrder;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, view);
            primitiveBatch.Begin(projection, view);

#if DEBUG
            LastFlushCount = FlushCount;
            FlushCount = 0;
#endif
        }

        public void End()
        {
            Flush();

            primitiveBatch.End();
            spriteBatch.End();
        }

        #region Line primitives
        public void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            BatchItem item = CreateBatchItem();
            item.SetLine(start, end, color);
        }

        public void DrawPolygon(Vector2[] vertices, Color color, bool closed = true)
        {
            //if (!Batch.IsReady())
            //    throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            for (int i = 0; i < vertices.Length - 1; i++)
                DrawLine(vertices[i], vertices[i + 1], color);

            if (closed)
                DrawLine(vertices[vertices.Length - 1], vertices[0], color);
        }

        public void DrawCircle(Vector2 center, float radius, Color color, int circleSegments = 32)
        {
            //if (!Batch.IsReady())
            //    throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            double increment = Math.PI * 2.0 / circleSegments;
            double theta = 0.0;

            for (int i = 0; i < circleSegments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                DrawLine(v1, v2, color);

                theta += increment;
            }
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(rectangle.Left, rectangle.Top);
            verts[1] = new Vector2(rectangle.Right, rectangle.Top);
            verts[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            verts[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            DrawPolygon(verts, color);
        }
        #endregion

        #region Triagnle primitives
        public void FillPoint(Vector2 p, float size, Color color)
        {
            Vector2[] verts = new Vector2[4];
            float hs = size / 2.0f;
            verts[0] = p + new Vector2(-hs, -hs);
            verts[1] = p + new Vector2(hs, -hs);
            verts[2] = p + new Vector2(hs, hs);
            verts[3] = p + new Vector2(-hs, hs);

            FillPolygon(verts, color);
        }

        public void FillTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            BatchItem item = CreateBatchItem();
            item.SetTriangle(v1, v2, v3, color);
        }

        public void FillPolygon(Vector2[] vertices, Color color)
        {
            //if (!Batch.IsReady())
            //    throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            if (vertices.Length == 2)
            {
                DrawPolygon(vertices, color);
                return;
            }

            for (int i = 1; i < vertices.Length - 1; i++)
                FillTriangle(vertices[0], vertices[i], vertices[i + 1], color);
        }

        public void FillCircle(Vector2 center, float radius, Vector2 axis, Color color, int circleSegments = 32)
        {
            //if (!Batch.IsReady())
            //    throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            double increment = Math.PI * 2.0 / circleSegments;
            double theta = 0.0;

            Color colorFill = color * 0.5f;

            Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < circleSegments - 1; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                FillTriangle(v0, v1, v2, color);

                theta += increment;
            }
        }

        public void FillRectangle(Rectangle rectangle, Color color)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(rectangle.Left, rectangle.Top);
            verts[1] = new Vector2(rectangle.Right, rectangle.Top);
            verts[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            verts[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            FillPolygon(verts, color);
        }
        #endregion

        #region Sprites
        public void DrawSprite(Texture2D texture, Vector2 position, Rectangle? source, Vector2 origin, float rotation, Vector2 scale, Color color, SpriteEffects effects)
        {
            BatchItem item = CreateBatchItem();
            item.SetSprite(texture, position, source, origin, rotation, scale, color, effects);
        }

        public void DrawSprite(Texture2D texture, Rectangle destination, Rectangle? source, Vector2 origin, float rotation, Color color, SpriteEffects effects)
        {
            BatchItem item = CreateBatchItem();
            item.SetSprite(texture, destination, source, origin, rotation, color, effects);;
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
            BatchItem item = CreateBatchItem();
            item.SetString(font, text, position, color, rotation, origin, scale, effects);
        }
        #endregion

        public void Flush()
        {
            BatchItemType type = (BatchItemType)(-1);

            for (int i = 0; i < batchItemsCount; i++)
            {
                BatchItem item = batchItems[i];

#if DEBUG
                if (item.Breakpoint)
                    System.Diagnostics.Debugger.Break();
#endif

                FlushAccordingToTypes(type, item.Type);

                type = item.Type;

                if (type == BatchItemType.Line)
                {
                    primitiveBatch.AddVertex(item.LineStart, item.Color, PrimitiveType.LineList);
                    primitiveBatch.AddVertex(item.LineEnd, item.Color, PrimitiveType.LineList);
                }
                else if (type == BatchItemType.Triangle)
                {
                    primitiveBatch.AddVertex(item.TriangleV1, item.Color, PrimitiveType.TriangleList);
                    primitiveBatch.AddVertex(item.TriangleV2, item.Color, PrimitiveType.TriangleList);
                    primitiveBatch.AddVertex(item.TriangleV3, item.Color, PrimitiveType.TriangleList);
                }
                else if (type == BatchItemType.SpriteRectangle)
                {
                    spriteBatch.Draw(item.SpriteTexture, item.SpriteDestination, item.SpriteSource, item.Color, item.SpriteRotation, item.SpriteOrigin, item.SpriteEffects, 0f);
                }
                else if (type == BatchItemType.Sprite)
                {
                    spriteBatch.Draw(item.SpriteTexture, item.SpritePosition, item.SpriteSource, item.Color, item.SpriteRotation, item.SpriteOrigin, item.SpriteScale, item.SpriteEffects, 0f);
                }
                else if (type == BatchItemType.String)
                {
                    spriteBatch.DrawString(item.StringFont, item.StringText, item.SpritePosition, item.Color, item.SpriteRotation, item.SpriteOrigin, item.SpriteScale, item.SpriteEffects, 0f);
                }
            }

            FlushPrimitiveBatch();
            FlushSpriteBatch();

            batchItemsCount = 0;
        }

        private BatchItem CreateBatchItem()
        {
            if (batchItemsCount < batchItems.Count)
                return batchItems[batchItemsCount++];
            
            BatchItem item = new BatchItem();
            batchItems.Add(item);
            batchItemsCount++;

            return item;
        }

        private void FlushSpriteBatch()
        {
#if DEBUG
            FlushCount++;
#endif
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, view);
        }

        private void FlushPrimitiveBatch()
        {
#if DEBUG
            FlushCount++;
#endif
            primitiveBatch.Flush();
        }

        private void FlushAccordingToTypes(BatchItemType oldType, BatchItemType newType)
        {
            if (oldType == (BatchItemType)(-1))
                return;

            if (batchMode == BatchMode.Full)
            {
                if ((oldType & BatchItemType.Primitive) != 0)
                {
                    if ((newType & BatchItemType.Sprite) != 0) // moving from primitives to sprites
                        FlushPrimitiveBatch();
                    else if (oldType != newType) // moving from line to triangles or vice versa
                        FlushPrimitiveBatch();
                }

                if ((oldType & BatchItemType.Sprite) != 0)
                {
                    if ((newType & BatchItemType.Primitive) != 0) // moving from sprites to primitives
                        FlushSpriteBatch();
                }
            }
            else if (batchMode == BatchMode.FlatPrimitivesLinesTriangles || batchMode == BatchMode.FlatPrimitivesTrianglesLines)
            {
                if ((oldType & BatchItemType.Primitive) != 0)
                {
                    if ((newType & BatchItemType.Sprite) != 0) // moving from primitives to sprites
                        FlushPrimitiveBatch();
                }

                if ((oldType & BatchItemType.Sprite) != 0)
                {
                    if ((newType & BatchItemType.Primitive) != 0) // moving from sprites to primitives
                        FlushSpriteBatch();
                }
            }
        }
    }
}
