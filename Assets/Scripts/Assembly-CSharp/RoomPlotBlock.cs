using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlotBlock : RoomBlock
{
	public int MinMoveY;

	public int MaxMoveY;

	private string plotCode;

	private Image iconImg;

	private Tween moveTween;

	public override string HandleLoadActionName => "HandleLoad_RoomPlotBlock";

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
	}

	private void OnEnable()
	{
		float duration = Random.Range(1f, 1.5f);
		iconImg.transform.localPosition = new Vector3(0f, MinMoveY, 0f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(iconImg.transform.DOLocalMoveY(MaxMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.Append(iconImg.transform.DOLocalMoveY(MinMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.SetLoops(-1);
		moveTween = sequence;
	}

	private void OnDisable()
	{
		if (moveTween != null && moveTween.IsActive())
		{
			moveTween.Kill();
		}
	}

	public void SetBlockPlotCode(Vector2Int pos, string plotCode)
	{
		base.BlockPosition = pos;
		this.plotCode = plotCode;
		PlotData plotData = DataManager.Instance.GetPlotData(plotCode);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(plotData.IconName, "Sprites/Plot");
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong)
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("PlotUI", plotCode, null);
			SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockPlot(plotCode);
			OnEndBlock();
		}
	}

	private void OnEndBlock()
	{
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		obj.RecycleRoomPlotBlock(base.BlockPosition);
		obj.AddEmptyBlock(base.BlockPosition, isSetSprite: false);
	}

	public override void ResetBlock()
	{
	}
}
