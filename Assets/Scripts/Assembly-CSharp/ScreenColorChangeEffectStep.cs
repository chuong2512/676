using DG.Tweening;
using UnityEngine;

public class ScreenColorChangeEffectStep : BaseEffectStep
{
	public Color targetColor;

	public Ease Ease;

	public float changeTime;

	public float durationTime;

	public float recoveryTime;

	public override EffectConfigType EffectConfigType => EffectConfigType.ScreenColorChange;
}
