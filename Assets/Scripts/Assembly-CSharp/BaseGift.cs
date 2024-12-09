using System;

public abstract class BaseGift
{
	[Serializable]
	public enum GiftType
	{
		Blessing,
		Damnation
	}

	[Serializable]
	public enum GiftName
	{
		EnduranceBlessing = 0,
		PowerBlessing = 1,
		AgileBlessing = 2,
		LifeBlessing = 3,
		ProtectBlessing = 4,
		BattleBlessing = 5,
		EvilProtectBlessing = 12,
		BreakDamnation = 6,
		PlagueDamnation = 7,
		FragileDamnation = 8,
		OldDamnation = 9,
		SilenceDamnation = 10,
		FreezeDamnation = 11
	}

	private const string GIFT_EFFECT_PATH = "EffectConfigScriObj/Gift";

	public abstract GiftName Name { get; }

	public virtual void OnBattleStart()
	{
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(Name);
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadConfigByName(giftDataByGiftName.EffectConfigName), null, null, Effect);
		GiftManager.Instace.OnGiftEffectOver(this);
	}

	protected abstract void Effect();

	protected static BaseEffectConfig LoadConfigByName(string configName)
	{
		if (configName.IsNullOrEmpty())
		{
			return null;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(configName, "EffectConfigScriObj/Gift");
	}
}
