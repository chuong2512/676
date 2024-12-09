public class ProphesyCard_PC_2 : ProphesyCard
{
	private class ProphesyCard2_BattleEffectForCoin : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverExtraCoin;

		public override void TakeEffect(BattleEffectData data, out int IntData, out string source)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && Singleton<GameManager>.Instance.CurrentMapLayer == 1 && Singleton<GameManager>.Instance.CurrentMapLevel == 1)
			{
				source = "预言牌";
				IntData = simpleEffectData.intData;
			}
			else
			{
				source = string.Empty;
				IntData = 0;
			}
		}
	}

	private class ProphesyCard2_BattleEffectForExp : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverExtraExp;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && Singleton<GameManager>.Instance.CurrentMapLayer == 1 && Singleton<GameManager>.Instance.CurrentMapLevel == 1)
			{
				IntData = -simpleEffectData.intData / 2;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	public override string ProphesyCode => "PC_2";

	public override void Active(bool isLoad)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(new ProphesyCard2_BattleEffectForCoin());
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(new ProphesyCard2_BattleEffectForExp());
	}
}
