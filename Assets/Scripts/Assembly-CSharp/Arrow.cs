using UnityEngine;

public abstract class Arrow
{
	public enum ArrowType
	{
		Normal,
		Blunt,
		Fire,
		Freeze,
		Poison,
		Sawtooth
	}

	private const string ARROW_EFFECT_PATH = "EffectConfigScriObj/Arrow";

	public abstract ArrowType MArrowType { get; }

	protected abstract string EffectConfigName { get; }

	protected abstract string AddArrowEffectConfigName { get; }

	public void OnAddArrow(Transform target)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadArrowEffect(AddArrowEffectConfigName), target, null, null);
	}

	public void ArrowEffect(EntityBase[] targets)
	{
		if (targets != null)
		{
			Transform[] array = new Transform[targets.Length];
			for (int i = 0; i < targets.Length; i++)
			{
				array[i] = targets[i].EntityTransform;
			}
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadArrowEffect(EffectConfigName), null, array, delegate
			{
				OnArrowEffect(targets);
				EventManager.BroadcastEvent(EventEnum.E_SpecialArrowEffect, new SimpleEventData
				{
					objValue = targets,
					intValue = (int)MArrowType
				});
			});
		}
		else
		{
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadArrowEffect(EffectConfigName), null, null, delegate
			{
				OnArrowEffect(null);
				EventManager.BroadcastEvent(EventEnum.E_SpecialArrowEffect, new SimpleEventData
				{
					objValue = null,
					intValue = (int)MArrowType
				});
			});
		}
	}

	protected abstract void OnArrowEffect(EntityBase[] targets);

	private static BaseEffectConfig LoadArrowEffect(string name)
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(name, "EffectConfigScriObj/Arrow");
	}
}
