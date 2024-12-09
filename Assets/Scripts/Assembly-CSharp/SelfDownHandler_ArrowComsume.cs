using UnityEngine;

public class SelfDownHandler_ArrowComsume : SelfDownHandler
{
	private int hintArrowAmount;

	private bool isDrop;

	private bool isHighlight;

	public SelfDownHandler_ArrowComsume(int hintArrowAmount, bool isDrop)
	{
		this.hintArrowAmount = hintArrowAmount;
		this.isDrop = isDrop;
	}

	protected override void OnHandleDown(Vector3 pointViewRect)
	{
		base.OnHandleDown(pointViewRect);
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		if (!isHighlight)
		{
			isHighlight = true;
			((ArcherArrowCtrl)battleUI.PlayerDefenceAttrCtrl).HighlightArrow(hintArrowAmount, isDrop);
		}
	}

	public override void HandleEndCastHit()
	{
		base.HandleEndCastHit();
		if (isHighlight)
		{
			((ArcherArrowCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl).CancelHighlightArrow(hintArrowAmount, isDrop);
		}
		isHighlight = false;
	}
}
