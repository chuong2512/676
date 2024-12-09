using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MonsterIlluBookPanel : MonoBehaviour, IllustratedBooksUI.IIlluPanel
{
	private abstract class MonsterHandler
	{
		protected Button btn;

		protected Transform root;

		protected MonsterIlluBookPanel parentPanel;

		protected Transform rootPanel;

		public MonsterHandler(MonsterIlluBookPanel parentPanel, Button btn, Transform rootPanel, Transform root)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			this.root = root;
			this.rootPanel = rootPanel;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisMonsterType();
		}

		public abstract void OnSetThisMonsterType();

		public void SetInterative()
		{
			btn.interactable = true;
			rootPanel.gameObject.SetActive(value: false);
		}
	}

	private class NormalMonsterHandler : MonsterHandler
	{
		public NormalMonsterHandler(MonsterIlluBookPanel parentPanel, Button btn, Transform rootPanel, Transform root)
			: base(parentPanel, btn, rootPanel, root)
		{
		}

		public override void OnSetThisMonsterType()
		{
			int num = 0;
			int num2 = 0;
			rootPanel.gameObject.SetActive(value: true);
			parentPanel.SetCurrentActiveHandler(this);
			parentPanel.RecycleAllShowingMonsterIlluBooksCtrls();
			btn.interactable = false;
			foreach (KeyValuePair<string, EnemyData> allEnemyAttr in DataManager.Instance.GetAllEnemyAttrs())
			{
				if (!allEnemyAttr.Value.IsBoss && allEnemyAttr.Value.isNeedShowInfo)
				{
					bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsNormalMonsterIlluUnlocked(allEnemyAttr.Value.EnemyCode);
					SingleMonsterIlluBooksCtrl singltMonsterIlluBooksCtrl = parentPanel.GetSingltMonsterIlluBooksCtrl();
					singltMonsterIlluBooksCtrl.transform.SetParent(root);
					singltMonsterIlluBooksCtrl.transform.localScale = 0.9f * Vector3.one;
					singltMonsterIlluBooksCtrl.transform.SetAsLastSibling();
					singltMonsterIlluBooksCtrl.LoadMonster(allEnemyAttr.Value, flag ? parentPanel.showMaterial : parentPanel.hideMaterial, flag);
					singltMonsterIlluBooksCtrl.CanvasGroup.DOKill();
					singltMonsterIlluBooksCtrl.CanvasGroup.Fade(0f, 1f);
					parentPanel.AddShowingMonsterIlluBooksCtrl(singltMonsterIlluBooksCtrl);
					num2++;
					if (flag)
					{
						num++;
					}
				}
			}
			parentPanel.ShowUnlockProgressInfo(num2, num);
			parentPanel.ParentPanel.SetScrolllbar();
		}
	}

	private class BossMonsterHandler : MonsterHandler
	{
		public BossMonsterHandler(MonsterIlluBookPanel parentPanel, Button btn, Transform rootPanel, Transform root)
			: base(parentPanel, btn, rootPanel, root)
		{
		}

		public override void OnSetThisMonsterType()
		{
			int num = 0;
			int num2 = 0;
			rootPanel.gameObject.SetActive(value: true);
			parentPanel.SetCurrentActiveHandler(this);
			parentPanel.RecycleAllShowingMonsterIlluBooksCtrls();
			btn.interactable = false;
			foreach (KeyValuePair<string, EnemyData> allEnemyAttr in DataManager.Instance.GetAllEnemyAttrs())
			{
				if (allEnemyAttr.Value.IsBoss && allEnemyAttr.Value.isNeedShowInfo)
				{
					bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsBossMonsterIlluUnlocked(allEnemyAttr.Value.EnemyCode);
					SingleMonsterIlluBooksCtrl singltMonsterIlluBooksCtrl = parentPanel.GetSingltMonsterIlluBooksCtrl();
					singltMonsterIlluBooksCtrl.transform.SetParent(root);
					singltMonsterIlluBooksCtrl.transform.localScale = Vector3.one;
					singltMonsterIlluBooksCtrl.transform.SetAsLastSibling();
					singltMonsterIlluBooksCtrl.LoadMonster(allEnemyAttr.Value, flag ? parentPanel.showMaterial : parentPanel.hideMaterial, flag);
					singltMonsterIlluBooksCtrl.CanvasGroup.DOKill();
					singltMonsterIlluBooksCtrl.CanvasGroup.Fade(0f, 1f);
					parentPanel.AddShowingMonsterIlluBooksCtrl(singltMonsterIlluBooksCtrl);
					num++;
					if (flag)
					{
						num2++;
					}
				}
			}
			parentPanel.ShowUnlockProgressInfo(num, num2);
			parentPanel.ParentPanel.SetScrolllbar();
		}
	}

	public Material showMaterial;

	public Material hideMaterial;

	private Button normalMonsterBtn;

	private Button bossMonsterBtn;

	private Transform normalMonsterRootPanel;

	private Transform bossMonsterRootPanel;

	private Transform normalMonsterRoot;

	private Transform bossMonsterRoot;

	private MonsterHandler normalMonsterHandler;

	private MonsterHandler bossMonsterHandler;

	private UIAnim_Common anim;

	private MonsterHandler currentMonsterHandler;

	private Action<int, int> unlockProgressAction;

	private Queue<SingleMonsterIlluBooksCtrl> allMonsterIlluBooksCtrlPools = new Queue<SingleMonsterIlluBooksCtrl>();

	private List<SingleMonsterIlluBooksCtrl> allShowingMonsterIlluBooksCtrls = new List<SingleMonsterIlluBooksCtrl>();

	public IllustratedBooksUI ParentPanel { get; private set; }

	private void Awake()
	{
		normalMonsterBtn = base.transform.Find("NormalMonster").GetComponent<Button>();
		bossMonsterBtn = base.transform.Find("BossMonster").GetComponent<Button>();
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
		normalMonsterRootPanel = base.transform.Find("NormalRoot");
		normalMonsterRoot = normalMonsterRootPanel.Find("Mask/Content");
		bossMonsterRootPanel = base.transform.Find("BossRoot");
		bossMonsterRoot = bossMonsterRootPanel.Find("Mask/Content");
		normalMonsterHandler = new NormalMonsterHandler(this, normalMonsterBtn, normalMonsterRootPanel, normalMonsterRoot);
		bossMonsterHandler = new BossMonsterHandler(this, bossMonsterBtn, bossMonsterRootPanel, bossMonsterRoot);
	}

	public void Show(IllustratedBooksUI parentUI, Action<int, int> unlockProgressAction)
	{
		ParentPanel = parentUI;
		this.unlockProgressAction = unlockProgressAction;
		base.gameObject.SetActive(value: true);
		normalMonsterHandler.OnSetThisMonsterType();
		anim.StartAnim();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllShowingMonsterIlluBooksCtrls();
		currentMonsterHandler?.SetInterative();
		currentMonsterHandler = null;
	}

	public void ShowUnlockProgressInfo(int maxAmount, int currentAmount)
	{
		unlockProgressAction(maxAmount, currentAmount);
	}

	private void SetCurrentActiveHandler(MonsterHandler handler)
	{
		currentMonsterHandler?.SetInterative();
		currentMonsterHandler = handler;
	}

	public void AddShowingMonsterIlluBooksCtrl(SingleMonsterIlluBooksCtrl ctrl)
	{
		allShowingMonsterIlluBooksCtrls.Add(ctrl);
	}

	public SingleMonsterIlluBooksCtrl GetSingltMonsterIlluBooksCtrl()
	{
		if (allMonsterIlluBooksCtrlPools.Count > 0)
		{
			SingleMonsterIlluBooksCtrl singleMonsterIlluBooksCtrl = allMonsterIlluBooksCtrlPools.Dequeue();
			singleMonsterIlluBooksCtrl.gameObject.SetActive(value: true);
			return singleMonsterIlluBooksCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleMonsterIlluBooksCtrl", "Prefabs", base.transform).GetComponent<SingleMonsterIlluBooksCtrl>();
	}

	public void RecycleAllShowingMonsterIlluBooksCtrls()
	{
		if (allShowingMonsterIlluBooksCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingMonsterIlluBooksCtrls.Count; i++)
			{
				allShowingMonsterIlluBooksCtrls[i].gameObject.SetActive(value: false);
				allMonsterIlluBooksCtrlPools.Enqueue(allShowingMonsterIlluBooksCtrls[i]);
			}
			allShowingMonsterIlluBooksCtrls.Clear();
		}
	}
}
