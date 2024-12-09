using System.Collections.Generic;

public class EquipCard_E_Trinket_2 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_2(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
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

	private void OnPlayerStoringForce(EventData data)
	{
		int specialAttr = Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr;
		List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		for (int i = 0; i < list.Count; i++)
		{
			EquipmentCard.EquipEffectAtkEntity(list[i], specialAttr, isAbsDmg: true);
		}
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
