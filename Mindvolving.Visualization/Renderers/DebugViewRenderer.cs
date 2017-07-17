using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FP = FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using MG = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using Mindvolving.Visualization;
using Mindvolving.Visualization.Engine.Controllers;

namespace Mindvolving.Visualization.Renderers
{
    /// <summary>
    /// A debug view shows you what happens inside the physics engine. You can view
    /// bodies, joints, fixtures and more.
    /// </summary>
    public class DebugViewRenderer : DebugViewBase, IDisposable
    {
        //Drawing
        private PrimitiveBatch _primitiveBatch;
        private SpriteBatch _batch;
        private SpriteFont _font;
        private GraphicsDevice _device;
        private FP.Vector2[] _tempVertices = new FP.Vector2[Settings.MaxPolygonVertices];
        private List<StringData> _stringData;

        private MG.Matrix _localProjection;
        private MG.Matrix _localView;

        //Shapes
        public MG.Color DefaultShapeColor = new MG.Color(0.9f, 0.7f, 0.7f);
        public MG.Color InactiveShapeColor = new MG.Color(0.5f, 0.5f, 0.3f);
        public MG.Color KinematicShapeColor = new MG.Color(0.5f, 0.5f, 0.9f);
        public MG.Color SleepingShapeColor = new MG.Color(0.6f, 0.6f, 0.6f);
        public MG.Color StaticShapeColor = new MG.Color(0.5f, 0.9f, 0.5f);
        public MG.Color TextColor = MG.Color.White;

        //Contacts
        private int _pointCount;
        private const int MaxContactPoints = 2048;
        private ContactPoint[] _points = new ContactPoint[MaxContactPoints];

        //Debug panel
        public Vector2 DebugPanelPosition = new Vector2(55, 100);
        private float _max;
        private float _avg;
        private float _min;
        private StringBuilder _debugPanelSb = new StringBuilder();

        //Performance graph
        public bool AdaptiveLimits = true;
        public int ValuesToGraph = 500;
        public float MinimumValue;
        public float MaximumValue = 10;
        private List<float> _graphValues = new List<float>(500);
        public MG.Rectangle PerformancePanelBounds = new MG.Rectangle(330, 100, 200, 100);
        private Vector2[] _background = new Vector2[4];
        public bool Enabled = true;

#if XBOX || WINDOWS_PHONE
        public const int CircleSegments = 16;
#else
        public const int CircleSegments = 32;
#endif

        public DebugViewRenderer(World world)
            : base(world)
        {
            world.ContactManager.PreSolve += PreSolve;

            //Default flags
            AppendFlags(DebugViewFlags.Shape);
            AppendFlags(DebugViewFlags.Controllers);
            AppendFlags(DebugViewFlags.Joint);
        }

        #region IDisposable Members

        public void Dispose()
        {
            World.ContactManager.PreSolve -= PreSolve;
        }

        #endregion

        private void PreSolve(Contact contact, ref Manifold oldManifold)
        {
            if ((Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
            {
                Manifold manifold = contact.Manifold;

                if (manifold.PointCount == 0)
                    return;

                Fixture fixtureA = contact.FixtureA;

                FP.FixedArray2<PointState> state1, state2;
                Collision.GetPointStates(out state1, out state2, ref oldManifold, ref manifold);

                FP.FixedArray2<FP.Vector2> points;
                FP.Vector2 normal;
                contact.GetWorldManifold(out normal, out points);

                for (int i = 0; i < manifold.PointCount && _pointCount < MaxContactPoints; ++i)
                {
                    if (fixtureA == null)
                        _points[i] = new ContactPoint();

                    ContactPoint cp = _points[_pointCount];
                    cp.Position = points[i].ToMGVector2();
                    cp.Normal = normal.ToMGVector2();
                    cp.State = state2[i];
                    _points[_pointCount] = cp;
                    ++_pointCount;
                }
            }
        }

        /// <summary>
        /// Call this to draw shapes and other debug draw data.
        /// </summary>
        private void DrawDebugData()
        {
            if ((Flags & DebugViewFlags.Shape) == DebugViewFlags.Shape)
            {
                foreach (Body b in World.BodyList)
                {
                    FP.Transform xf;
                    b.GetTransform(out xf);
                    foreach (Fixture f in b.FixtureList)
                    {
                        if (b.Enabled == false)
                            DrawShape(f, xf, InactiveShapeColor);
                        else if (b.BodyType == BodyType.Static)
                            DrawShape(f, xf, StaticShapeColor);
                        else if (b.BodyType == BodyType.Kinematic)
                            DrawShape(f, xf, KinematicShapeColor);
                        else if (b.Awake == false)
                            DrawShape(f, xf, SleepingShapeColor);
                        else
                            DrawShape(f, xf, DefaultShapeColor);
                    }

                    DrawString(b.Position.ToMGVector2(), string.Format("F={0:0.000}", b.LinearVelocity.Length())); 
                }
            }

            if ((Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
            {
                const float axisScale = 0.3f;

                for (int i = 0; i < _pointCount; ++i)
                {
                    ContactPoint point = _points[i];

                    if (point.State == PointState.Add)
                        DrawPoint(point.Position, 0.1f, new MG.Color(0.3f, 0.95f, 0.3f));
                    else if (point.State == PointState.Persist)
                        DrawPoint(point.Position, 0.1f, new MG.Color(0.3f, 0.3f, 0.95f));

                    if ((Flags & DebugViewFlags.ContactNormals) == DebugViewFlags.ContactNormals)
                    {
                        Vector2 p1 = point.Position;
                        Vector2 p2 = p1 + axisScale * point.Normal;
                        DrawSegment(p1, p2, new MG.Color(0.4f, 0.9f, 0.4f));
                    }
                }

                _pointCount = 0;
            }

            if ((Flags & DebugViewFlags.PolygonPoints) == DebugViewFlags.PolygonPoints)
            {
                foreach (Body body in World.BodyList)
                {
                    foreach (Fixture f in body.FixtureList)
                    {
                        PolygonShape polygon = f.Shape as PolygonShape;
                        if (polygon != null)
                        {
                            FP.Transform xf;
                            body.GetTransform(out xf);

                            for (int i = 0; i < polygon.Vertices.Count; i++)
                            {
                                Vector2 tmp = FP.MathUtils.Mul(ref xf, polygon.Vertices[i]).ToMGVector2();
                                DrawPoint(tmp, 0.1f, MG.Color.Red);
                            }
                        }
                    }
                }
            }

            if ((Flags & DebugViewFlags.Joint) == DebugViewFlags.Joint)
            {
                foreach (Joint j in World.JointList)
                {
                    DrawJoint(j);
                }
            }

            if ((Flags & DebugViewFlags.AABB) == DebugViewFlags.AABB)
            {
                MG.Color color = new MG.Color(0.9f, 0.3f, 0.9f);
                IBroadPhase bp = World.ContactManager.BroadPhase;

                foreach (Body body in World.BodyList)
                {
                    if (body.Enabled == false)
                        continue;

                    foreach (Fixture f in body.FixtureList)
                    {
                        for (int t = 0; t < f.ProxyCount; ++t)
                        {
                            FixtureProxy proxy = f.Proxies[t];
                            AABB aabb;
                            bp.GetFatAABB(proxy.ProxyId, out aabb);

                            DrawAABB(ref aabb, color);
                        }
                    }
                }
            }

            if ((Flags & DebugViewFlags.CenterOfMass) == DebugViewFlags.CenterOfMass)
            {
                foreach (Body b in World.BodyList)
                {
                    FP.Transform xf;
                    b.GetTransform(out xf);
                    xf.p = b.WorldCenter;
                    DrawTransform(ref xf);
                }
            }

            if ((Flags & DebugViewFlags.Controllers) == DebugViewFlags.Controllers)
            {
                for (int i = 0; i < World.ControllerList.Count; i++)
                {
                    Controller controller = World.ControllerList[i];

                    BuoyancyController buoyancy = controller as BuoyancyController;
                    if (buoyancy != null)
                    {
                        AABB container = buoyancy.Container;
                        DrawAABB(ref container, MG.Color.LightBlue);
                    }

                    SeaCurrentsController seaCurrents = controller as SeaCurrentsController;
                    if (seaCurrents != null)
                    {
                        DrawCircle(seaCurrents.Position, seaCurrents.Radius, MG.Color.LightBlue);

                        FP.Vector2 dir = seaCurrents.Direction;
                        dir.Normalize();

                        DrawArrow(seaCurrents.Position, seaCurrents.Position + dir * seaCurrents.Strength, 5, 8, true, Color.Green);
                    }
                }
            }

            if ((Flags & DebugViewFlags.DebugPanel) == DebugViewFlags.DebugPanel)
                DrawDebugPanel();
        }

        private void DrawPerformanceGraph()
        {
            _graphValues.Add(World.UpdateTime / TimeSpan.TicksPerMillisecond);

            if (_graphValues.Count > ValuesToGraph + 1)
                _graphValues.RemoveAt(0);

            float x = PerformancePanelBounds.X;
            float deltaX = PerformancePanelBounds.Width / (float)ValuesToGraph;
            float yScale = PerformancePanelBounds.Bottom - (float)PerformancePanelBounds.Top;

            // we must have at least 2 values to start rendering
            if (_graphValues.Count > 2)
            {
                _max = _graphValues.Max();
                _avg = _graphValues.Average();
                _min = _graphValues.Min();

                if (AdaptiveLimits)
                {
                    MaximumValue = _max;
                    MinimumValue = 0;
                }

                // start at last value (newest value added)
                // continue until no values are left
                for (int i = _graphValues.Count - 1; i > 0; i--)
                {
                    float y1 = PerformancePanelBounds.Bottom - ((_graphValues[i] / (MaximumValue - MinimumValue)) * yScale);
                    float y2 = PerformancePanelBounds.Bottom - ((_graphValues[i - 1] / (MaximumValue - MinimumValue)) * yScale);

                    Vector2 x1 = new Vector2(MathHelper.Clamp(x, PerformancePanelBounds.Left, PerformancePanelBounds.Right), MathHelper.Clamp(y1, PerformancePanelBounds.Top, PerformancePanelBounds.Bottom));
                    Vector2 x2 = new Vector2(MathHelper.Clamp(x + deltaX, PerformancePanelBounds.Left, PerformancePanelBounds.Right), MathHelper.Clamp(y2, PerformancePanelBounds.Top, PerformancePanelBounds.Bottom));

                    DrawSegment(x1, x2, MG.Color.LightGreen);

                    x += deltaX;
                }
            }

            DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Top, string.Format("Max: {0} ms", _max));
            DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Center.Y - 7, string.Format("Avg: {0} ms", _avg));
            DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Bottom - 15, string.Format("Min: {0} ms", _min));

            //Draw background.
            _background[0] = new Vector2(PerformancePanelBounds.X, PerformancePanelBounds.Y);
            _background[1] = new Vector2(PerformancePanelBounds.X, PerformancePanelBounds.Y + PerformancePanelBounds.Height);
            _background[2] = new Vector2(PerformancePanelBounds.X + PerformancePanelBounds.Width, PerformancePanelBounds.Y + PerformancePanelBounds.Height);
            _background[3] = new Vector2(PerformancePanelBounds.X + PerformancePanelBounds.Width, PerformancePanelBounds.Y);

            DrawSolidPolygon(_background, 4, MG.Color.DarkGray, true);
        }

        private void DrawDebugPanel()
        {
            int fixtureCount = 0;
            for (int i = 0; i < World.BodyList.Count; i++)
            {
                fixtureCount += World.BodyList[i].FixtureList.Count;
            }

            int x = (int)DebugPanelPosition.X;
            int y = (int)DebugPanelPosition.Y;

#if XBOX
            _debugPanelSb = new StringBuilder();
#else
            _debugPanelSb.Clear();
#endif
            _debugPanelSb.AppendLine("Objects:");
            _debugPanelSb.Append("- Bodies: ").AppendLine(World.BodyList.Count.ToString());
            _debugPanelSb.Append("- Fixtures: ").AppendLine(fixtureCount.ToString());
            _debugPanelSb.Append("- Contacts: ").AppendLine(World.ContactList.Count.ToString());
            _debugPanelSb.Append("- Joints: ").AppendLine(World.JointList.Count.ToString());
            _debugPanelSb.Append("- Controllers: ").AppendLine(World.ControllerList.Count.ToString());
            _debugPanelSb.Append("- Proxies: ").AppendLine(World.ProxyCount.ToString());
            DrawString(x, y, _debugPanelSb.ToString());

#if XBOX
            _debugPanelSb = new StringBuilder();
#else
            _debugPanelSb.Clear();
#endif
            _debugPanelSb.AppendLine("Update time:");
            _debugPanelSb.Append("- Body: ").AppendLine(string.Format("{0} ms", World.SolveUpdateTime / TimeSpan.TicksPerMillisecond));
            _debugPanelSb.Append("- Contact: ").AppendLine(string.Format("{0} ms", World.ContactsUpdateTime / TimeSpan.TicksPerMillisecond));
            _debugPanelSb.Append("- CCD: ").AppendLine(string.Format("{0} ms", World.ContinuousPhysicsTime / TimeSpan.TicksPerMillisecond));
            _debugPanelSb.Append("- Joint: ").AppendLine(string.Format("{0} ms", World.Island.JointUpdateTime / TimeSpan.TicksPerMillisecond));
            _debugPanelSb.Append("- Controller: ").AppendLine(string.Format("{0} ms", World.ControllersUpdateTime / TimeSpan.TicksPerMillisecond));
            _debugPanelSb.Append("- Total: ").AppendLine(string.Format("{0} ms", World.UpdateTime / TimeSpan.TicksPerMillisecond));
            DrawString(x + 110, y, _debugPanelSb.ToString());
        }

        public void DrawAABB(ref AABB aabb, MG.Color color)
        {
            Vector2[] verts = new Vector2[4];
            verts[0] = new Vector2(aabb.LowerBound.X, aabb.LowerBound.Y);
            verts[1] = new Vector2(aabb.UpperBound.X, aabb.LowerBound.Y);
            verts[2] = new Vector2(aabb.UpperBound.X, aabb.UpperBound.Y);
            verts[3] = new Vector2(aabb.LowerBound.X, aabb.UpperBound.Y);

            DrawPolygon(verts, 4, color);
        }

        private void DrawJoint(Joint joint)
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
                    DrawPoint(p1, 0.5f, new MG.Color(0.0f, 1.0f, 0.0f));
                    DrawSegment(p1, p2, new MG.Color(0.8f, 0.8f, 0.8f));
                    break;
                case JointType.Revolute:
                    DrawSegment(x1, p1, color);
                    DrawSegment(p1, p2, color);
                    DrawSegment(x2, p2, color);

                    DrawSolidCircle(p2, 0.1f, FP.Vector2.Zero, MG.Color.Red);
                    DrawSolidCircle(p1, 0.1f, FP.Vector2.Zero, MG.Color.Blue);
                    break;
                case JointType.FixedAngle:
                    //Should not draw anything.
                    break;
                case JointType.FixedRevolute:
                    DrawSegment(x1, p1, color);
                    DrawSolidCircle(p1, 0.1f, FP.Vector2.Zero, MG.Color.Pink);
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

        public void DrawShape(Fixture fixture, FP.Transform xf, MG.Color color)
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

        public override void DrawPolygon(FP.Vector2[] vertices, int count, float red, float green, float blue, bool closed = true)
        {
            DrawPolygon(vertices.ToMGVector2(), count, new MG.Color(red, green, blue), closed);
        }

        public void DrawPolygon(Vector2[] vertices, int count, MG.Color color, bool closed = true)
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

        public override void DrawSolidPolygon(FP.Vector2[] vertices, int count, float red, float green, float blue)
        {
            DrawSolidPolygon(vertices.ToMGVector2(), count, new MG.Color(red, green, blue));
        }

        public void DrawSolidPolygon(FP.Vector2[] vertices, int count, MG.Color color, bool outline = true)
        {
            DrawSolidPolygon(vertices.ToMGVector2(), count, color, outline);
        }

        public void DrawSolidPolygon(Vector2[] vertices, int count, MG.Color color, bool outline = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            if (count == 2)
            {
                DrawPolygon(vertices, count, color);
                return;
            }

            MG.Color colorFill = color * (outline ? 0.5f : 1.0f);

            for (int i = 1; i < count - 1; i++)
            {
                _primitiveBatch.AddVertex(vertices[0], colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i], colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(vertices[i + 1], colorFill, PrimitiveType.TriangleList);
            }

            if (outline)
                DrawPolygon(vertices, count, color);
        }

        public override void DrawCircle(FP.Vector2 center, float radius, float red, float green, float blue)
        {
            DrawCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), new MG.Color(red, green, blue));
        }

        public void DrawCircle(FP.Vector2 center, float radius, MG.Color color)
        {
            DrawCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), color);
        }

        public void DrawCircle(Vector2 center, float radius, MG.Color color)
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

        public override void DrawSolidCircle(FP.Vector2 center, float radius, FP.Vector2 axis, float red, float green, float blue)
        {
            DrawSolidCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), axis.ToMGVector2(), new Color(red, green, blue));
        }

        public void DrawSolidCircle(FP.Vector2 center, float radius, FP.Vector2 axis, MG.Color color)
        {
            DrawSolidCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), new Vector2(axis.X, axis.Y), color);
        }

        public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, MG.Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            const double increment = Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            MG.Color colorFill = color * 0.5f;

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

        public override void DrawSegment(FP.Vector2 start, FP.Vector2 end, float red, float green, float blue)
        {
            DrawSegment(start, end, new MG.Color(red, green, blue));
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

        public override void DrawTransform(ref FP.Transform transform)
        {
            const float axisScale = 0.4f;
            FP.Vector2 p1 = transform.p;

            FP.Vector2 p2 = p1 + axisScale * transform.q.GetXAxis();
            DrawSegment(p1, p2, MG.Color.Red);

            p2 = p1 + axisScale * transform.q.GetYAxis();
            DrawSegment(p1, p2, MG.Color.Green);
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

        public void DrawString(int x, int y, string text)
        {
            DrawString(new Vector2(x, y), text);
        }

        public void DrawString(Vector2 position, string text)
        {
            _stringData.Add(new StringData(position, text, TextColor));
        }

        public void DrawArrow(FP.Vector2 start, FP.Vector2 end, float length, float width, bool drawStartIndicator, MG.Color color)
        {
            DrawArrow(start.ToMGVector2(), end.ToMGVector2(), length, width, drawStartIndicator, color);
        }

        public void DrawArrow(Vector2 start, Vector2 end, float length, float width, bool drawStartIndicator, MG.Color color)
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

        public void BeginCustomDraw(MG.Matrix projection, MG.Matrix view)
        {
            BeginCustomDraw(ref projection, ref view);
        }

        public void BeginCustomDraw(ref MG.Matrix projection, ref MG.Matrix view)
        {
            _primitiveBatch.Begin(ref projection, ref view);
        }

        public void EndCustomDraw()
        {
            _primitiveBatch.End();
        }

        public void RenderDebugData(MG.Matrix projection, MG.Matrix view)
        {
            RenderDebugData(ref projection, ref view);
        }

        public void RenderDebugData(ref MG.Matrix projection, ref MG.Matrix view)
        {
            if (!Enabled)
                return;

            //Nothing is enabled - don't draw the debug view.
            if (Flags == 0)
                return;

            _device.RasterizerState = RasterizerState.CullNone;
            _device.DepthStencilState = DepthStencilState.Default;

            _primitiveBatch.Begin(ref projection, ref view);
            DrawDebugData();
            _primitiveBatch.End();

            if ((Flags & DebugViewFlags.PerformanceGraph) == DebugViewFlags.PerformanceGraph)
            {
                _primitiveBatch.Begin(ref _localProjection, ref _localView);
                DrawPerformanceGraph();
                _primitiveBatch.End();
            }

            // begin the sprite batch effect
            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, view);

            // draw any strings we have
            for (int i = 0; i < _stringData.Count; i++)
            {
                _batch.DrawString(_font, _stringData[i].Text, _stringData[i].Position, _stringData[i].Color);
            }

            // end the sprite batch effect
            _batch.End();

            _stringData.Clear();
        }

        public void RenderDebugData(ref MG.Matrix projection)
        {
            if (!Enabled)
                return;

            MG.Matrix view = MG.Matrix.Identity;
            RenderDebugData(ref projection, ref view);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _device = device;
            _batch = new SpriteBatch(_device);
            _primitiveBatch = new PrimitiveBatch(_device, 1000);
            _font = content.Load<SpriteFont>("Font");
            _stringData = new List<StringData>();

            _localProjection = MG.Matrix.CreateOrthographicOffCenter(0f, _device.Viewport.Width, _device.Viewport.Height, 0f, 0f, 1f);
            _localView = MG.Matrix.Identity;
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
            public MG.Color Color;
            public string Text;
            public Vector2 Position;

            public StringData(Vector2 position, string text, MG.Color color)
            {
                Position = position;
                Text = text;
                Color = color;
            }
        }

        #endregion
    }
}