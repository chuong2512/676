using UnityEngine;

public class Buff_WolfRelationship : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_WolfRelationship;

	public Buff_WolfRelationship(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("狼族血缘buff效果触发：每有一只恐狼死亡, 自身力量+1)");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), null, new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		entityBase.GetBuff(new Buff_Power(entityBase, 1));
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.objValue is Enemy_3)
		{
			TakeEffect(entityBase);
		}
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}
}
