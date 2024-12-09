public class Enemy_56 : EnemyBase
{
	private EnemyCtrl_56 enemy56Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_56(EnemyAttr attr, EnemyCtrl_56 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy56Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[0];
	}

	protected override void OnStartBattle()
	{
	}

	public void OnEnemyRound(int remain)
	{
		enemy56Ctrl.SetBoomRoundRemain(remain);
	}

	public void SetBuffRound(int round)
	{
		GetBuff(new Buff_CountDown(this, round));
		enemy56Ctrl.SetBoomRoundRemain(round);
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
