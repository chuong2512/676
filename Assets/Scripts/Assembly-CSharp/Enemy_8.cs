public class Enemy_8 : ElementEnemy
{
	private EnemyCtrl_8 enemy8Ctrl;

	private Buff_OceanSprite _buffOceanSprite;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_8(EnemyAttr attr, EnemyCtrl_8 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy8Ctrl = enemyCtrl;
	}

	protected override void OnStartBattle()
	{
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffOceanSprite = new Buff_OceanSprite(this, int.MaxValue);
		GetBuff(_buffOceanSprite);
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffOceanSprite.TakeEffect(this);
		}
	}

	public override void StartBattleAction()
	{
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}

	protected override EnemyMean GetNextAction()
	{
		return null;
	}
}
