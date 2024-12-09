public class Enemy_9 : ElementEnemy
{
	private EnemyCtrl_9 enemy9Ctrl;

	private Buff_RockSprite _buffRockSprite;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_9(EnemyAttr attr, EnemyCtrl_9 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy9Ctrl = enemyCtrl;
	}

	protected override void OnStartBattle()
	{
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffRockSprite = new Buff_RockSprite(this, int.MaxValue);
		GetBuff(_buffRockSprite);
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffRockSprite.TakeEffect(this);
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
