using UnityEngine;

public class EquipCard_E_Hands_10 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_10(EquipmentCardAttr equipmentCardAttr)
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
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle && Singleton<EnemyController>.Instance.AllEnemies.Count > 0)
		{
			EquipmentCard.EquipEffectAtkEntity(Singleton<EnemyController>.Instance.AllEnemies[Random.Range(0, Singleton<EnemyController>.Instance.AllEnemies.Count)], 3, isAbsDmg: true);
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
	}
}
