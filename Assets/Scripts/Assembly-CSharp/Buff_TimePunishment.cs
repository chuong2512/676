public class Buff_TimePunishment : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_TimePunishment;

	public Buff_TimePunishment(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "被动生效，玩家丢弃所有的手牌");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, null, BuffEffect);
	}

	private void BuffEffect()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).RecycleAllSupHandCardControllers(null, null);
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearMainHandCards(null);
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearSupHandCards(null);
	}

	public override void UpdateRoundTurn()
	{
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
