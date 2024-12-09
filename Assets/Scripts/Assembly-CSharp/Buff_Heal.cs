using UnityEngine;

public class Buff_Heal : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Heal;

	public Buff_Heal(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的治愈buff效果触发，恢复生命值" + base.BuffRemainRound);
		}
		buffIconCtrl.BuffEffectHint();
		buffRemainRound -= 1f;
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { base.entityBase.EntityTransform }, BuffEffect);
	}

	private void BuffEffect()
	{
		entityBase.EntityRecoveryHealthOnBattle(base.BuffRemainRound);
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.BuffRemainRound;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的治愈buff层数获得提升，当前的buff层数剩余：" + base.BuffRemainRound);
		}
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(isPlayer ? EventEnum.E_PlayerRound : EventEnum.E_EnemyRound, OnRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(isPlayer ? EventEnum.E_PlayerRound : EventEnum.E_EnemyRound, OnRound);
	}

	private void OnRound(EventData data)
	{
		TakeEffect(entityBase);
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
