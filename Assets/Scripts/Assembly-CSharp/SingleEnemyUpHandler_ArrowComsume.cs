using UnityEngine;

public class SingleEnemyUpHandler_ArrowComsume : SingleEnemyUpHandler
{
	private int hintArrowAmount;

	private bool isDrop;

	public SingleEnemyUpHandler_ArrowComsume(int hintArrowAmount, bool isDrop)
	{
		this.hintArrowAmount = hintArrowAmount;
		this.isDrop = isDrop;
	}

	protected override void OnPointUp(Vector3 pointViewRect)
	{
		base.OnPointUp(pointViewRect);
		((ArcherArrowCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl).CancelHighlightArrow(hintArrowAmount, isDrop);
	}
}
