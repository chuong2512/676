using UnityEngine;

public class ScreenDisdortionEffectStep : BaseEffectStep
{
	[Range(0f, 0.1f)]
	public float blurFactor = 0.02f;

	[Range(0f, 2f)]
	public float lerpFactor = 1.2f;

	public int downSampleFactor = 3;

	public float inTime;

	public float holdTime;

	public float recoverTime;

	public override EffectConfigType EffectConfigType => EffectConfigType.ScreenDisdortion;
}
