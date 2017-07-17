using System;
namespace Mindvolving.Organisms
{
	public partial class DNA
	{
		public interface IMutator
		{
			int OrganConnectionChangePoints { get; set; }
			int OrganRadiusChangePoints { get; set; }
			int OrganRadialOrientationChangePoints { get; set; }
			int MuscleConnectionChangePoints { get; set; }
			int MuscleRadialPositionChangePoints { get; set; }


		}
	}
}
