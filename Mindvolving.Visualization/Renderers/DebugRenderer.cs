﻿using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FP = FarseerPhysics.Common;

namespace Mindvolving.Visualization.Renderers
{
    public class DebugRenderer
    {
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _batch;
        private GraphicsDevice _device;
        private FP.Vector2[] _tempVertices = new FP.Vector2[Settings.MaxPolygonVertices];
        private List<StringData> _stringData;

        private Matrix _localProjection;
        private Matrix _localView;
        private Matrix view;

        public SpriteFont Font { get; private set; }


#if XBOX || WINDOWS_PHONE
        public const int CircleSegments = 16;
#else
        public const int CircleSegments = 32;
#endif

        public DebugRenderer()
        {

        }

        public void DrawAABB(AABB aabb, Color color)
        {
            FP.Vector2[] verts = new FP.Vector2[4];
            verts[0] = new FP.Vector2(aabb.LowerBound.X, aabb.LowerBound.Y);
            verts[1] = new FP.Vector2(aabb.UpperBound.X, aabb.LowerBound.Y);
            verts[2] = new FP.Vector2(aabb.UpperBound.X, aabb.UpperBound.Y);
            verts[3] = new FP.Vector2(aabb.LowerBound.X, aabb.UpperBound.Y);

            DrawPolygon(verts.ToMGVector2(), 4, color);
        }

        public void DrawJoint(Joint joint)
        {
            if (!joint.Enabled)
                return;

            Body b1 = joint.BodyA;
            Body b2 = joint.BodyB;
            FP.Transform xf1;
            b1.GetTransform(out xf1);

            FP.Vector2 x2 = FP.Vector2.Zero;

            // WIP David
            if (!joint.IsFixedType())
            {
                FP.Transform xf2;
                b2.GetTransform(out xf2);
                x2 = xf2.p;
            }

            FP.Vector2 p1 = joint.WorldAnchorA;
            FP.Vector2 p2 = joint.WorldAnchorB;
            FP.Vector2 x1 = xf1.p;

            Color color = new Color(0.5f, 0.8f, 0.8f);

            switch (joint.JointType)
            {
                case JointType.Distance:
                    DrawSegment(p1, p2, color);
                    break;
                case JointType.Pulley:
                    PulleyJoint pulley = (PulleyJoint)joint;
                    FP.Vector2 s1 = b1.GetWorldPoint(pulley.LocalAnchorA);
                    FP.Vector2 s2 = b2.GetWorldPoint(pulley.LocalAnchorB);
                    DrawSegment(p1, p2, color);
                    DrawSegment(p1, s1, color);
                    DrawSegment(p2, s2, color);
                    break;
                case JointType.FixedMouse:
                    DrawPoint(p1, 0.5f, new Color(0.0f, 1.0f, 0.0f));
                    DrawSegment(p1, p2, new Color(0.8f, 0.8f, 0.8f));
                    break;
                case JointType.Revolute:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    DrawSegment(x2, p2, color);

                    DrawSolidCircle(p2, 0.1f, FP.Vector2.Zero, Color.Red);
                    DrawSolidCircle(p1, 0.1f, FP.Vector2.Zero, Color.Blue);
                    break;
                case JointType.FixedAngle:
                    //Should not draw anything.
                    break;
                case JointType.FixedRevolute:
                    DrawSegment(x1, p1, color);
                    DrawSolidCircle(p1, 0.1f, FP.Vector2.Zero, Color.Pink);
                    break;
                case JointType.FixedLine:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    break;
                case JointType.FixedDistance:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    break;
                case JointType.FixedPrismatic:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    break;
                case JointType.Gear:
                    DrawSegment(x1, x2, color);
                    break;
                default:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    DrawSegment(x2, p2, color);
                    break;
            }
        }

        public void DrawShape(Fixture fixture, FP.Transform xf, Color color)
        {
            switch (fixture.Shape.ShapeType)
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)fixture.Shape;

                        FP.Vector2 center = FP.MathUtils.Mul(ref xf, circle.Position);
                        float radius = circle.Radius;
                        FP.Vector2 axis = FP.MathUtils.Mul(xf.q, new FP.Vector2(1.0f, 0.0f));

                        DrawSolidCircle(center, radius, axis, color);
                    }
                    break;

                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)fixture.Shape;
                        int vertexCount = poly.Vertices.Count;
                        Debug.Assert(vertexCount <= Settings.MaxPolygonVertices);

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            _tempVertices[i] = FP.MathUtils.Mul(ref xf, poly.Vertices[i]);
                        }

                        DrawSolidPolygon(_tempVertices, vertexCount, color);
                    }
                    break;


                case ShapeType.Edge:
                    {
                        EdgeShape edge = (EdgeShape)fixture.Shape;
                        FP.Vector2 v1 = FP.MathUtils.Mul(ref xf, edge.Vertex1);
                        FP.Vector2 v2 = FP.MathUtils.Mul(ref xf, edge.Vertex2);
                        DrawSegment(v1, v2, color);
                    }
                    break;

                case ShapeType.Chain:
                    {
                        ChainShape chain = (ChainShape)fixture.Shape;

                        for (int i = 0; i < chain.Vertices.Count - 1; ++i)
                        {
                            FP.Vector2 v1 = FP.MathUtils.Mul(ref xf, chain.Vertices[i]);
                            FP.Vector2 v2 = FP.MathUtils.Mul(ref xf, chain.Vertices[i + 1]);
                            DrawSegment(v1, v2, color);
                        }
                    }
                    break;
            }
        }

        public void DrawPolygon(Vector2[] vertices, int count, Color color, bool closed = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            for (int i = 0; i < count - 1; i++)
            {
                _primitiveBatch.AddVertex(vertices[i], color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(vertices[i + 1], color, PrimitiveType.LineList);
            }
            if (closed)
            {
                _primitiveBatch.AddVertex(vertices[count - 1], color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(vertices[0], color, PrimitiveType.LineList);
            }
        }

        public void DrawSolidPolygon(FP.Vector2[] vertices, int count, Color color, bool outline = true)
        {
            DrawSolidPolygon(vertices.ToMGVector2(), count, color, outline);
        }

        public void DrawSolidPolygon(Vector2[] vertices, int count, Color color, bool outline = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            if (count == 2)
            {
                DrawPolygon(vertices, count, color);
                return;
            }

            Color colorFill = color * (outline ? 0.5f : 1.0f);

            for (int i = 1; i < count - 1; i++)
            {
                _primitiveBatch.AddVertex(vertices[0], colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i], colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i + 1], colorFill, PrimitiveType.TriangleList);
            }

            if (outline)
                DrawPolygon(vertices, count, color);
        }

        public void DrawCircle(FP.Vector2 center, float radius, Color color)
        {
            DrawCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), color);
        }

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            for (int i = 0; i < CircleSegments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v1, color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(v2, color, PrimitiveType.LineList);

                theta += increment;
            }
        }

        public void DrawSolidCircle(FP.Vector2 center, float radius, FP.Vector2 axis, Color color)
        {
            DrawSolidCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), new Vector2(axis.X, axis.Y), color);
        }

        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            Color colorFill = color * 0.5f;

            Vector2 v0 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < CircleSegments - 1; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)Math.Cos(theta + increment), (float)Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v0, colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v1, colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v2, colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }

            DrawCircle(center, radius, color);
            DrawSegment(center, center + axis * radius, color);
        }

        public void DrawSegment(FP.Vector2 start, FP.Vector2 end, Color color)
        {
            DrawSegment(start.ToMGVector2(), end.ToMGVector2(), color);
        }

        public void DrawSegment(Vector2 start, Vector2 end, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            _primitiveBatch.AddVertex(start, color, PrimitiveType.LineList);
            _primitiveBatch.AddVertex(end, color, PrimitiveType.LineList);
        }

        public void DrawPoint(FP.Vector2 p, float size, Color color)
        {
            DrawPoint(p.ToMGVector2(), ConvertUnits.ToDisplayUnits(size), color);
        }

        public void DrawPoint(Vector2 p, float size, Color color)
        {
            Vector2[] verts = new Vector2[4];
            float hs = size / 2.0f;
            verts[0] = p + new Vector2(-hs, -hs);
            verts[1] = p + new Vector2(hs, -hs);
            verts[2] = p + new Vector2(hs, hs);
            verts[3] = p + new Vector2(-hs, hs);

            DrawSolidPolygon(verts, 4, color, true);
        }

        public void DrawString(int x, int y, string text, Color color)
        {
            DrawString(new Vector2(x, y), text, color);
        }

        public void DrawString(Vector2 position, string text, Color color)
        {
            _stringData.Add(new StringData(position, text, color));
        }

        public void DrawArrow(FP.Vector2 start, FP.Vector2 end, float length, float width, bool drawStartIndicator, Color color)
        {
            DrawArrow(start.ToMGVector2(), end.ToMGVector2(), length, width, drawStartIndicator, color);
        }

        public void DrawArrow(Vector2 start, Vector2 end, float length, float width, bool drawStartIndicator, Color color)
        {
            // Draw connection segment between start- and end-point
            DrawSegment(start, end, color);

            // Precalculate halfwidth
            float halfWidth = width / 2;

            // Create directional reference
            Vector2 rotation = (start - end);
            rotation.Normalize();

            // Calculate angle of directional vector
            float angle = (float)Math.Atan2(rotation.X, -rotation.Y);
            // Create matrix for rotation
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            // Create translation matrix for end-point
            Matrix endMatrix = Matrix.CreateTranslation(end.X, end.Y, 0);

            // Setup arrow end shape
            Vector2[] verts = new Vector2[3];
            verts[0] = new Vector2(0, 0);
            verts[1] = new Vector2(-halfWidth, -length);
            verts[2] = new Vector2(halfWidth, -length);

            // Rotate end shape
            Vector2.Transform(verts, ref rotMatrix, verts);
            // Translate end shape
            Vector2.Transform(verts, ref endMatrix, verts);

            // Draw arrow end shape
            DrawSolidPolygon(verts, 3, color, false);

            if (drawStartIndicator)
            {
                // Create translation matrix for start
                Matrix startMatrix = Matrix.CreateTranslation(start.X, start.Y, 0);
                // Setup arrow start shape
                Vector2[] baseVerts = new Vector2[4];
                baseVerts[0] = new Vector2(-halfWidth, length / 4);
                baseVerts[1] = new Vector2(halfWidth, length / 4);
                baseVerts[2] = new Vector2(halfWidth, 0);
                baseVerts[3] = new Vector2(-halfWidth, 0);

                // Rotate start shape
                Vector2.Transform(baseVerts, ref rotMatrix, baseVerts);
                // Translate start shape
                Vector2.Transform(baseVerts, ref startMatrix, baseVerts);
                // Draw start shape
                DrawSolidPolygon(baseVerts, 4, color, false);
            }
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(rectangle.Left, rectangle.Top);
            verts[1] = new Vector2(rectangle.Right, rectangle.Top);
            verts[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            verts[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            DrawPolygon(verts, 4, color);
        }

        public void DrawSolidRectangle(Rectangle rectangle, Color color, bool outline = false)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(rectangle.Left, rectangle.Top);
            verts[1] = new Vector2(rectangle.Right, rectangle.Top);
            verts[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            verts[3] = new Vector2(rectangle.Left, rectangle.Bottom);

            DrawSolidPolygon(verts, 4, color, outline);
        }

        public void Begin(Matrix projection)
        {
            Beign(projection, Matrix.Identity);
        }

        public void Beign(Matrix projection, Matrix view)
        {
            this.view = view;

            _primitiveBatch.Begin(projection, view);
        }

        public void End()
        {
            _device.RasterizerState = RasterizerState.CullNone;
            _device.DepthStencilState = DepthStencilState.None;

            _primitiveBatch.End();

            // begin the sprite batch effect
            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, view);

            // draw any strings we have
            for (int i = 0; i < _stringData.Count; i++)
            {
                _batch.DrawString(Font, _stringData[i].Text, _stringData[i].Position, _stringData[i].Color);
            }

            // end the sprite batch effect
            _batch.End();

            _stringData.Clear();
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _device = device;
            _batch = new SpriteBatch(_device);
            _primitiveBatch = new PrimitiveBatch(_device, 1000);
            Font = content.Load<SpriteFont>("Font");
            _stringData = new List<StringData>();

            _localProjection = Matrix.CreateOrthographicOffCenter(0f, _device.Viewport.Width, _device.Viewport.Height, 0f, 0f, 1f);
            _localView = Matrix.Identity;
        }

        #region Nested type: ContactPoint

        private struct ContactPoint
        {
            public Vector2 Normal;
            public Vector2 Position;
            public PointState State;
        }

        #endregion

        #region Nested type: StringData

        private struct StringData
        {
            public Color Color;
            public string Text;
            public Vector2 Position;

            public StringData(Vector2 position, string text, Color color)
            {
                Position = position;
                Text = text;
                Color = color;
            }
        }

        #endregion
    }
}
