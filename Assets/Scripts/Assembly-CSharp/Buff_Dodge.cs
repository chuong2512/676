using UnityEngine;

public class Buff_Dodge : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Dodge;

	public Buff_Dodge(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的躲闪buff效果触发，闪躲当前的攻击，当前的buff的效果层数剩余：" + base.BuffRemainRound);
		}
		buffIconCtrl.BuffEffectHint();
		buffRemainRound -= 1f;
		if (buffRemainRound <= 0f)
		{
			base.entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
		}
		Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("Buff_Dodge_Name".LocalizeText(), Color.white, Color.white, entityBase.EntityTransform, isSetParent: false, Vector3.zero, null, 0f);
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, null);
		EventManager.BroadcastEvent(entityBase, EventEnum.E_OnDodgeBuffEffect, null);
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
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的躲闪buff层数获得提升，当前的buff的效果层数剩余：" + base.BuffRemainRound);
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
