public class EquipCard_E_Shoes_20 : EquipCard_E_Shoes
{
	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Shoes_20(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerHandCardChanged, OnPlayerHandCardChanged);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerHandCardChanged, OnPlayerHandCardChanged);
	}

	private void OnPlayerHandCardChanged(EventData data)
	{
		if (!(_effectIconCtrl == null))
		{
			if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardAmount == 0)
			{
				_effectIconCtrl.SetEffect();
			}
			else
			{
				_effectIconCtrl.SetNotEffect();
			}
		}
	}

	private void OnPlayerEndRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardAmount == 0)
		{
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DefenceToAttack(Singleton<GameManager>.Instance.Player, 1));
		}
	}

	private void OnBattleStart(EventData data)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetNotEffect();
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
