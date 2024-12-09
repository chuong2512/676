using UnityEngine;

public class Buff_HuntAbility : BaseBuff
{
	private EntityBase target;

	public override BuffType BuffType => BuffType.Buff_HuntAbility;

	public Buff_HuntAbility(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
	}

	private void BuffEffect()
	{
		target.GetBuff(new Buff_Bleeding(target, 1));
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerAtkEnemy, OnPlayerAtkEnemy);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerAtkEnemy, OnPlayerAtkEnemy);
	}

	private void OnPlayerAtkEnemy(EventData data)
	{
		SimpleEventData simpleEventData;
		EntityBase entityBase;
		if ((simpleEventData = data as SimpleEventData) != null && (entityBase = simpleEventData.objValue as EntityBase) != null)
		{
			target = entityBase;
			TakeEffect(base.entityBase);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}
}
