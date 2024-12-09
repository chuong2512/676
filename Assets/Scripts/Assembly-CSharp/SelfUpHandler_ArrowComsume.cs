using UnityEngine;

public class SelfUpHandler_ArrowComsume : SelfUpHandler
{
	private int hintArrowAmount;

	private bool isDrop;

	public SelfUpHandler_ArrowComsume(int hintArrowAmount, bool isDrop)
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
