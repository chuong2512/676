using UnityEngine;

public class Buff_DefenceRestrik : BaseBuff
{
	private EntityBase targetEntity;

	public override BuffType BuffType => BuffType.Buff_DefenceRestrik;

	public Buff_DefenceRestrik(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(entityBase.EntityName + "的格挡反击的效果触发，反弹的真实伤害为：" + (isPlayer ? ((PlayerAttr)entityBase.EntityAttr).DefenceAttr : ((EnemyAttr)entityBase.EntityAttr).Block));
			}
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), isPlayer ? null : entityBase.EntityTransform, (!isPlayer) ? null : new Transform[1] { ((EnemyBase)targetEntity).EnemyCtrl.transform }, BuffEffect);
			buffIconCtrl.BuffEffectHint();
		}
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(targetEntity, isPlayer ? ((PlayerAttr)entityBase.EntityAttr).DefenceAttr : ((EnemyAttr)entityBase.EntityAttr).Block, isAbsDmg: true);
		EventManager.BroadcastEvent(entityBase, EventEnum.E_EntityDefenceRestrick, null);
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
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的格挡反击的效果获得提升，当前的格挡反击剩余次数：" + base.BuffRemainRound);
		}
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_EntityBeAttacked, OnEntityBeAttacked);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_EntityBeAttacked, OnEntityBeAttacked);
	}

	private void OnEntityBeAttacked(EventData data)
	{
		SimpleEventData simpleEventData;
		EntityBase entityBase;
		if ((simpleEventData = data as SimpleEventData) != null && (entityBase = simpleEventData.objValue as EntityBase) != null)
		{
			targetEntity = entityBase;
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
