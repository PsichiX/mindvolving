using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Physics = FarseerPhysics;

namespace Mindvolving.Visualization.Engine.Entities
{
    public class OrganismEntityBuilder
    {
        private OrganismEntity entity;
        private World world;

        public void BeginBuilding(World world)
        {
            entity = new OrganismEntity();
            entity.OrganicBody = new Organism.Body();
        }

        public void AddBodyPart(Physics.Dynamics.Body physicalBodyPart)
        {
            Organism.BodyPart bodyPart = entity.OrganicBody.CreateBodyPart();
            bodyPart.PhysicalBody = physicalBodyPart;
        }

        public void AddBone(int bodyPartIndex1, int bodyPartIndex2)
        {
            entity.OrganicBody.Skeleton.CreateBone(entity.OrganicBody.BodyParts[bodyPartIndex1], entity.OrganicBody.BodyParts[bodyPartIndex2]);
        }

        public void AddMuscle(int bodyPartIndex1, int bodyPartIndex2, DistanceJoint joint, float idleLength, float contractionLength)
        {
            Organism.Muscle muscle = entity.OrganicBody.CreateMuscle(entity.OrganicBody.BodyParts[bodyPartIndex1], entity.OrganicBody.BodyParts[bodyPartIndex2]);
            muscle.Joint = joint;
            muscle.IdleLength = idleLength;
            muscle.ContractionLength = contractionLength;
        }

        public OrganismEntity Build()
        {
            world.BringEntityIntoWorld(entity);

            return entity;
        }
    }
}
