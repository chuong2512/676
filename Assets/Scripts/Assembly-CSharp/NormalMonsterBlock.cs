using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NormalMonsterBlock : BaseBlock
{
	public int MinMoveY;

	public int MaxMoveY;

	private Image normalMonsterImg;

	private Tween normalMonsterMoveTween;

	private bool isEverInteracted;

	private string enemyHeapCode;

	public override string HandleLoadActionName => "HandleLoad_NormalMonsterBlock";

	protected override void OnAwake()
	{
		base.OnAwake();
		normalMonsterImg = base.transform.Find("Shadow/NormalMonster").GetComponent<Image>();
	}

	private void OnEnable()
	{
		float duration = Random.Range(1f, 1.5f);
		normalMonsterImg.transform.localPosition = new Vector3(0f, MinMoveY, 0f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(normalMonsterImg.transform.DOLocalMoveY(MaxMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.Append(normalMonsterImg.transform.DOLocalMoveY(MinMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.SetLoops(-1);
		normalMonsterMoveTween = sequence;
	}

	private void OnDisable()
	{
		if (normalMonsterMoveTween != null && normalMonsterMoveTween.IsActive())
		{
			normalMonsterMoveTween.Kill();
		}
	}

	public void ActiveNormalMonsterBlock(Vector2Int pos, string enemyHeapCode, int roomSeed, bool isAutoActive)
	{
		base.RoomSeed = roomSeed;
		base.BlockPosition = pos;
		isEverInteracted = false;
		this.enemyHeapCode = enemyHeapCode;
		if (isAutoActive)
		{
			StartCoroutine(ActiveNormalMonsterBlock_IE());
		}
	}

	private IEnumerator ActiveNormalMonsterBlock_IE()
	{
		isEverInteracted = true;
		RoomUI.IsAnyBlockInteractiong = true;
		yield return new WaitForSeconds(0.8f);
		StartNormalMonsterBattle();
	}

	protected override void OnClick()
	{
		if (!isEverInteracted && !RoomUI.IsAnyBlockInteractiong)
		{
			StartNormalMonsterBattle();
		}
	}

	private void StartNormalMonsterBattle()
	{
		RoomUI.IsAnyBlockInteractiong = true;
		isEverInteracted = true;
		Random.InitState(base.RoomSeed);
		EnemyHeapData enemyHeapData = Singleton<EnemyController>.Instance.GetEnemyHeapData(enemyHeapCode);
		Singleton<EnemyController>.Instance.SetTriggeringEnemyHeap(enemyHeapData.EnemyHeapCode);
		Singleton<GameManager>.Instance.StartBattle(new BattleSystem.NormalBattleHandler(enemyHeapData), SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(base.transform.position));
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddSystemReportContent("开始普通怪战斗");
		}
	}

	private void OnBattleEnd(EventData data)
	{
		RoomUI.IsAnyBlockInteractiong = false;
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		obj.RecycleNormalMonsterBlock(base.BlockPosition);
		obj.AddEmptyBlock(base.BlockPosition, isSetSprite: false);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	public override void ResetBlock()
	{
	}
}
