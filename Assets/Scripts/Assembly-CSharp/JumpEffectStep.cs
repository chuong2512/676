public class JumpEffectStep : BaseEffectStep
{
	public float jumpHeight;

	public float jumpUpTime;

	public float jumpDowmTime;

	public override EffectConfigType EffectConfigType => EffectConfigType.Jump;
}
