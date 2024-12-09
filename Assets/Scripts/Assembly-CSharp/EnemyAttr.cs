public class EnemyAttr : EntityAttr
{
	public string EnemyCode;

	public string NameKey;

	public string DesKey;

	private EnemyBase enemyBase;

	public int Block { get; protected set; }

	public int BaseBlock { get; set; }

	public void StartBattle()
	{
		Block = BaseBlock;
		Health = MaxHealth;
		base.Armor = base.BaseArmor;
	}

	public EnemyAttr(EnemyData enemyData)
	{
		MaxHealth = enemyData.MaxHealth;
		base.BaseArmor = enemyData.BaseArmor;
		BaseBlock = enemyData.BaseBlock;
		EnemyCode = enemyData.EnemyCode;
		NameKey = enemyData.NameKey;
		DesKey = enemyData.DesKey;
	}

	public void SetEnemyBase(EnemyBase enemyBase)
	{
		this.enemyBase = enemyBase;
	}

	protected override void OnHealthChanged(bool isAdd)
	{
		base.OnHealthChanged(isAdd);
		if (enemyBase.IsDead)
		{
			return;
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{enemyBase.EntityName}的生命值发生变化，当前护甲：{base.Armor}");
		}
		Health = ((Health >= 0) ? Health : 0);
		if (Health <= 0)
		{
			Health = 0;
			enemyBase.EnemyCtrl.UpdateHealth(Health, MaxHealth);
			enemyBase.Dead();
		}
		else
		{
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig(isAdd ? "Usual_HealthRecovery_Enemy_EffectConfig" : "Usual_HealthReduce_Enemy_EffectConfig", enemyBase.EntityTransform, null, delegate
			{
				enemyBase.EnemyCtrl.UpdateHealth(Health, MaxHealth);
			});
		}
	}

	protected override void OnArmorChanged(bool isAdd)
	{
		base.OnArmorChanged(isAdd);
		Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig(isAdd ? "Usual_AddArmor_Enemy_EffectConfig" : "Usual_ReduceArmor_Enemy_EffectConfig", enemyBase.ArmorTrans, null, delegate
		{
			enemyBase.EnemyCtrl.UpdateArmor(base.Armor);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{enemyBase.EntityName}的护甲发生变化，当前护甲：{base.Armor}");
			}
		});
	}
}
