public class Buff_FireSprite : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_FireSprite;

	public Buff_FireSprite(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "被动生效，对玩家造成3点绝对伤害");
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, null, BuffEffect);
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(Singleton<GameManager>.Instance.Player, 3, isAbsDmg: true);
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
