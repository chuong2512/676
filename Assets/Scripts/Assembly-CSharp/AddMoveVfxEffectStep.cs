using UnityEngine;

public class AddMoveVfxEffectStep : BaseEffectStep
{
	public enum PosType
	{
		BuffIcon,
		PlayerHeadPortrait,
		EnemyItSelf,
		ApPoint,
		FaithPoint,
		PlayerHealthPoint,
		AllEnemy
	}

	public string vfxName;

	public PosType startPos;

	public Vector3 startPosOffset;

	public PosType endPos;

	public Vector3 endPosOffset;

	public float duration;

	public override EffectConfigType EffectConfigType => EffectConfigType.AddMoveVfx;
}
