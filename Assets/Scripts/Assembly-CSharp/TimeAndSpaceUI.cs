using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndSpaceUI : UIView
{
	private CanvasGroup cg;

	private Image startField;

	private VfxBase vfx;

	private Action finishAct;

	public override string UIViewName => "TimeAndSpaceUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		cg.alpha = 0f;
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("普通通关动画音效");
		vfx = Singleton<VfxManager>.Instance.LoadVfx("effect_general_rockfall_3");
		vfx.Play();
		vfx.transform.position = Vector3.zero;
		startField.color = new Color(1f, 1f, 1f, 0f);
		yield return new WaitForSeconds(1f);
		SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI").transform.DOShakePosition(5f, 5f, 30, 1f, snapping: false, fadeOut: false);
		yield return new WaitForSeconds(1.5f);
		cg.Fade(0f, 1f, 0.8f);
		yield return new WaitForSeconds(0.5f);
		startField.DOFade(1f, 0.3f);
		vfx.Recycle();
		vfx = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_shikong");
		vfx.Play();
		vfx.transform.position = Vector3.zero;
		yield return new WaitForSeconds(3f);
		SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, new RadialBlueEffectArgs(0.02f, 0.6f, 3, Vector3.zero, 0.3f, 0.3f, 0f));
		yield return new WaitForSeconds(0.3f);
		base.transform.DOShakePosition(0.2f, 15f, 20, 1f, snapping: false, fadeOut: false);
		cg.DOFade(0f, 0.3f);
		finishAct?.Invoke();
		base.gameObject.SetActive(value: false);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		cg = GetComponent<CanvasGroup>();
		startField = base.transform.Find("Mask/StarField").GetComponent<Image>();
	}

	public void SetFinishAct(Action action)
	{
		finishAct = action;
	}
}
