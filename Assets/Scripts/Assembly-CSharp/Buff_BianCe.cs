using UnityEngine;

public class Buff_BianCe : BaseBuff
{
	private const string BuffEffectConfigName = "Buff_BianCe_Player";

	private int dmg;

	private EnemyBase targetEnemy;

	public override BuffType BuffType => BuffType.Buff_BianCe;

	public Buff_BianCe(EntityBase entityBase, int round, int dmg)
		: base(entityBase, round)
	{
		this.dmg = dmg;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, new Transform[1] { targetEnemy.EnemyCtrl.transform }, BuffEffect);
			buffIconCtrl.BuffEffectHint();
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(string.Format(entityBase.EntityName + "的复仇buff的效果触发，对{0}造成{1}的真实伤害", entityBase.EntityName, Singleton<GameManager>.Instance.Player.PlayerAttr.AtkDmg));
			}
		}
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(targetEnemy, Singleton<GameManager>.Instance.Player.PlayerAttr.AtkDmg, isAbsDmg: true);
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else if (buffRemainRound == (float)base.BuffRemainRound)
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
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的复仇buff层数获得提升，当前的buff层数剩余：" + base.BuffRemainRound);
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

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_EntityBeAttacked, PlayerBeAttacked);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_EntityBeAttacked, PlayerBeAttacked);
	}

	private void PlayerBeAttacked(EventData data)
	{
		SimpleEventData simpleEventData;
		EntityBase entityBase;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.objValue.IsNull() && (entityBase = simpleEventData.objValue as EntityBase) != null)
		{
			targetEnemy = (EnemyBase)entityBase;
			TakeEffect(base.entityBase);
		}
	}
}
