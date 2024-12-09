public class EquipCard_E_Shoes_11 : EquipCard_E_Shoes
{
	private class BattleEffect_E_Shoes_11 : BaseBattleEffect
	{
		private const string EquipNameKey = "E_Shoes_11_Name";

		public override BattleEffectType BattleEffectType => BattleEffectType.UponBattleOverExtraCoin;

		public override void TakeEffect(BattleEffectData data, out int IntData, out string source)
		{
			IntData = 2;
			source = "E_Shoes_11_Name".LocalizeText();
		}
	}

	private BaseBattleEffect battleEffect;

	public EquipCard_E_Shoes_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		battleEffect = new BattleEffect_E_Shoes_11();
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
