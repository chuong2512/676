using UnityEngine;

public class Buff_Angry : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Angry;

	public Buff_Angry(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
	}

	private void BuffEffect()
	{
		entityBase.GetBuff(new Buff_Power(entityBase, base.BuffRemainRound));
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
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的愤怒buff的效果提升，受到攻击能获得的力量：" + base.BuffRemainRound);
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
		EventManager.RegisterObjRelatedEvent(entityBase, EventEnum.E_EntityGetHurt, OnEntityGetHurt);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterObjRelatedEvent(entityBase, EventEnum.E_EntityGetHurt, OnEntityGetHurt);
	}

	private void OnEntityGetHurt(EventData data)
	{
		TakeEffect(entityBase);
	}
}
