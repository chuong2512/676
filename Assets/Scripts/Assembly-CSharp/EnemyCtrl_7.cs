public class EnemyCtrl_7 : EnemyBaseCtrl
{
	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_7"));
		enemyEntity = new Enemy_7(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}
}
