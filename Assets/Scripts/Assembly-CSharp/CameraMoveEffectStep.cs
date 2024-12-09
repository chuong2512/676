using DG.Tweening;
using UnityEngine;

public class CameraMoveEffectStep : BaseEffectStep
{
	public Vector3 moveDir;

	public float moveTime;

	public float durationTime;

	public float recoverTime;

	public Ease moveEase;

	public override EffectConfigType EffectConfigType => EffectConfigType.CameraMove;
}
