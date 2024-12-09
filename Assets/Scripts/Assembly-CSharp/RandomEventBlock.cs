using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventBlock : BaseBlock
{
	public int MinMoveY;

	public int MaxMoveY;

	private Image randomEventImg;

	private Tween randomEventMoveTween;

	private BaseGameEvent gameEvent;

	public override string HandleLoadActionName => "HandleLoad_EventBlock";

	protected override void OnAwake()
	{
		base.OnAwake();
		randomEventImg = base.transform.Find("Shadow/EventImg").GetComponent<Image>();
	}

	private void OnEnable()
	{
		float duration = Random.Range(1f, 1.5f);
		randomEventImg.transform.localPosition = new Vector3(0f, MinMoveY, 0f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(randomEventImg.transform.DOLocalMoveY(MaxMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.Append(randomEventImg.transform.DOLocalMoveY(MinMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.SetLoops(-1);
		randomEventMoveTween = sequence;
	}

	private void OnDisable()
	{
		if (randomEventMoveTween != null && randomEventMoveTween.IsActive())
		{
			randomEventMoveTween.Kill();
		}
	}

	public void ActiveRandomEventBlock(Vector2Int pos, int roomSeed, BaseGameEvent tmpGameEvent, bool isAutoActive)
	{
		base.BlockPosition = pos;
		gameEvent = tmpGameEvent;
		base.RoomSeed = roomSeed;
		if (isAutoActive)
		{
			StartCoroutine(ActiveRandomEventBlock_IE());
		}
	}

	protected IEnumerator ActiveRandomEventBlock_IE()
	{
		RoomUI.IsAnyBlockInteractiong = true;
		yield return new WaitForSeconds(0.8f);
		StartEvent();
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong)
		{
			StartEvent();
		}
	}

	private void StartEvent()
	{
		gameEvent.StartEvent(base.RoomSeed, ClearEventBlock);
	}

	private void ClearEventBlock()
	{
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		obj.RecycleRandomEventBlock(base.BlockPosition);
		obj.AddEmptyBlock(base.BlockPosition, isSetSprite: false);
	}

	public override void ResetBlock()
	{
	}
}
