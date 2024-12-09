public class Enemy_7 : ElementEnemy
{
	private EnemyCtrl_7 enemy7Ctrl;

	private Buff_FireSprite _buffFireSprite;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_7(EnemyAttr attr, EnemyCtrl_7 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy7Ctrl = enemyCtrl;
	}

	protected override void OnStartBattle()
	{
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffFireSprite = new Buff_FireSprite(this, int.MaxValue);
		GetBuff(_buffFireSprite);
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffFireSprite.TakeEffect(this);
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
