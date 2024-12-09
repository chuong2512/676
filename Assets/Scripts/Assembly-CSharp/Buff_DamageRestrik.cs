using UnityEngine;

public class Buff_DamageRestrik : BaseBuff
{
	private EntityBase targetEntity;

	public override BuffType BuffType => BuffType.Buff_DamageRestrik;

	public Buff_DamageRestrik(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			BaseEffectConfig config = BaseBuff.LoadEffectConfig(isPlayer, BuffType);
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(config, buffIconCtrl.transform, (!isPlayer) ? new Transform[1] { entityBase.EntityTransform } : new Transform[1] { ((EnemyBase)targetEntity).EnemyCtrl.transform }, BuffEffect);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(string.Format(entityBase.EntityName + "的反伤buff触发，对{0}造成{1}的真实伤害", targetEntity.EntityName, base.BuffRemainRound));
			}
			EventManager.BroadcastEvent(entityBase, EventEnum.E_EntityDamageRestrick, new SimpleEventData
			{
				objValue = targetEntity
			});
			buffIconCtrl.BuffEffectHint();
		}
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(targetEntity, base.BuffRemainRound, isAbsDmg: true);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.BuffRemainRound;
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的反伤buff层数发生变化，当前的buff层数剩余：" + base.BuffRemainRound);
		}
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
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
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.objValue.IsNull() && (entityBase = simpleEventData.objValue as EntityBase) != null)
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
