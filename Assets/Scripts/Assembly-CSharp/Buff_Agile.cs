using System.Globalization;

public class Buff_Agile : BaseBuff
{
	private int agileAmount;

	public int AgileAmount => agileAmount;

	public override BuffType BuffType => BuffType.Buff_Agile;

	public Buff_Agile(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
		agileAmount = round;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		agileAmount--;
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的迅捷buff效果触发，剩余层数：" + agileAmount);
		}
		if (agileAmount <= 0)
		{
			base.entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
			Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), null, null, null);
		}
		buffIconCtrl.BuffEffectHint();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		agileAmount += ((Buff_Agile)baseBuff).AgileAmount;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的迅捷buff的层数增加，当前层数为：" + agileAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + agileAmount.ToString(CultureInfo.InvariantCulture) + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return agileAmount;
	}
}
