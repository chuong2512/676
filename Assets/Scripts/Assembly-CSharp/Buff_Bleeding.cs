using UnityEngine;

public class Buff_Bleeding : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Bleeding;

	public Buff_Bleeding(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的流血buff效果触发，减少生命值：" + base.BuffRemainRound);
		}
	}

	private void BuffEffect()
	{
		int dmgAmount = base.BuffRemainRound;
		entityBase.EntityAttr.ReduceHealth(base.BuffRemainRound);
		buffRemainRound -= 1f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
		}
		Transform transform = null;
		transform = ((!isPlayer) ? ((EnemyBase)entityBase).EnemyCtrl.HealthBarTransform : (SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerHealthTransform);
		Singleton<GameHintManager>.Instance.ShowBleedingDamageFlowingText(transform, !isPlayer, Vector3.right, dmgAmount, isPlayer ? 0.005f : 0.5f);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的流血buff效果增加，当前的流血层数：" + base.BuffRemainRound);
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
		return "<color=#ec2125ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}
}
