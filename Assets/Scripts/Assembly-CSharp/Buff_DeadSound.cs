public class Buff_DeadSound : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_DeadSound;

	public Buff_DeadSound(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "死亡被动触发：连续撕裂，给玩家施加3层震荡buff");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, null, BuffEffect);
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 3));
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
