using UnityEngine;

public class Buff_SeekChance : BaseBuff
{
	private EnemyBase targetEnemy;

	public override BuffType BuffType => BuffType.Buff_SeekChance;

	public Buff_SeekChance(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的光明守卫buff效果触发，为对方施加震荡buff");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, new Transform[1] { targetEnemy.EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		targetEnemy.GetBuff(new Buff_Shocked(targetEnemy, base.BuffRemainRound));
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的光明守卫buff效果提升，这回合每次格挡都会为敌人施加震荡的层数：" + base.BuffRemainRound);
		}
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
	}

	private void OnPlayerBlockDmg(EventData data)
	{
		SimpleEventData simpleEventData;
		EntityBase entityBase;
		if ((simpleEventData = data as SimpleEventData) != null && (entityBase = simpleEventData.objValue as EntityBase) != null)
		{
			targetEnemy = (EnemyBase)entityBase;
			TakeEffect(base.entityBase);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}
}
