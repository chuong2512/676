using UnityEngine;

public class Buff_HoldPosition : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_HoldPosition;

	public Buff_HoldPosition(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer, BuffType), buffIconCtrl.transform, isPlayer ? null : new Transform[1] { ((EnemyBase)entityBase).EnemyCtrl.transform }, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		buffRemainRound -= 1f;
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
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的招架buff次数获得提升，当前的次数剩余：" + base.BuffRemainRound);
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
