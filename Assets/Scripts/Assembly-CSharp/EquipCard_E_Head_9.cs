public class EquipCard_E_Head_9 : EquipCard_E_Head
{
	private class BattleEffect_E_Head_9 : BaseBattleEffect
	{
		private const string EquipNameKey = "E_Head_9_Name";

		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverExtraCoin;

		public override void TakeEffect(BattleEffectData data, out int IntData, out string source)
		{
			IntData = 5;
			source = "E_Head_9_Name".LocalizeText();
		}
	}

	private BaseBattleEffect battleEffect;

	public EquipCard_E_Head_9(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		battleEffect = new BattleEffect_E_Head_9();
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerEffectContainer.AddBattleEffect(battleEffect);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerEffectContainer.RemoveBattleEffect(battleEffect);
	}
}
