using System;
using System.Collections.Generic;

public class FlameDevourSuit : Suit
{
	public class FlameDevour_SuitEffect_2 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		}

		private void OnPlayerRound(EventData data)
		{
			List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
			for (int i = 0; i < list.Count; i++)
			{
				BaseBattleEffect.BattleEffectAtkEntity(list[i], 4, isAbsDmg: true);
			}
		}
	}

	public class FlameDevour_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			Singleton<GameManager>.Instance.Player.PlayerAttr.IsWillTakeFlameDevourDmg = false;
			EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			Singleton<GameManager>.Instance.Player.PlayerAttr.IsWillTakeFlameDevourDmg = true;
			EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		}

		private void OnPlayerRound(EventData data)
		{
			List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
			for (int i = 0; i < list.Count; i++)
			{
				BaseBattleEffect.BattleEffectAtkEntity(list[i], 4, isAbsDmg: true);
			}
		}
	}

	public override SuitType SuitType => SuitType.FlameDevour;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 2:
			AddSuitBattleEffect(2, new FlameDevour_SuitEffect_2());
			break;
		case 3:
			AddSuitBattleEffect(3, new FlameDevour_SuitEffect_3());
			break;
		}
	}

	public override void RemoveSuit(string equipCode)
	{
		if (!suitInfomation.Remove(equipCode))
		{
			throw new ArgumentException("Value does not contain in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 1:
			RemoveSuitBattleEffect(2);
			break;
		case 2:
			RemoveSuitBattleEffect(3);
			break;
		}
	}
}
