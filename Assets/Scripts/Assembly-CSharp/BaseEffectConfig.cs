using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "_EffectConfig", menuName = "CustomAsset/EffectConfig")]
public class BaseEffectConfig : SerializedScriptableObject
{
	public float effecTime;

	public float durationTime;

	public BaseEffectStep[] allStep;
}
