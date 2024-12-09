using System.Collections.Generic;

public class EnemySkeletonAnimEffectStep : BaseEffectStep
{
	public struct EnemySkeletonEffect
	{
		public string skeletonAnimName;

		public int skeletonAnimTrack;

		public bool isLoop;
	}

	public List<EnemySkeletonEffect> animList;

	public override EffectConfigType EffectConfigType => EffectConfigType.EnemySkeletonAnim;
}
