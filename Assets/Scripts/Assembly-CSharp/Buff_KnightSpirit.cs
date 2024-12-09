using System.Collections.Generic;
using UnityEngine;

public class Buff_KnightSpirit : BaseBuff
{
	private int baseSpirit;

	public int BaseSpirit => baseSpirit;

	public override BuffType BuffType => BuffType.Buff_KnightSpirit;

	public Buff_KnightSpirit(EntityBase entityBase, int spiritAmount)
		: base(entityBase, int.MaxValue)
	{
		baseSpirit = spiritAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		Transform[] array = new Transform[allEnemies.Count - 1];
		if (allEnemies.Count > 1)
		{
			int num = 0;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				if (allEnemies[i] != entityBase)
				{
					array[num] = allEnemies[i].EntityTransform;
					num++;
				}
			}
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : array, BuffEffect);
	}

	private void BuffEffect()
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i] != entityBase)
			{
				allEnemies[i].GetBuff(new Buff_Cover(allEnemies[i], baseSpirit));
			}
		}
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + baseSpirit + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return baseSpirit;
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		baseSpirit += ((Buff_KnightSpirit)baseBuff).BaseSpirit;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_EnemyRound, OnEnemyRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_EnemyRound, OnEnemyRound);
	}

	private void OnEnemyRound(EventData data)
	{
		TakeEffect(entityBase);
	}
}
