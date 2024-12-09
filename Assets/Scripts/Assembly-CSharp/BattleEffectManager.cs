using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

public class BattleEffectManager : Singleton<BattleEffectManager>
{
	private Dictionary<string, MethodInfo> allEffectMethodInfos = new Dictionary<string, MethodInfo>();

	private const string USUAL_BATTLE_EFFECT_PATH = "EffectConfigScriObj/Usual";

	public void HandleUsualEffectConfig(string configName, Transform caster, Transform[] targets, Action activeAction)
	{
		BaseEffectConfig config = SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(configName, "EffectConfigScriObj/Usual");
		HandleEffectConfig(config, caster, targets, activeAction);
	}

	public void HandleEffectConfig(BaseEffectConfig config, Transform caster, Transform[] targets, Action activeAction)
	{
		if (config.IsNull())
		{
			activeAction?.Invoke();
			return;
		}
		if (!config.allStep.IsNull() && config.allStep.Length != 0)
		{
			Array.Sort(config.allStep);
			StartCoroutine(EffectConfig_IE(config, caster, targets));
		}
		if (config.effecTime > 0f)
		{
			StartCoroutine(TimeCounter_IE(config.effecTime, activeAction));
		}
		else
		{
			activeAction?.Invoke();
		}
	}

	private IEnumerator EffectConfig_IE(BaseEffectConfig config, Transform caster, Transform[] targets)
	{
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
			string key = "Handle" + config.allStep[i].EffectConfigType.ToString() + "Step";
			if (!allEffectMethodInfos.TryGetValue(key, out var value))
			{
				value = typeof(BattleEffectManager).GetMethod(key);
				allEffectMethodInfos.Add(key, value);
			}
			value.Invoke(this, new object[3]
			{
				config.allStep[i],
				caster,
				targets
			});
			int num = i + 1;
			i = num;
		}
	}

	public void HandleEffectConfig(BaseEffectConfig config, Transform caster, Transform[] targets, Action activeAction, Action endAction)
	{
		if (config.IsNull() || config.allStep.IsNull() || config.allStep.Length == 0)
		{
			activeAction?.Invoke();
			endAction?.Invoke();
			return;
		}
		Array.Sort(config.allStep);
		if (activeAction != null)
		{
			StartCoroutine(TimeCounter_IE(config.effecTime, activeAction));
		}
		if (endAction != null)
		{
			StartCoroutine(TimeCounter_IE(config.durationTime, endAction));
		}
		StartCoroutine(EffectConfig_IE(config, caster, targets));
	}

	public void HandleScreenDisdortionStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		ScreenDisdortionEffectStep screenDisdortionEffectStep;
		if ((screenDisdortionEffectStep = step as ScreenDisdortionEffectStep) != null)
		{
			Vector3 zero = Vector3.zero;
			if (targets != null)
			{
				foreach (Transform transform in targets)
				{
					zero += transform.position;
				}
				zero /= (float)targets.Length;
			}
			RadialBlueEffectArgs args = new RadialBlueEffectArgs(screenDisdortionEffectStep.blurFactor, screenDisdortionEffectStep.lerpFactor, screenDisdortionEffectStep.downSampleFactor, Camera.main.WorldToViewportPoint(zero), screenDisdortionEffectStep.inTime, screenDisdortionEffectStep.recoverTime, screenDisdortionEffectStep.holdTime);
			SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, args);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraShakeEffectStep");
	}

	public void HandleAddVfxStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		AddVfxEffectStep addVfxEffectStep;
		if ((addVfxEffectStep = step as AddVfxEffectStep) != null)
		{
			List<VfxBase> list = null;
			BattleUI battleUi = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			switch (addVfxEffectStep.castType)
			{
			case AddVfxEffectStep.VfxCastType.SingleTarget:
				list = AddVfx_ProcessSingleTarget(addVfxEffectStep, caster, targets);
				break;
			case AddVfxEffectStep.VfxCastType.AllTarget:
				list = AddVfx_ProcessAllTargets(addVfxEffectStep, caster, targets);
				break;
			case AddVfxEffectStep.VfxCastType.ScreenCentre:
				list = AddVfx_ProcessScreenCentre(addVfxEffectStep);
				break;
			case AddVfxEffectStep.VfxCastType.Player:
				list = AddVfx_ProcessPlayer(addVfxEffectStep, battleUi);
				break;
			case AddVfxEffectStep.VfxCastType.PlayerApPt:
				list = AddVfx_ProcessPlayerApPt(addVfxEffectStep, battleUi);
				break;
			case AddVfxEffectStep.VfxCastType.PlayerFaithPt:
				list = AddVfx_ProcessPlayerFaithPt(addVfxEffectStep, battleUi);
				break;
			case AddVfxEffectStep.VfxCastType.Self:
				list = AddVfx_ProcessSelf(addVfxEffectStep, caster);
				break;
			case AddVfxEffectStep.VfxCastType.AllExistEnemy:
				list = AddVfx_ProcessAllExistEnemy(addVfxEffectStep);
				break;
			case AddVfxEffectStep.VfxCastType.ArmorPt:
				list = AddVfx_ArmorPt(caster, addVfxEffectStep);
				break;
			}
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					float x = ((addVfxEffectStep.randomOffset.x > 0f) ? UnityEngine.Random.Range(0f - addVfxEffectStep.randomOffset.x, addVfxEffectStep.randomOffset.x) : 0f);
					float y = ((addVfxEffectStep.randomOffset.y > 0f) ? UnityEngine.Random.Range(0f - addVfxEffectStep.randomOffset.y, addVfxEffectStep.randomOffset.y) : 0f);
					list[i].transform.position += (addVfxEffectStep.offset + new Vector3(x, y, 0f)) * UIManager.WorldScale;
				}
			}
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to AddVfxEffectStep");
	}

	private List<VfxBase> AddVfx_ArmorPt(Transform target, AddVfxEffectStep addVfxEffect)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxEffect.vfxName);
		vfxBase.transform.position = target.position;
		vfxBase.Play(addVfxEffect.mute);
		if (addVfxEffect.isFollow)
		{
			vfxBase.transform.SetParent(target, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessAllExistEnemy(AddVfxEffectStep addVfxStep)
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		List<VfxBase> list = new List<VfxBase>(allEnemies.Count);
		for (int i = 0; i < allEnemies.Count; i++)
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
			vfxBase.transform.position = allEnemies[i].EnemyCtrl.transform.position;
			vfxBase.Play(addVfxStep.mute);
			if (addVfxStep.isFollow)
			{
				vfxBase.transform.SetParent(allEnemies[i].EnemyCtrl.transform, worldPositionStays: true);
			}
			list.Add(vfxBase);
		}
		return list;
	}

	private List<VfxBase> AddVfx_ProcessSingleTarget(AddVfxEffectStep addVfxStep, Transform caster, Transform[] targets)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		vfxBase.transform.position = targets[0].position;
		vfxBase.Play(addVfxStep.mute);
		if (addVfxStep.isFollow)
		{
			vfxBase.transform.SetParent(targets[0].transform, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessAllTargets(AddVfxEffectStep addVfxStep, Transform caster, Transform[] targets)
	{
		List<VfxBase> list = new List<VfxBase>(targets.Length);
		for (int i = 0; i < targets.Length; i++)
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
			vfxBase.transform.position = targets[i].position;
			vfxBase.Play(addVfxStep.mute);
			if (addVfxStep.isFollow)
			{
				vfxBase.transform.SetParent(targets[i].transform, worldPositionStays: true);
			}
			list.Add(vfxBase);
		}
		return list;
	}

	private List<VfxBase> AddVfx_ProcessScreenCentre(AddVfxEffectStep addVfxStep)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		Vector3 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		vfxBase.transform.position = new Vector3(vector.x, vector.y, 0f);
		vfxBase.Play(addVfxStep.mute);
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessPlayer(AddVfxEffectStep addVfxStep, BattleUI battleUi)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		vfxBase.transform.position = battleUi.PlayerHeadProtraitTrans.position.WithV3(null, null, 0f);
		vfxBase.Play(addVfxStep.mute);
		if (addVfxStep.isFollow)
		{
			vfxBase.transform.SetParent(battleUi.PlayerHeadProtraitTrans, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessPlayerApPt(AddVfxEffectStep addVfxStep, BattleUI battleUi)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		vfxBase.transform.position = battleUi.PlayerApImgTrans.position.WithV3(null, null, 0f);
		vfxBase.Play(addVfxStep.mute);
		if (addVfxStep.isFollow)
		{
			vfxBase.transform.SetParent(battleUi.PlayerApImgTrans, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessPlayerFaithPt(AddVfxEffectStep addVfxStep, BattleUI battleUi)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		vfxBase.transform.position = battleUi.PlayerSpecialAttrImgTrans.position.WithV3(null, null, 0f);
		vfxBase.Play(addVfxStep.mute);
		if (addVfxStep.isFollow)
		{
			vfxBase.transform.SetParent(battleUi.PlayerSpecialAttrImgTrans, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	private List<VfxBase> AddVfx_ProcessSelf(AddVfxEffectStep addVfxStep, Transform caster)
	{
		if (caster == null)
		{
			return null;
		}
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addVfxStep.vfxName);
		vfxBase.transform.position = caster.position.WithV3(null, null, 0f);
		vfxBase.Play(addVfxStep.mute);
		if (addVfxStep.isFollow)
		{
			vfxBase.transform.SetParent(caster, worldPositionStays: true);
		}
		return new List<VfxBase>(1) { vfxBase };
	}

	public void HandleCameraMoveStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		CameraMoveEffectStep moveStep;
		if ((moveStep = step as CameraMoveEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.MoveScreen(moveStep);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraMoveEffectStep");
	}

	public void HandleCameraShakeStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		CameraShakeEffectStep cameraShakeEffectStep;
		if ((cameraShakeEffectStep = step as CameraShakeEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.ShakeScreeen(cameraShakeEffectStep.shakeDuration, cameraShakeEffectStep.shakeStrength, cameraShakeEffectStep.shakeVibrato, cameraShakeEffectStep.shakeRandomness);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraShakeEffectStep");
	}

	public void HandleCameraSizeChangeStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		CameraSizeChangeEffectStep shakeStep;
		if ((shakeStep = step as CameraSizeChangeEffectStep) != null)
		{
			SingletonDontDestroy<CameraController>.Instance.CameraSizeChange(shakeStep);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to CameraSizeChangeEffectStep");
	}

	public void HandleScreenColorChangeStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		ScreenColorChangeEffectStep screenColorChangeEffectStep;
		if ((screenColorChangeEffectStep = step as ScreenColorChangeEffectStep) != null)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectUI") as ScreenEffectUI).ChangeScreenColor(screenColorChangeEffectStep.targetColor, screenColorChangeEffectStep.Ease, screenColorChangeEffectStep.changeTime, screenColorChangeEffectStep.durationTime, screenColorChangeEffectStep.recoveryTime);
			return;
		}
		throw new InvalidCastException("Cannot cast step type to ScreenColorChangeEffectStep");
	}

	public void HandleAddMoveVfxStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		AddMoveVfxEffectStep addMoveVfxEffectStep;
		if ((addMoveVfxEffectStep = step as AddMoveVfxEffectStep) != null)
		{
			int num = ((addMoveVfxEffectStep.startPos != AddMoveVfxEffectStep.PosType.AllEnemy && addMoveVfxEffectStep.endPos != AddMoveVfxEffectStep.PosType.AllEnemy) ? 1 : targets.Length);
			for (int i = 0; i < num; i++)
			{
				Vector3 startPoint = Vector3.zero;
				Vector3 endPoint = Vector3.zero;
				BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
				switch (addMoveVfxEffectStep.startPos)
				{
				case AddMoveVfxEffectStep.PosType.BuffIcon:
					startPoint = caster.position;
					break;
				case AddMoveVfxEffectStep.PosType.ApPoint:
					startPoint = battleUI.PlayerApImgTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.FaithPoint:
					startPoint = battleUI.PlayerSpecialAttrImgTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.EnemyItSelf:
					startPoint = caster.position;
					break;
				case AddMoveVfxEffectStep.PosType.PlayerHeadPortrait:
					startPoint = battleUI.PlayerHeadProtraitTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.PlayerHealthPoint:
					startPoint = battleUI.PlayerHealthTransform.position;
					break;
				case AddMoveVfxEffectStep.PosType.AllEnemy:
					startPoint = targets[i].position;
					break;
				}
				startPoint += addMoveVfxEffectStep.startPosOffset;
				switch (addMoveVfxEffectStep.endPos)
				{
				case AddMoveVfxEffectStep.PosType.BuffIcon:
					endPoint = caster.position;
					break;
				case AddMoveVfxEffectStep.PosType.ApPoint:
					endPoint = battleUI.PlayerApImgTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.FaithPoint:
					endPoint = battleUI.PlayerSpecialAttrImgTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.EnemyItSelf:
					endPoint = targets[0].position;
					break;
				case AddMoveVfxEffectStep.PosType.PlayerHeadPortrait:
					endPoint = battleUI.PlayerHeadProtraitTrans.position;
					break;
				case AddMoveVfxEffectStep.PosType.PlayerHealthPoint:
					endPoint = battleUI.PlayerHealthTransform.position;
					break;
				case AddMoveVfxEffectStep.PosType.AllEnemy:
					endPoint = targets[i].position;
					break;
				}
				endPoint += addMoveVfxEffectStep.endPosOffset;
				startPoint.z = -1f;
				endPoint.z = -1f;
				float num2 = Mathf.Max(4f, Mathf.Abs(startPoint.x - endPoint.x)) / 2f;
				float num3 = Mathf.Max(4f, Mathf.Abs(startPoint.y - endPoint.y)) / 2f;
				float x = Mathf.Lerp(startPoint.x - num2, endPoint.x + num2, UnityEngine.Random.Range(0f, 1f));
				float y = Mathf.Lerp(startPoint.y - num3, endPoint.y - num3, UnityEngine.Random.Range(0f, 1f));
				Vector3 middlePoint = new Vector3(x, y, startPoint.z);
				VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(addMoveVfxEffectStep.vfxName);
				vfxBase.Play();
				vfxBase.transform.TransformMoveByBezier(startPoint, middlePoint, endPoint, addMoveVfxEffectStep.duration, delegate
				{
					vfxBase.Recycle();
				});
			}
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to AddVfxEffectStep");
	}

	private IEnumerator TimeCounter_IE(float time, Action activeAction)
	{
		yield return new WaitForSeconds(time);
		activeAction?.Invoke();
	}

	public void HandleJumpStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		JumpEffectStep jumpEffectStep;
		if ((jumpEffectStep = step as JumpEffectStep) != null)
		{
			float initY = caster.position.y;
			caster.DOMoveY(initY + jumpEffectStep.jumpHeight, jumpEffectStep.jumpUpTime).OnComplete(delegate
			{
				caster.DOMoveY(initY, jumpEffectStep.jumpDowmTime);
			});
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to JumpEffectStep");
	}

	public void HandleEnemySkeletonAnimStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		EnemySkeletonAnimEffectStep enemySkeletonAnimEffectStep;
		if ((enemySkeletonAnimEffectStep = step as EnemySkeletonAnimEffectStep) != null)
		{
			caster.GetComponentInChildren<MonsterAnimCtrlBase>().PlaySkeletonAnim(enemySkeletonAnimEffectStep.animList);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to SkeletonAnimEffectStep");
	}

	public void HandleEnemySkeletonSkinStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		EnemySkeletonChangeSkinEffectStep enemySkeletonChangeSkinEffectStep;
		if ((enemySkeletonChangeSkinEffectStep = step as EnemySkeletonChangeSkinEffectStep) != null)
		{
			caster.GetComponentInChildren<MonsterAnimCtrlBase>().ChangeSkin(enemySkeletonChangeSkinEffectStep.animSkinName);
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to EnemySkeletonChangeSkinEffectStep");
	}

	public void HandlePlaySoundStep(BaseEffectStep step, Transform caster, Transform[] targets)
	{
		PlaySoundEffectStep playSoundEffectStep;
		if ((playSoundEffectStep = step as PlaySoundEffectStep) != null && !playSoundEffectStep.soundName.IsNullOrEmpty())
		{
			if (!playSoundEffectStep.isLoop)
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound(playSoundEffectStep.soundName);
			}
			else
			{
				SingletonDontDestroy<AudioManager>.Instance.PlayerSound_Loop(playSoundEffectStep.soundName);
			}
			return;
		}
		throw new InvalidCastException("Cannot cast steptype to PlaySoundEffectStep");
	}
}
