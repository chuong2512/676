using UnityEngine;

public class AllEnemyUpHandler_ArrowComsume : AllEnemyUpHandler
{
	private int hintArrowAmount;

	private bool isDrop;

	public AllEnemyUpHandler_ArrowComsume(int hintArrowAmount, bool isDrop)
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
