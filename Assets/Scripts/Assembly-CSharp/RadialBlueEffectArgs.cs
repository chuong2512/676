using UnityEngine;

public class RadialBlueEffectArgs
{
	public float blurFactor;

	public float lerpFactor;

	public int downSampleFactor;

	public Vector2 blurCenter;

	public float inTime;

	public float recoverTime;

	public float holdTime;

	public RadialBlueEffectArgs(float blurFactor, float lerpFactor, int downSampleFactor, Vector2 blurCenter, float inTime, float recoverTime, float holdTime)
	{
		this.blurFactor = blurFactor;
		this.lerpFactor = lerpFactor;
		this.downSampleFactor = downSampleFactor;
		this.blurCenter = blurCenter;
		this.inTime = inTime;
		this.recoverTime = recoverTime;
		this.holdTime = holdTime;
	}
}
