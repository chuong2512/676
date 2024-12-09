using DG.Tweening;

public class CameraSizeChangeEffectStep : BaseEffectStep
{
	public float targetScale;

	public float moveTime;

	public float holdTime;

	public float recoverTime;

	public Ease moveEase;

	public override EffectConfigType EffectConfigType => EffectConfigType.CameraSizeChange;
}
