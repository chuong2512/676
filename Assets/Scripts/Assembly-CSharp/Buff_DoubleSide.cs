using UnityEngine;

public class Buff_DoubleSide : BaseBuff
{
	private Enemy_40 targetEnemy;

	private const string BuffEffectConfigName_SideA = "Buff_DoubleSideA_Enemy";

	private const string BuffEffectConfigName_SideB = "Buff_DoubleSideB_Enemy";

	public override BuffType BuffType => BuffType.Buff_DoubleSide;

	public Buff_DoubleSide(Enemy_40 entityBase, int round)
		: base(entityBase, round)
	{
		targetEnemy = entityBase;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadConfigByName(targetEnemy.IsSideA ? "Buff_DoubleSideB_Enemy" : "Buff_DoubleSideA_Enemy"), buffIconCtrl.transform, new Transform[1] { targetEnemy.EnemyCtrl.transform }, Effect);
	}

	private void Effect()
	{
		if (targetEnemy.SwitchTime > 0)
		{
			if (targetEnemy.IsSideA)
			{
				int num = targetEnemy.SwitchTime * 10;
				targetEnemy.EntityAttr.RecoveryHealth(num);
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent($"{targetEnemy}被动触发：恢复{num}点生命值");
				}
			}
			else
			{
				GameReportUI gameReportUI2 = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI2 != null)
				{
					gameReportUI2.AddGameReportContent($"{targetEnemy}被动触发：提升自己{targetEnemy.SwitchTime}点的力量");
				}
				targetEnemy.GetBuff(new Buff_Power(targetEnemy, targetEnemy.SwitchTime));
			}
		}
		targetEnemy.ResetEnemySwitchTime();
	}

	public void UpdateBuffHint()
	{
		buffIconCtrl.UpdateBuff();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + targetEnemy.SwitchTime + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return targetEnemy.SwitchTime;
	}
}
