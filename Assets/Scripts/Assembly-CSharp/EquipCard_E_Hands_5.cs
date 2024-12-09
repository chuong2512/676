public class EquipCard_E_Hands_5 : EquipCard_E_Hands
{
	private class E_Hands_5_BattleEffect : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardApCostReduce;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_O_1")
			{
				IntData = 1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private BaseBattleEffect _baseBattleEffect;

	public EquipCard_E_Hands_5(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_baseBattleEffect = new E_Hands_5_BattleEffect();
	}

	protected override void OnEquip(Player player)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(_baseBattleEffect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.RemoveBattleEffect(_baseBattleEffect);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes).SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return string.Empty;
	}

	private string GetEquipEffectDes()
	{
		return equipmentCardAttr.EquipEffectDesKey.LocalizeText();
	}
}
