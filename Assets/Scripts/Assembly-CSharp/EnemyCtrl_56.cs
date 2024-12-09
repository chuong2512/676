public class EnemyCtrl_56 : EnemyBaseCtrl
{
	private Monster56AnimCtrl _monster56AnimCtrl;

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_56"));
		enemyEntity = new Enemy_56(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		_monster56AnimCtrl = (Monster56AnimCtrl)enemyAnimCtrl;
	}

	public void SetBoomRoundRemain(int index)
	{
		_monster56AnimCtrl.PlaySingleIdle(index);
	}
}
