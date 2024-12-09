using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EliteMonsterBlock : BaseBlock
{
	public int MinMoveY;

	public int MaxMoveY;

	private Image eliteMonsterImg;

	private Tween eliteMonsterMoveTween;

	private bool isEverInteracted;

	private string enemyHeapCode;

	public override string HandleLoadActionName => "HandleLoad_EliteMonsterBlock";

	protected override void OnAwake()
	{
		base.OnAwake();
		eliteMonsterImg = base.transform.Find("Shadow/EliteMonster").GetComponent<Image>();
	}

	private void OnEnable()
	{
		float duration = Random.Range(1f, 1.5f);
		Sequence sequence = DOTween.Sequence();
		eliteMonsterImg.transform.localPosition = new Vector3(0f, MinMoveY, 0f);
		sequence.Append(eliteMonsterImg.transform.DOLocalMoveY(MaxMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.Append(eliteMonsterImg.transform.DOLocalMoveY(MinMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.SetLoops(-1);
		eliteMonsterMoveTween = sequence;
	}

	private void OnDisable()
	{
		if (eliteMonsterMoveTween != null && eliteMonsterMoveTween.IsActive())
		{
			eliteMonsterMoveTween.Kill();
		}
	}

	public void ActiveEliteMonsterBlock(Vector2Int pos, string enemyHeapCode, int roomSeed, bool isAutoActive)
	{
		base.RoomSeed = roomSeed;
		base.BlockPosition = pos;
		this.enemyHeapCode = enemyHeapCode;
		isEverInteracted = false;
		if (isAutoActive)
		{
			StartCoroutine(ActiveEliteMonsterBlock_IE());
		}
	}

	private IEnumerator ActiveEliteMonsterBlock_IE()
	{
		isEverInteracted = true;
		RoomUI.IsAnyBlockInteractiong = true;
		yield return new WaitForSeconds(0.8f);
		StartEliteMonsterBattle();
	}

	protected override void OnClick()
	{
		if (!isEverInteracted && !RoomUI.IsAnyBlockInteractiong)
		{
			StartEliteMonsterBattle();
		}
	}

	private void StartEliteMonsterBattle()
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
			gameReportUI.AddSystemReportContent("开始精英怪战斗");
		}
	}

	private void OnBattleEnd(EventData data)
	{
		RoomUI.IsAnyBlockInteractiong = false;
		RoomUI obj = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		base.gameObject.SetActive(value: false);
		obj.AddEmptyBlock(base.BlockPosition, isSetSprite: false);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	public override void ResetBlock()
	{
	}
}
