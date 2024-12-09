using UnityEngine;

public class AllEnemyDownHandler_ArrowComsume : AllEnemyDownHandler
{
	protected int hintArrowAmount;

	protected bool isDrop;

	protected bool isHighlight;

	public AllEnemyDownHandler_ArrowComsume(int hintArrowAmount, bool isDrop)
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
