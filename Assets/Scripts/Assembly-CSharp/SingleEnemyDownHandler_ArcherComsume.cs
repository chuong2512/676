using UnityEngine;

public class SingleEnemyDownHandler_ArcherComsume : SingleEnemyDownHandler
{
	private int hintArrowAmount;

	private bool isDrop;

	private bool isHighlight;

	public SingleEnemyDownHandler_ArcherComsume(int hintArrowAmount, bool isDrop)
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
		isHighlight = false;
		((ArcherArrowCtrl)(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDefenceAttrCtrl).CancelHighlightArrow(hintArrowAmount, isDrop);
	}
}
