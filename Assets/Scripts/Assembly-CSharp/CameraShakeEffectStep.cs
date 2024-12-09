public class CameraShakeEffectStep : BaseEffectStep
{
	public float shakeDuration = 0.2f;

	public float shakeStrength = 0.3f;

	public int shakeVibrato = 10;

	public float shakeRandomness;

	public override EffectConfigType EffectConfigType => EffectConfigType.CameraShake;
}
