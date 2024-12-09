using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HiddenBossBlock : BaseBlock
{
	private Transform bossLight;

	private Image groundCrack;

	private Image crackLight;

	private bool isEverInteractive;

	private Action finishAct;

	public override string HandleLoadActionName => "HandleLoad_HiddenBossBlock";

	public void ActiveHiddenBossBlock(int roomSeed)
	{
		base.RoomSeed = roomSeed;
		isEverInteractive = false;
	}

	protected override void OnClick()
	{
		if (!isEverInteractive)
		{
			UnityEngine.Random.InitState(base.RoomSeed);
			EnemyHeapData specificEnemyHeap = DataManager.Instance.GetSpecificEnemyHeap("MHeap_66");
			Singleton<GameManager>.Instance.StartBattle(new BattleSystem.HiddenBossBattleHandler(specificEnemyHeap), SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(base.transform.position));
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddSystemReportContent("开始最终Boss战斗");
			}
			EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
			isEverInteractive = true;
		}
	}

	private void OnBattleEnd(EventData data)
	{
		if (!Singleton<GameManager>.Instance.Player.IsDead)
		{
			Singleton<GameManager>.Instance.AddClearBossHeapID("MHeap_66");
		}
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	public override void ResetBlock()
	{
	}

	public void Init()
	{
		groundCrack = base.transform.Find("Crack").GetComponent<Image>();
		crackLight = base.transform.Find("CrackLight").GetComponent<Image>();
		bossLight = base.transform.Find("BossLightTrans");
		Reset();
	}

	public void StartGameWinAnim(EnemyBase dragonEnemy, Action act)
	{
		Reset();
		StopAllCoroutines();
		finishAct = act;
		StartCoroutine(StartAnimCo(dragonEnemy));
	}

	private void Reset()
	{
		groundCrack.enabled = false;
		crackLight.enabled = false;
		groundCrack.fillAmount = 0f;
	}

	private IEnumerator StartAnimCo(EnemyBase dragonEnemy)
	{
		RecycleAllEnemy(dragonEnemy);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("龙被消灭转场音效");
		VfxBase vfx = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_BossLight");
		vfx.Play();
		vfx.transform.SetParent(bossLight);
		vfx.transform.localPosition = new Vector3(0f, 0f, -5f);
		UIView room = SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI");
		room.transform.DOShakePosition(0.3f, 30f, 13);
		yield return new WaitForSeconds(1f);
		groundCrack.enabled = true;
		groundCrack.DOFillAmount(1f, 2f);
		room.transform.DOShakePosition(5f, 5f, 40, 90f, snapping: false, fadeOut: false).SetEase(Ease.InOutCubic);
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_rockfall_4");
		vfxBase.Play();
		vfxBase.transform.position = Vector3.zero;
		yield return new WaitForSeconds(2f);
		crackLight.enabled = true;
		crackLight.Fade(0f, 1f);
		yield return new WaitForSeconds(2f);
		SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, new RadialBlueEffectArgs(0.02f, 1.2f, 3, Vector3.one * 0.5f, 0.5f, 0.5f, 1.2f));
		VfxBase vfxBase2 = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_BossGlobalLight");
		vfxBase2.Play();
		vfxBase2.transform.SetParent(bossLight);
		vfxBase2.transform.localPosition = new Vector3(0f, 0f, -5f);
		MaskUI maskUi = SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI;
		yield return new WaitForSeconds(0.5f);
		vfx.Recycle();
		maskUi.ShowMask(0f, 1.5f, null);
		yield return new WaitForSeconds(2f);
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameWinUI", new Action(FinishAnim));
	}

	private void RecycleAllEnemy(EnemyBase dragonEnemy)
	{
		BattleEnvironmentManager.Instance.HideBg();
		dragonEnemy.EnemyCtrl.gameObject.SetActive(value: false);
	}

	private void FinishAnim()
	{
		finishAct?.Invoke();
		finishAct = null;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowFade(null);
	}
}
