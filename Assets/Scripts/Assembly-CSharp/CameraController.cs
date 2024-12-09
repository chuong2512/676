using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : SingletonDontDestroy<CameraController>
{
	private Camera mainCamera;

	private float originalOrthographicSize;

	private Dictionary<PostEffectType, PostEffectBase> postEffects = new Dictionary<PostEffectType, PostEffectBase>();

	private Tween cameraTween;

	[Header("AttackShakeScreen")]
	public float attackScreenDuration;

	public float attackScreenStrength = 1f;

	public int attackScreenVibrato = 10;

	public float attackScreenRandomness = 90f;

	private Coroutine CameraMoveCoroutin;

	private Vector3 camOriginalPos = Vector3.zero;

	private Coroutine CameraSizeChangeCoroutin;

	public Camera MainCamera
	{
		get
		{
			if (mainCamera.IsNull())
			{
				mainCamera = GetComponent<Camera>();
			}
			return mainCamera;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (!(SingletonDontDestroy<CameraController>.Instance != this))
		{
			PostEffectBase[] componentsInChildren = GetComponentsInChildren<PostEffectBase>();
			foreach (PostEffectBase postEffectBase in componentsInChildren)
			{
				postEffects.Add(postEffectBase.PostEffectType, postEffectBase);
				postEffectBase.Stop();
			}
			mainCamera = GetComponent<Camera>();
			originalOrthographicSize = mainCamera.orthographicSize;
		}
	}

	public void AttackShakeScreen()
	{
		if (!cameraTween.IsNull() && cameraTween.IsActive())
		{
			cameraTween.Complete();
		}
		cameraTween = base.transform.DOShakePosition(attackScreenDuration, attackScreenStrength, attackScreenVibrato, attackScreenRandomness);
	}

	public void ShakeScreeen(float duration, float strength, int vibrato, float randomness)
	{
		if (!cameraTween.IsNull() && cameraTween.IsActive())
		{
			cameraTween.Complete();
		}
		cameraTween = base.transform.DOShakePosition(duration, strength, vibrato, randomness);
	}

	public void MoveScreen(CameraMoveEffectStep moveStep)
	{
		if (camOriginalPos == Vector3.zero)
		{
			camOriginalPos = mainCamera.transform.localPosition;
		}
		mainCamera.transform.localPosition = camOriginalPos;
		if (CameraMoveCoroutin != null)
		{
			StopCoroutine(CameraMoveCoroutin);
		}
		mainCamera.orthographicSize = originalOrthographicSize;
		CameraMoveCoroutin = StartCoroutine(MoveScreenCo(moveStep));
	}

	private IEnumerator MoveScreenCo(CameraMoveEffectStep moveStep)
	{
		Vector3 endValue = camOriginalPos + moveStep.moveDir;
		mainCamera.transform.DOLocalMove(endValue, moveStep.moveTime).SetEase(moveStep.moveEase);
		yield return new WaitForSeconds(moveStep.moveTime + moveStep.durationTime);
		if (!(moveStep.recoverTime <= 0f))
		{
			mainCamera.transform.DOLocalMove(camOriginalPos, moveStep.recoverTime);
		}
	}

	public void CameraSizeChange(CameraSizeChangeEffectStep shakeStep)
	{
		if (CameraSizeChangeCoroutin != null)
		{
			StopCoroutine(CameraSizeChangeCoroutin);
		}
		mainCamera.orthographicSize = originalOrthographicSize;
		CameraSizeChangeCoroutin = StartCoroutine(CameraSizeChangCo(shakeStep));
	}

	private IEnumerator CameraSizeChangCo(CameraSizeChangeEffectStep scaleStep)
	{
		mainCamera.DOOrthoSize(originalOrthographicSize * scaleStep.targetScale, scaleStep.moveTime).SetEase(scaleStep.moveEase);
		yield return new WaitForSeconds(scaleStep.moveTime + scaleStep.holdTime);
		if (!(scaleStep.recoverTime <= 0f))
		{
			mainCamera.DOOrthoSize(originalOrthographicSize, scaleStep.recoverTime);
		}
	}

	public void ActivatePostEffect(PostEffectType effectType, RadialBlueEffectArgs args)
	{
		if (postEffects.ContainsKey(effectType))
		{
			postEffects[effectType].StartEffect(args);
			return;
		}
		throw new InvalidCastException("没有相应的屏幕特效" + effectType);
	}
}
