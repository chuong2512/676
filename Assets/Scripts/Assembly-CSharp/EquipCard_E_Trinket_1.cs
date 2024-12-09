using System.Collections.Generic;
using UnityEngine;

public class EquipCard_E_Trinket_1 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_1(EquipmentCardAttr equipmentCardAttr)
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

	private void OnPlayerStoringForce(EventData data)
	{
		int playerCurrentCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PlayerCurrentCardAmount;
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		for (int i = 0; i < playerCurrentCardAmount; i++)
		{
			if (allEnemies.Count <= 0)
			{
				break;
			}
			EquipmentCard.EquipEffectAtkEntity(allEnemies[Random.Range(0, allEnemies.Count)], 1, isAbsDmg: true);
		}
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
