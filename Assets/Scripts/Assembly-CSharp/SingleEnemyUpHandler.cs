using System;
using UnityEngine;

public class SingleEnemyUpHandler : PointUpHandler
{
	public override void PointUp(Vector3 pointViewRect, Action tryUseAction, Action tryCancelAction)
	{
		OnPointUp(pointViewRect);
		if (Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose != null)
		{
			Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose.EnemyCtrl.OnCancelHighlight();
			tryUseAction?.Invoke();
		}
		else
		{
			tryCancelAction?.Invoke();
		}
		BattleEnvironmentManager.Instance.HideAllEnemyHint();
	}

	protected virtual void OnPointUp(Vector3 pointViewRect)
	{
	}
}
