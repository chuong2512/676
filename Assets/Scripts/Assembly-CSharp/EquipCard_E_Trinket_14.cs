public class EquipCard_E_Trinket_14 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	private void OnPlayerStoringForce(EventData data)
	{
		ArcherPlayerAttr archerPlayerAttr = (ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr;
		Arrow[] array = new Arrow[3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new FireArrow();
		}
		archerPlayerAttr.AddSpecialArrow(array);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
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
