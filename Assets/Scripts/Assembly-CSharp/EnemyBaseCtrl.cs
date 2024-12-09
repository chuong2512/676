using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyBaseCtrl : MonoBehaviour
{
	public int actionIndex;

	public static bool LockEnemyMean = false;

	protected const float StepForwardTargetScale = 1.1f;

	protected const float StepForwardAnimTime = 0.2f;

	protected const float StepBackAnimTime = 0.2f;

	protected const float StepAnimWaitTime = 0.3f;

	protected const float StepBackWaitTime = 0.2f;

	public const float NextEnemyWaitTime = 0.4f;

	public float EnemySize = 1f;

	public Transform ShadowTrans;

	public Vector2 hintOffset = Vector2.zero;

	[Header("ShowAndDeadEffectConfig")]
	public string ShowEffectConfig;

	public string DeadEffectConfig;

	protected EnemyBase enemyEntity;

	protected HealthBarCtrl healthBarCtrl;

	protected Transform meanRoot;

	protected MonsterAnimCtrlBase enemyAnimCtrl;

	protected BoxCollider2D boxCollider2D;

	protected bool isEnemyActionAnim;

	private static readonly List<string> IgnoreTags = new List<string> { "WorldCanvasUI", "IgnoreCollider" };

	private bool isHighlight;

	private static Queue<MeanIconCtrl> meanIconPool = new Queue<MeanIconCtrl>();

	private List<MeanIconCtrl> showingMeanIcons = new List<MeanIconCtrl>();

	private List<MeanHandler> allMeans = new List<MeanHandler>();

	public EnemyBase EnemyEntity => enemyEntity;

	public HealthBarCtrl HealthBarCtrl => healthBarCtrl;

	public virtual Transform HealthBarTransform => healthBarCtrl.transform;

	public Transform MeanRoot => meanRoot;

	public BoxCollider2D BoxCollider2D => boxCollider2D;

	private void Awake()
	{
		InitComponent();
	}

	private void OnEnable()
	{
		OnEnableMeanRootAnim();
	}

	private void OnDisable()
	{
		if (isHighlight)
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("EnemyMeanHintUI");
		}
	}

	protected virtual void OnEnableMeanRootAnim()
	{
		enemyAnimCtrl.ChangeBlackColor(Color.black);
		enemyAnimCtrl.ChangeTintColor(Color.white);
	}

	protected virtual void InitComponent()
	{
		meanRoot = base.transform.Find("MeanRoot");
		boxCollider2D = GetComponent<BoxCollider2D>();
		healthBarCtrl = base.transform.Find("HealthBar").GetComponent<HealthBarCtrl>();
		Transform transform = base.transform.Find("EnemyAnim");
		ShadowTrans = base.transform.Find("Shadow");
		if (!transform.IsNull())
		{
			enemyAnimCtrl = transform.GetComponent<MonsterAnimCtrlBase>();
			enemyAnimCtrl.Init();
			StartCoroutine(InitAnimCtrl_IE());
		}
	}

	protected IEnumerator InitAnimCtrl_IE()
	{
		while (!enemyAnimCtrl.isInitOver)
		{
			yield return null;
		}
		enemyAnimCtrl.IdleAnim();
	}

	public void StartBattle()
	{
		InitHealthBar();
		base.transform.localScale = Vector3.one;
		isHighlight = true;
		StartCoroutine(CannotHighligh_IE());
	}

	private IEnumerator CannotHighligh_IE()
	{
		yield return new WaitForSeconds(0.1f);
		isHighlight = false;
	}

	public void OnEnemyDead(Action DeadAction)
	{
		OnCancelHighlight();
		ClearAllBuff();
		RecycleMeanIcon();
		HandleEffectConfig(DeadEffectConfig.IsNullOrEmpty() ? null : LoadEnemyEffectConfig(DeadEffectConfig), base.transform, null, null, null);
		OnEnemyDeadAnim(DeadAction);
	}

	public void OnEnemyBorn()
	{
		HandleEffectConfig(ShowEffectConfig.IsNullOrEmpty() ? null : LoadEnemyEffectConfig(ShowEffectConfig), base.transform, null, null, null);
	}

	protected virtual void OnEnemyDeadAnim(Action deadAction)
	{
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(EnemyDeadAnim(deadAction));
		}
	}

	private IEnumerator EnemyDeadAnim(Action DeadAction)
	{
		Singleton<EnemyController>.Instance.AddDieingMonster(enemyEntity);
		Singleton<EnemyController>.Instance.RemoveMonster(enemyEntity, isNeedReAdjust: false);
		base.transform.DOShakePosition(1.2f, Vector3.one * 0.1f, 10, 360f).OnComplete(delegate
		{
			Singleton<EnemyController>.Instance.RemoveDieingMonster(enemyEntity);
			if (Singleton<EnemyController>.Instance.AllEnemies.Count > 0)
			{
				Singleton<EnemyController>.Instance.ReAdjustMonstersPos();
			}
			Singleton<EnemyController>.Instance.RecycleEnemy(enemyEntity.EnemyCode, this);
			StopAllCoroutines();
		});
		enemyAnimCtrl.FadeToTransparent(1.2f);
		yield return new WaitForSeconds(0.2f);
		DeadAction?.Invoke();
	}

	protected static void HandleEffectConfig(BaseEffectConfig config, Transform caster, Transform[] targets, Action handler, Action endAction)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(config, caster, targets, handler, endAction);
	}

	protected static void BubbleTalk(string[] contentKeys, Transform root)
	{
		if (!contentKeys.IsNull() && contentKeys.Length != 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowMTopBubble((contentKeys.Length == 1) ? contentKeys[0].LocalizeText() : contentKeys[UnityEngine.Random.Range(0, contentKeys.Length)].LocalizeText(), root);
		}
	}

	protected static void BubbleTalkMRight(string[] contentKeys, Transform root)
	{
		if (!contentKeys.IsNull() && contentKeys.Length != 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowMRightBubble((contentKeys.Length == 1) ? contentKeys[0].LocalizeText() : contentKeys[UnityEngine.Random.Range(0, contentKeys.Length)].LocalizeText(), root);
		}
	}

	protected static BaseEffectConfig LoadEnemyEffectConfig(string configName)
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(configName, "EffectConfigScriObj/Enemy");
	}

	protected void EnemyStepForward()
	{
		base.transform.DOScale(1.1f, 0.2f);
	}

	protected void EnemyStepBack(Action actionOverAction)
	{
		StartCoroutine(EnemyStepBack_IE(actionOverAction));
	}

	private IEnumerator EnemyStepBack_IE(Action actionOverAction)
	{
		yield return new WaitForSeconds(0.2f);
		base.transform.DOScale(1f, 0.2f).OnComplete(delegate
		{
			WaitActionOver(actionOverAction);
		});
	}

	protected void ActionAnim(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		StartCoroutine(ActionAnimAddStepForward_IE(config, targets, effect, actionOver));
	}

	private IEnumerator ActionAnimAddStepForward_IE(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		HideEnemyMean();
		EnemyStepForward();
		isEnemyActionAnim = true;
		yield return new WaitForSeconds(0.3f);
		HandleEffectConfig(config, base.transform, targets, effect, delegate
		{
			EnemyStepBack(actionOver);
			isEnemyActionAnim = false;
		});
	}

	protected void ActionAnim(BaseEffectConfig config, int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		StartCoroutine(ActionAnimAddStepAForward_IE(config, time, targets, effect, actionOver));
	}

	private IEnumerator ActionAnimAddStepAForward_IE(BaseEffectConfig config, int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		HideEnemyMean();
		EnemyStepForward();
		isEnemyActionAnim = true;
		yield return new WaitForSeconds(0.3f);
		if (time == 0)
		{
			HandleEffectConfig(config, base.transform, targets, delegate
			{
				effect(0);
			}, delegate
			{
				EnemyStepBack(actionOver);
				isEnemyActionAnim = false;
			});
			yield break;
		}
		bool isMove = false;
		int i = 0;
		while (i < time)
		{
			if (Singleton<GameManager>.Instance.Player.IsDead)
			{
				break;
			}
			isMove = true;
			int tempI = i;
			HandleEffectConfig(config, base.transform, targets, delegate
			{
				effect(tempI);
			}, delegate
			{
				isMove = false;
			});
			while (isMove)
			{
				yield return null;
			}
			if (enemyEntity.IsDead)
			{
				break;
			}
			int num = i + 1;
			i = num;
		}
		EnemyStepBack(actionOver);
		isEnemyActionAnim = false;
	}

	protected void WaitActionOver(Action overAction)
	{
		if (!enemyEntity.IsDead)
		{
			StartCoroutine(WaitActionOver_IE(overAction));
		}
	}

	private IEnumerator WaitActionOver_IE(Action overAction)
	{
		while (!Singleton<EnemyController>.Instance.IsAllEnemyActionOver())
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.4f);
		overAction?.Invoke();
	}

	protected virtual void InitHealthBar()
	{
		healthBarCtrl.InitHealthBar(enemyEntity.EntityName, enemyEntity.EntityAttr.MaxHealth, enemyEntity.EntityAttr.Armor);
	}

	private void OnMouseDown()
	{
		OnHighlight();
	}

	private void OnMouseEnter()
	{
		OnHighlight();
	}

	private void OnMouseUp()
	{
		OnCancelHighlight();
	}

	private void OnMouseExit()
	{
		OnCancelHighlight();
	}

	private void OnHighlight()
	{
		if (!isHighlight && !UIUtilities.IsCursorOnUI(IgnoreTags) && !enemyEntity.IsDead)
		{
			isHighlight = true;
			Singleton<GameManager>.Instance.BattleSystem.SetEnemyPlayerChoose(enemyEntity);
			if (!LockEnemyMean)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("EnemyMeanHintUI") as EnemyMeanHintUI).ShowEnemyMean(this, allMeans);
			}
		}
	}

	public void OnCancelHighlight()
	{
		if (isHighlight && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(CancelHighlight_IE());
		}
	}

	private IEnumerator CancelHighlight_IE()
	{
		yield return new WaitForEndOfFrame();
		isHighlight = false;
		Singleton<GameManager>.Instance.BattleSystem.SetEnemyPlayerChoose(null);
		SingletonDontDestroy<UIManager>.Instance.HideView("EnemyMeanHintUI");
	}

	public virtual void UpdateHealth(int currentHealth, int maxHealth)
	{
		healthBarCtrl.UpdateHealth(currentHealth, maxHealth);
	}

	public virtual void UpdateArmor(int armor)
	{
		healthBarCtrl.UpdateAmor(armor);
	}

	public virtual void FlashWhite()
	{
		if (base.gameObject.activeSelf && !enemyEntity.IsDead)
		{
			enemyAnimCtrl.FlashWhite();
			EnemyAtkBack();
		}
	}

	private void EnemyAtkBack()
	{
		if (!isEnemyActionAnim)
		{
			Sequence s = DOTween.Sequence();
			s.Append(base.transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad));
			s.Append(base.transform.DOScale(1f, 0.1f)).SetEase(Ease.OutQuad);
		}
	}

	public void DeAtiveThisEntity()
	{
		StopAllCoroutines();
		base.transform.DOKill();
	}

	protected virtual void ClearAllBuff()
	{
		healthBarCtrl.ClearHealthBarBuff();
	}

	public virtual void AddBuffIcon(BaseBuff buff)
	{
		healthBarCtrl.AddBuff(buff);
	}

	public virtual void RemoveBuffIcon(BaseBuff buff)
	{
		healthBarCtrl.RemoveBuff(buff);
	}

	public virtual void UpdateBuffIcon(BaseBuff buff)
	{
		healthBarCtrl.UpdateBuff(buff);
	}

	private void AddMean(MeanHandler meanHandler)
	{
		MeanIconCtrl meanIcon = GetMeanIcon();
		meanIcon.transform.SetParent(meanRoot);
		meanIcon.transform.localScale = Vector3.one;
		meanIcon.SetMean(meanHandler);
		showingMeanIcons.Add(meanIcon);
		allMeans.Add(meanHandler);
	}

	public virtual void AddMean(MeanHandler[] meanHandlers)
	{
		RecycleMeanIcon();
		for (int i = 0; i < meanHandlers.Length; i++)
		{
			AddMean(meanHandlers[i]);
		}
	}

	protected MeanIconCtrl GetMeanIcon()
	{
		if (meanIconPool.Count > 0)
		{
			MeanIconCtrl meanIconCtrl = meanIconPool.Dequeue();
			if (meanIconCtrl == null)
			{
				return GetMeanIcon();
			}
			meanIconCtrl.gameObject.SetActive(value: true);
			return meanIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("MeanIcon", "Prefabs", meanRoot).GetComponent<MeanIconCtrl>();
	}

	protected virtual void RecycleMeanIcon()
	{
		if (showingMeanIcons.Count > 0)
		{
			for (int i = 0; i < showingMeanIcons.Count; i++)
			{
				showingMeanIcons[i].gameObject.SetActive(value: false);
				meanIconPool.Enqueue(showingMeanIcons[i]);
			}
			showingMeanIcons.Clear();
			allMeans.Clear();
		}
	}

	protected virtual void HideEnemyMean()
	{
		meanRoot.gameObject.SetActive(value: false);
	}

	public virtual void ShowEnemyMean()
	{
		meanRoot.gameObject.SetActive(value: true);
	}
}
