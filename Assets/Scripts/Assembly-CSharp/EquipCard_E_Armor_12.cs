public class EquipCard_E_Armor_12 : EquipCard_E_Armor
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private bool isHaveGetBuff;

	public EquipCard_E_Armor_12(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_UpdatePlayerHealth, OnPlayerHealthChanged);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_UpdatePlayerHealth, OnPlayerHealthChanged);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayerHealthChanged(EventData data)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			float num = (float)Singleton<GameManager>.Instance.Player.PlayerAttr.Health / (float)Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth;
			if (num < 0.5f && !isHaveGetBuff)
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, 3));
				isHaveGetBuff = true;
				EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
				_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
				_equipEffectIconCtrl.SetEffect();
			}
			else if (num >= 0.5f && isHaveGetBuff)
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, -3));
				isHaveGetBuff = false;
				_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
				_equipEffectIconCtrl.SetNotEffect();
			}
		}
	}

	private void OnBattleStart(EventData data)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		if ((float)Singleton<GameManager>.Instance.Player.PlayerAttr.Health / (float)Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth < 0.5f)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, 3));
			isHaveGetBuff = true;
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
			_equipEffectIconCtrl.SetEffect();
		}
		else
		{
			isHaveGetBuff = false;
			_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
			_equipEffectIconCtrl.SetNotEffect();
		}
	}

	private string GetEquipEffectHint()
	{
		if (!isHaveGetBuff)
		{
			return "0";
		}
		return "3";
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}
}
