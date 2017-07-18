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
        private DebugRenderer debugRenderer;
        private GraphicsDevice device;

        //Shapes
        public Color DefaultShapeColor = new Color(0.9f, 0.7f, 0.7f);
        public Color InactiveShapeColor = new Color(0.5f, 0.5f, 0.3f);
        public Color KinematicShapeColor = new Color(0.5f, 0.5f, 0.9f);
        public Color SleepingShapeColor = new Color(0.6f, 0.6f, 0.6f);
        public Color StaticShapeColor = new Color(0.5f, 0.9f, 0.5f);
        public Color TextColor = Color.White;

        //Contacts
        private int _pointCount;
        private const int MaxContactPoints = 2048;
        private ContactPoint[] _points = new ContactPoint[MaxContactPoints];

        //Debug panel
        private StringBuilder dPStringBuilder = new StringBuilder();

        //Performance graph
        private float _max;
        private float _avg;
        private float _min;
        public bool AdaptiveLimits = true;
        public int ValuesToGraph = 500;
        public float MinimumValue;
        public float MaximumValue = 10;
        private List<float> _graphValues = new List<float>(500);
        public Rectangle PerformancePanelBounds = new Rectangle(330, 100, 200, 100);
        private Vector2[] _background = new Vector2[4];
        public bool Enabled = true;

        public Body SelectedBody { get; set; }

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

            debugRenderer = new Renderers.DebugRenderer();
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
            if ((Flags & DebugViewFlags.Controllers) == DebugViewFlags.Controllers)
            {
                for (int i = 0; i < World.ControllerList.Count; i++)
                {
                    Controller controller = World.ControllerList[i];

                    BuoyancyController buoyancy = controller as BuoyancyController;
                    if (buoyancy != null)
                    {
                        AABB container = buoyancy.Container;
                        debugRenderer.DrawAABB(container, Color.LightBlue);
                    }

                    SeaCurrentsController seaCurrents = controller as SeaCurrentsController;
                    if (seaCurrents != null)
                    {
                        debugRenderer.DrawCircle(seaCurrents.Position, seaCurrents.Radius, Color.LightBlue);

                        FP.Vector2 dir = seaCurrents.Direction;
                        dir.Normalize();

                        debugRenderer.DrawArrow(seaCurrents.Position, seaCurrents.Position + dir * seaCurrents.Strength, 5, 8, true, Color.Green);
                    }
                }
            }

            //debugRenderer.batch.Flush();

            if ((Flags & DebugViewFlags.Shape) == DebugViewFlags.Shape)
            {
                foreach (Body b in World.BodyList)
                {
                    FP.Transform xf;
                    b.GetTransform(out xf);
                    foreach (Fixture f in b.FixtureList)
                    {
                        if (b.Enabled == false)
                            debugRenderer.DrawShape(f, xf, InactiveShapeColor);
                        else if (b.BodyType == BodyType.Static)
                            debugRenderer.DrawShape(f, xf, StaticShapeColor);
                        else if (b.BodyType == BodyType.Kinematic)
                            debugRenderer.DrawShape(f, xf, KinematicShapeColor);
                        else if (b.Awake == false)
                            debugRenderer.DrawShape(f, xf, SleepingShapeColor);
                        else
                            debugRenderer.DrawShape(f, xf, DefaultShapeColor);

                        if (f.Body == SelectedBody)
                        {
                            Color color = new Color(0.9f, 0.3f, 0.3f);
                            IBroadPhase bp = World.ContactManager.BroadPhase;

                            for (int t = 0; t < f.ProxyCount; ++t)
                            {
                                FixtureProxy proxy = f.Proxies[t];
                                AABB aabb;
                                bp.GetFatAABB(proxy.ProxyId, out aabb);

                                debugRenderer.DrawAABB(aabb, color);
                            }
                        }
                    }
                }
            }

            //debugRenderer.batch.Flush();

            if ((Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
            {
                const float axisScale = 0.3f;

                for (int i = 0; i < _pointCount; ++i)
                {
                    ContactPoint point = _points[i];

                    if (point.State == PointState.Add)
                        debugRenderer.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.95f, 0.3f));
                    else if (point.State == PointState.Persist)
                        debugRenderer.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.3f, 0.95f));

                    if ((Flags & DebugViewFlags.ContactNormals) == DebugViewFlags.ContactNormals)
                    {
                        Vector2 p1 = point.Position;
                        Vector2 p2 = p1 + axisScale * point.Normal;
                        debugRenderer.DrawSegment(p1, p2, new Color(0.4f, 0.9f, 0.4f));
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
                                debugRenderer.DrawPoint(tmp, 0.1f, Color.Red);
                            }
                        }
                    }
                }
            }

            if ((Flags & DebugViewFlags.Joint) == DebugViewFlags.Joint)
            {
                foreach (Joint j in World.JointList)
                {
                    debugRenderer.DrawJoint(j);
                }
            }

            if ((Flags & DebugViewFlags.AABB) == DebugViewFlags.AABB)
            {
                Color color = new Color(0.9f, 0.3f, 0.9f);
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

                            debugRenderer.DrawAABB(aabb, color);
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

            //debugRenderer.batch.Flush();

            if (SelectedBody != null)
            {
                debugRenderer.DrawArrow(SelectedBody.Position, SelectedBody.Position + SelectedBody.LinearVelocity, 4, 8, false, Color.DarkRed);
            }
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

                    debugRenderer.DrawSegment(x1, x2, Color.LightGreen);

                    x += deltaX;
                }
            }

            debugRenderer.DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Top, string.Format("Max: {0} ms", _max), TextColor);
            debugRenderer.DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Center.Y - 7, string.Format("Avg: {0} ms", _avg), TextColor);
            debugRenderer.DrawString(PerformancePanelBounds.Right + 10, PerformancePanelBounds.Bottom - 15, string.Format("Min: {0} ms", _min), TextColor);

            //Draw background.
            _background[0] = new Vector2(PerformancePanelBounds.X, PerformancePanelBounds.Y);
            _background[1] = new Vector2(PerformancePanelBounds.X, PerformancePanelBounds.Y + PerformancePanelBounds.Height);
            _background[2] = new Vector2(PerformancePanelBounds.X + PerformancePanelBounds.Width, PerformancePanelBounds.Y + PerformancePanelBounds.Height);
            _background[3] = new Vector2(PerformancePanelBounds.X + PerformancePanelBounds.Width, PerformancePanelBounds.Y);

            debugRenderer.DrawSolidPolygon(_background, 4, Color.DarkGray, true);
        }

        private void DrawDebugPanel()
        {
            int fixtureCount = 0;
            for (int i = 0; i < World.BodyList.Count; i++)
            {
                fixtureCount += World.BodyList[i].FixtureList.Count;
            }

            int x = 10;
            int y = 10;

            debugRenderer.DrawSolidRectangle(new Rectangle(0, 0, 300, device.Viewport.Height), Color.Black);
            debugRenderer.DrawSegment(new Vector2(300, 0), new Vector2(300, device.Viewport.Height), Color.Gray);
            debugRenderer.DrawSegment(new Vector2(299, 0), new Vector2(299, device.Viewport.Height), Color.Gray);
#if XBOX
            _debugPanelSb = new StringBuilder();
#else
            dPStringBuilder.Clear();
#endif
            dPStringBuilder.AppendLine("Objects:");
            dPStringBuilder.Append("- Bodies: ").AppendLine(World.BodyList.Count.ToString());
            dPStringBuilder.Append("- Fixtures: ").AppendLine(fixtureCount.ToString());
            dPStringBuilder.Append("- Contacts: ").AppendLine(World.ContactList.Count.ToString());
            dPStringBuilder.Append("- Joints: ").AppendLine(World.JointList.Count.ToString());
            dPStringBuilder.Append("- Controllers: ").AppendLine(World.ControllerList.Count.ToString());
            dPStringBuilder.Append("- Proxies: ").AppendLine(World.ProxyCount.ToString());
            debugRenderer.DrawString(x, y, dPStringBuilder.ToString(), TextColor);

#if XBOX
            _debugPanelSb = new StringBuilder();
#else
            dPStringBuilder.Clear();
#endif
            dPStringBuilder.AppendLine("Update time:");
            dPStringBuilder.Append("- Body: ").AppendLine(string.Format("{0} ms", World.SolveUpdateTime / TimeSpan.TicksPerMillisecond));
            dPStringBuilder.Append("- Contact: ").AppendLine(string.Format("{0} ms", World.ContactsUpdateTime / TimeSpan.TicksPerMillisecond));
            dPStringBuilder.Append("- CCD: ").AppendLine(string.Format("{0} ms", World.ContinuousPhysicsTime / TimeSpan.TicksPerMillisecond));
            dPStringBuilder.Append("- Joint: ").AppendLine(string.Format("{0} ms", World.Island.JointUpdateTime / TimeSpan.TicksPerMillisecond));
            dPStringBuilder.Append("- Controller: ").AppendLine(string.Format("{0} ms", World.ControllersUpdateTime / TimeSpan.TicksPerMillisecond));
            dPStringBuilder.Append("- Total: ").AppendLine(string.Format("{0} ms", World.UpdateTime / TimeSpan.TicksPerMillisecond));
            debugRenderer.DrawString(x + 110, y, dPStringBuilder.ToString(), TextColor);

            Vector2 textSize = debugRenderer.Font.MeasureString(dPStringBuilder.ToString());

            y += (int)textSize.Y + 2;

            //Batch.Breakpoint = true;
            debugRenderer.DrawSegment(new Vector2(0, y), new Vector2(300, y), Color.Gray);

            Batch.Breakpoint = false;

            y += 10;

#if DEBUG
            string flushes = "Flush count: " + debugRenderer.Batch.LastFlushCount;
            textSize = debugRenderer.Font.MeasureString(flushes.ToString());
            debugRenderer.DrawString(x, y, flushes, TextColor);
            y += (int)textSize.Y + 10;
#endif

            if (SelectedBody != null)
            {
                dPStringBuilder.Clear();

                dPStringBuilder.AppendLine(string.Format("Body {0} info: ", SelectedBody.BodyId));

                dPStringBuilder.AppendLine(string.Format("Type: {0}", SelectedBody.BodyType.ToString()));
                dPStringBuilder.AppendLine(string.Format("Position: [{0:0.000}, {1:0.000}]", SelectedBody.Position.X, SelectedBody.Position.Y));
                dPStringBuilder.AppendLine(string.Format("Rotation: {0:0.000}", SelectedBody.Rotation));
                dPStringBuilder.AppendLine(string.Format("Linear velocity: [{0:0.000}, {1:0.000}] -> {2:0.000}", SelectedBody.LinearVelocity.X, SelectedBody.LinearVelocity.Y, SelectedBody.LinearVelocity.Length()));
                dPStringBuilder.AppendLine(string.Format("Angular velocity: {0:0.000}", SelectedBody.AngularVelocity));

                debugRenderer.DrawString(x, y, dPStringBuilder.ToString(), TextColor);
            }
        }

        #region DebugViewBase
        public override void DrawPolygon(FP.Vector2[] vertices, int count, float red, float green, float blue, bool closed = true)
        {
            debugRenderer.DrawPolygon(vertices.ToMGVector2(), count, new Color(red, green, blue), closed);
        }

        public override void DrawSolidPolygon(FP.Vector2[] vertices, int count, float red, float green, float blue)
        {
            debugRenderer.DrawSolidPolygon(vertices.ToMGVector2(), count, new Color(red, green, blue));
        }

        public override void DrawCircle(FP.Vector2 center, float radius, float red, float green, float blue)
        {
            debugRenderer.DrawCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), new Color(red, green, blue));
        }

        public override void DrawSolidCircle(FP.Vector2 center, float radius, FP.Vector2 axis, float red, float green, float blue)
        {
            debugRenderer.DrawSolidCircle(center.ToMGVector2(), ConvertUnits.ToDisplayUnits(radius), axis.ToMGVector2(), new Color(red, green, blue));
        }

        public override void DrawSegment(FP.Vector2 start, FP.Vector2 end, float red, float green, float blue)
        {
            debugRenderer.DrawSegment(start, end, new Color(red, green, blue));
        }

        public override void DrawTransform(ref FP.Transform transform)
        {
            const float axisScale = 0.4f;
            FP.Vector2 p1 = transform.p;

            FP.Vector2 p2 = p1 + axisScale * transform.q.GetXAxis();
            debugRenderer.DrawSegment(p1, p2, Color.Red);

            p2 = p1 + axisScale * transform.q.GetYAxis();
            debugRenderer.DrawSegment(p1, p2, Color.Green);
        }
        #endregion

        public void RenderDebugData(Matrix projection, Matrix view)
        {
            if (!Enabled)
                return;

            //Nothing is enabled - don't draw the debug view.
            if (Flags == 0)
                return;

            debugRenderer.Begin(projection, view);
            DrawDebugData();
            debugRenderer.End();

            if ((Flags & DebugViewFlags.PerformanceGraph) == DebugViewFlags.PerformanceGraph)
            {
                debugRenderer.Begin(projection, view);
                DrawPerformanceGraph();
                debugRenderer.End();
            }

            debugRenderer.Begin(projection);
            DrawDebugPanel();
            debugRenderer.End();
        }

        public void RenderDebugData(Matrix projection)
        {
            if (!Enabled)
                return;

            Matrix view = Matrix.Identity;
            RenderDebugData(projection, view);
        }

        public void LoadContent(GraphicsDevice device, ContentManager content)
        {
            this.device = device;

            debugRenderer.LoadContent(device, content);
        }

        #region Nested type: ContactPoint

        private struct ContactPoint
        {
            public Vector2 Normal;
            public Vector2 Position;
            public PointState State;
        }

        #endregion
    }
}