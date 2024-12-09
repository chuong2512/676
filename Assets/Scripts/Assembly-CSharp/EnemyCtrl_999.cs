public class EnemyCtrl_999 : EnemyBaseCtrl
{
	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_999"));
		enemyEntity = new Enemy_999(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}
}
