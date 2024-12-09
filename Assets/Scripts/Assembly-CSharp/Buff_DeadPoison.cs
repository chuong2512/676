using UnityEngine;

public class Buff_DeadPoison : BaseBuff
{
	private int poisonAmount;

	public int PoisonAmount => poisonAmount;

	public override BuffType BuffType => BuffType.Buff_DeadPoison;

	public Buff_DeadPoison(EntityBase entityBase, int poisonAmount)
		: base(entityBase, 999)
	{
		this.poisonAmount = poisonAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的致命毒药buff效果触发，减少的生命值：" + poisonAmount);
		}
	}

	private void BuffEffect()
	{
		entityBase.EntityAttr.ReduceHealth(poisonAmount);
		Transform transform = null;
		transform = ((!isPlayer) ? ((EnemyBase)entityBase).EnemyCtrl.HealthBarTransform : (SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerHealthTransform);
		Singleton<GameHintManager>.Instance.ShowPoisonDamageFlowingText(transform, !isPlayer, Vector3.right, poisonAmount, isPlayer ? 0.005f : 0.5f);
		EventManager.BroadcastEvent(EventEnum.E_PoisonBuffEffect, new SimpleEventData
		{
			objValue = entityBase
		});
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		poisonAmount += ((Buff_DeadPoison)baseBuff).PoisonAmount;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的致命毒药buff效果获得提升，现在每回合能减少的生命值为：" + poisonAmount);
		}
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(isPlayer ? EventEnum.E_PlayerRound : EventEnum.E_EnemyRound, OnItRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(isPlayer ? EventEnum.E_PlayerRound : EventEnum.E_EnemyRound, OnItRound);
	}

	private void OnItRound(EventData data)
	{
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return "<color=#ec2125ff>" + poisonAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return poisonAmount;
	}
}
