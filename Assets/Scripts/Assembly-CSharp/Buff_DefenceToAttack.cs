public class Buff_DefenceToAttack : BaseBuff
{
	private int ap;

	public int Ap => ap;

	public override BuffType BuffType => BuffType.Buff_DefenceToAttack;

	public Buff_DefenceToAttack(EntityBase entityBase, int ap)
		: base(entityBase, 1)
	{
		this.ap = ap;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: true, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的振奋buff效果触发，获得额外的体力值为：" + ap);
		}
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(ap);
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
		ap += ((Buff_DefenceToAttack)baseBuff).Ap;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的振奋buff效果获得提升，下回合能获得额外体力值为：" + ap);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + ap + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return ap;
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		TakeEffect(entityBase);
	}
}
