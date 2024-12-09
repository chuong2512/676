using System;
using System.Collections;
using UnityEngine;

public class TempEffectManager : Singleton<TempEffectManager>
{
	public void HandleEffectConfing(BaseEffectConfig config, Transform caster, Action activeAction)
	{
		if (config.IsNull() || config.allStep.IsNull() || config.allStep.Length == 0)
		{
			activeAction?.Invoke();
			return;
		}
		Array.Sort(config.allStep);
		StartCoroutine(EffectConfig_IE(config, caster, activeAction));
	}

	private IEnumerator EffectConfig_IE(BaseEffectConfig config, Transform caster, Action activeAction)
	{
		StartCoroutine(EffectActive_IE(config.effecTime, activeAction));
		float time = config.allStep[0].waitTime;
		float preTime = time;
		int i = 0;
		while (i < config.allStep.Length)
		{
			if (time > 0f)
			{
				yield return new WaitForSeconds(time);
			}
			if (i < config.allStep.Length - 1)
			{
				time = config.allStep[i + 1].waitTime - preTime;
				preTime = config.allStep[i + 1].waitTime;
			}
			typeof(TempEffectManager).GetMethod("Handle" + config.allStep[i].EffectConfigType.ToString() + "Step").Invoke(this, new object[2]
			{
				config.allStep[i],
				caster
			});
			int num = i + 1;
			i = num;
		}
	}

	public void HandleScreenDisdortionStep(BaseEffectStep step, Transform caster)
	{
		ScreenDisdortionEffectStep screenDisdortionEffectStep;
		if ((screenDisdortionEffectStep = step as ScreenDisdortionEffectStep) != null)
		{
			Vector3 position = caster.transform.position;
			RadialBlueEffectArgs args = new RadialBlueEffectArgs(screenDisdortionEffectStep.blurFactor, screenDisdortionEffectStep.lerpFactor, screenDisdortionEffectStep.downSampleFactor, Camera.main.WorldToViewportPoint(position), screenDisdortionEffectStep.inTime, screenDisdortionEffectStep.recoverTime, screenDisdortionEffectStep.holdTime);
			SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, args);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraShakeEffectStep");
	}

	public void HandleAddVfxStep(BaseEffectStep step, Transform caster)
	{
		AddVfxEffectStep addVfxEffectStep;
		if ((addVfxEffectStep = step as AddVfxEffectStep) != null)
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxEffectStep.vfxName);
			vfxBase.transform.position = caster.position + addVfxEffectStep.offset;
			vfxBase.Play(addVfxEffectStep.mute);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to AddVfxEffectStep");
	}

	public void HandleAddMoveVfxStep(BaseEffectStep step, Transform caster)
	{
		AddVfxEffectStep addVfxEffectStep;
		if ((addVfxEffectStep = step as AddVfxEffectStep) != null)
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxEffectStep.vfxName);
			vfxBase.transform.position = caster.position + addVfxEffectStep.offset;
			vfxBase.Play(addVfxEffectStep.mute);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to AddVfxEffectStep");
	}

	public void HandleCameraMoveStep(BaseEffectStep step, Transform caster)
	{
		CameraMoveEffectStep moveStep;
		if ((moveStep = step as CameraMoveEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.MoveScreen(moveStep);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraMoveEffectStep");
	}

	public void HandleCameraShakeStep(BaseEffectStep step, Transform caster)
	{
		CameraShakeEffectStep cameraShakeEffectStep;
		if ((cameraShakeEffectStep = step as CameraShakeEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.ShakeScreeen(cameraShakeEffectStep.shakeDuration, cameraShakeEffectStep.shakeStrength, cameraShakeEffectStep.shakeVibrato, cameraShakeEffectStep.shakeRandomness);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraShakeEffectStep");
	}

	public void HandleCameraSizeChangeStep(BaseEffectStep step, Transform caster)
	{
		CameraSizeChangeEffectStep shakeStep;
		if ((shakeStep = step as CameraSizeChangeEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.CameraSizeChange(shakeStep);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraSizeChangeEffectStep");
	}

	public void HandleScreenColorChangeStep(BaseEffectStep step, Transform caster)
	{
		ScreenColorChangeEffectStep screenColorChangeEffectStep;
		if ((screenColorChangeEffectStep = step as ScreenColorChangeEffectStep) != null)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectUI") as ScreenEffectUI).ChangeScreenColor(screenColorChangeEffectStep.targetColor, screenColorChangeEffectStep.Ease, screenColorChangeEffectStep.changeTime, screenColorChangeEffectStep.durationTime, screenColorChangeEffectStep.recoveryTime);
			return;
		}
		throw new InvalidCastException("Cannot cast step type to ScreenColorChangeEffectStep");
	}

	private IEnumerator EffectActive_IE(float time, Action activeAction)
	{
		yield return new WaitForSeconds(time);
		activeAction?.Invoke();
	}
}
