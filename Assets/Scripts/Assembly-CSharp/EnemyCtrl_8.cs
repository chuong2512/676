public class EnemyCtrl_8 : EnemyBaseCtrl
{
	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_8"));
		enemyEntity = new Enemy_8(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}
}
