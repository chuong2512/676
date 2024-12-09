public class EnemyCtrl_9 : EnemyBaseCtrl
{
	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_9"));
		enemyEntity = new Enemy_9(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}
}
