using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Mindvolving.Organisms.Physics;
using System;
using System.Collections.Generic;

namespace Mindvolving.Organisms
{
	public class Organ : IDisposable
	{
		internal List<Organ> m_children = new List<Organ>();

		internal Guid DNAUID { get; private set; }

		public Organism Organism { get; private set; }

		public Organ Parent { get; private set; }

		public World World { get; private set; }

		public Body Body { get; private set; }

		public DistanceJoint Bone { get; private set; }

		public float Radius { get; private set; }

		public IReadOnlyList<Organ> Children { get { return m_children; } }

		internal Organ(Guid dnauid, Organism organism, Organ parent, Vector2 position, float radius)
		{
			DNAUID = dnauid;
			Organism = organism;
			Parent = parent;
			Radius = radius;
			Body = new Body(organism.World, position, 0, BodyType.Dynamic, this);
			Body.CreateFixture(new CircleShape(radius, 1), new OrganUserData() { Organ = this });
			Body.UserData = new OrganUserData() { Organ = this };
			if (parent != null)
			{
				Bone = new DistanceJoint(parent.Body, Body, Vector2.Zero, Vector2.Zero);
				Bone.UserData = new BoneUserData() { From = parent, To = this };
				organism.World.AddJoint(Bone);
			}
		}

		~Organ()
		{
			Dispose();
		}

		public void Dispose()
		{
			foreach (var child in Children)
				child.Dispose();
			if (Body != null)
				Body.Dispose();
			if (Bone != null && Organism != null)
				Organism.World.RemoveJoint(Bone);
			Organism = null;
			Parent = null;
			World = null;
			Body = null;
			Bone = null;
			Radius = 0;
			m_children.Clear();
			GC.SuppressFinalize(this);
		}
	}
}
