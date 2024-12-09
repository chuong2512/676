using UnityEngine;

public class Enemy_999 : EnemyBase
{
	protected class Enemy999_AtkMean : EnemyMean
	{
		private MeanHandler meanHandler;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy999_AtkMean(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			meanHandler = new AttackMeanHandler(RealDmg(), 1);
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1] { meanHandler });
		}

		public override void OnLogic()
		{
			string atkDes;
			int num = RealDmg(out atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，对玩家造1次{num}的非真实伤害({atkDes})");
			}
			LogicAction();
		}

		private int RealDmg()
		{
			return Mathf.FloorToInt((float)(6 + thisEnemy.PowerBuff) * (1f + thisEnemy.PowUpRate()));
		}

		private int RealDmg(out string atkDes)
		{
			atkDes = thisEnemy.GetEnemyAtkDes("6", out var pwdBuff, out var rate);
			return Mathf.FloorToInt((float)(6 + pwdBuff) * (1f + rate));
		}

		private void LogicAction()
		{
			Singleton<GameManager>.Instance.Player.TakeDamage(RealDmg(), thisEnemy, isAbsDmg: false);
		}
	}

	private EnemyCtrl_999 enemy999Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_999(EnemyAttr attr, EnemyCtrl_999 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy999Ctrl = enemyCtrl;
		currentEnemyMean = new Enemy999_AtkMean(this);
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		return null;
	}

	public override void StartBattleAction()
	{
		currentEnemyMean.OnLogic();
		if (currentEnemyMean.ItWillBreakDefence)
		{
			TryRemoveDefence();
		}
		currentEnemyMean.OnSetMean();
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}
}
