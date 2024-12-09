public class ProphesyCard_PC_3 : ProphesyCard
{
	private class ProphesyCard3_BattleEffectForEquipDrop : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverEquipExtraDropRate;

		public override void TakeEffect(BattleEffectData data, out float FloatData)
		{
			if (Singleton<GameManager>.Instance.CurrentMapLayer == 1 && Singleton<GameManager>.Instance.CurrentMapLevel == 1)
			{
				FloatData = 1f;
			}
			else
			{
				FloatData = 0f;
			}
		}
	}

	private class ProphesyCard3_BattleEffectForCardDrop : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverCardExtraDropRate;

		public override void TakeEffect(BattleEffectData data, out float FloatData)
		{
			if (Singleton<GameManager>.Instance.CurrentMapLayer == 1 && Singleton<GameManager>.Instance.CurrentMapLevel == 1)
			{
				FloatData = -1f;
			}
			else
			{
				FloatData = 0f;
			}
		}
	}

	public override string ProphesyCode => "PC_3";

	public override void Active(bool isLoad)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(new ProphesyCard3_BattleEffectForEquipDrop());
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(new ProphesyCard3_BattleEffectForCardDrop());
	}
}
