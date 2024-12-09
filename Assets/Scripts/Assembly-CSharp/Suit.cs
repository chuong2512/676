using System.Collections.Generic;
using System.Text;

public abstract class Suit
{
	protected HashSet<string> suitInfomation = new HashSet<string>();

	protected Dictionary<int, BaseBattleEffect> suitBattleEffect = new Dictionary<int, BaseBattleEffect>();

	public abstract SuitType SuitType { get; }

	public HashSet<string> SuitInfomation => suitInfomation;

	public virtual void OnBattleStart()
	{
		if (suitBattleEffect.Count != 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddSuitEffet(SuitType, string.Empty, GetSuitBattleEffectDescription).SetEffect();
		}
	}

	protected string GetSuitBattleEffectDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		SuitHandler.SuitInfo suitInfo = SuitHandler.GetSuitInfo(SuitType);
		bool flag = true;
		for (int i = 0; i < suitInfo.SuitNeedAmount.Length; i++)
		{
			if (suitInfomation.Count >= suitInfo.SuitNeedAmount[i])
			{
				if (flag)
				{
					flag = false;
					stringBuilder.Append(suitInfo.SuitContentKeys[i].LocalizeText());
				}
				else
				{
					stringBuilder.Append("\n").Append(suitInfo.SuitContentKeys[i].LocalizeText());
				}
			}
		}
		return stringBuilder.ToString();
	}

	protected void AddSuitBattleEffect(int level, BaseBattleEffect effect)
	{
		suitBattleEffect[level] = effect;
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(effect);
	}

	protected void RemoveSuitBattleEffect(int level)
	{
		if (suitBattleEffect.TryGetValue(level, out var value))
		{
			suitBattleEffect.Remove(level);
			Singleton<GameManager>.Instance.Player.PlayerEffectContainer.RemoveBattleEffect(value);
		}
	}

	public abstract void AddSuit(string equipCode);

	public abstract void RemoveSuit(string equipCode);
}
