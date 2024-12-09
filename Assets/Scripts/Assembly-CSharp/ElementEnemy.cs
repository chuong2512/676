public abstract class ElementEnemy : EnemyBase
{
	protected ElementEnemy(EnemyAttr attr, EnemyBaseCtrl enemyCtrl)
		: base(attr, enemyCtrl)
	{
	}

	public void FadeDead()
	{
		isDead = true;
		Singleton<GameManager>.Instance.BattleSystem.BuffSystem.RemoveBuff(this);
		enemyCtrl.OnEnemyDead(null);
	}
}
