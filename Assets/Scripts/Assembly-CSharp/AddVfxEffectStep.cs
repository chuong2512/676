using UnityEngine;

public class AddVfxEffectStep : BaseEffectStep
{
	public enum VfxCastType
	{
		SingleTarget,
		AllTarget,
		ScreenCentre,
		Player,
		PlayerApPt,
		PlayerFaithPt,
		Self,
		AllExistEnemy,
		ArmorPt
	}

	public string vfxName;

	public bool mute;

	public VfxCastType castType;

	public Vector3 offset;

	public bool isFollow;

	public Vector2 randomOffset;

	public override EffectConfigType EffectConfigType => EffectConfigType.AddVfx;
}
