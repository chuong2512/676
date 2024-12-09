public class Buff_FastAttack : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_FastAttack;

	public Buff_FastAttack(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropHandCard();
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
