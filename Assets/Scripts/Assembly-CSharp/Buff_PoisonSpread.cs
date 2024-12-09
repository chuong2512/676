public class Buff_PoisonSpread : BaseBuff
{
	private int poisonAmount;

	public int PoisonAmount => poisonAmount;

	public override BuffType BuffType => BuffType.Buff_PoisonSpread;

	public Buff_PoisonSpread(EntityBase entityBase, int poisonAmount)
		: base(entityBase, int.MaxValue)
	{
		this.poisonAmount = poisonAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{entityBase.EntityName}被动触发：给玩家上{poisonAmount}层致命毒药");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, poisonAmount));
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		poisonAmount += ((Buff_PoisonSpread)baseBuff).PoisonAmount;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_EnemyRound, OnEnemyRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_EnemyRound, OnEnemyRound);
	}

	private void OnEnemyRound(EventData data)
	{
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return poisonAmount.ToString();
	}

	public override int GetBuffHinAmount()
	{
		return poisonAmount;
	}
}
