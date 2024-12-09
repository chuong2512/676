using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIlluBookPanel : MonoBehaviour, IllustratedBooksUI.IIlluPanel
{
	private class SkillIlluHandler
	{
		private Button btn;

		private SkillIlluBookPanel parentPanel;

		public PlayerOccupation PlayerOccupation { get; }

		public SkillIlluHandler(SkillIlluBookPanel parentPanel, Button btn, PlayerOccupation playerOccupation)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			PlayerOccupation = playerOccupation;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisHandler();
		}

		public void OnSetThisHandler()
		{
			SetBtnInteractive(isActive: false);
			parentPanel.RecycleAllShowingIlluCtrls();
			parentPanel.SetCurrentHandler(this);
			int num = 0;
			Dictionary<string, SkillCardAttr> allPlayerOccupationSkillCardDatas = DataManager.Instance.GetAllPlayerOccupationSkillCardDatas(PlayerOccupation);
			foreach (KeyValuePair<string, SkillCardAttr> item in allPlayerOccupationSkillCardDatas)
			{
				SingleIlluSkillCtrl singleIlluSkillCtrl = parentPanel.GetSingleIlluSkillCtrl();
				singleIlluSkillCtrl.gameObject.SetActive(value: true);
				singleIlluSkillCtrl.transform.SetAsLastSibling();
				bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSkillIlluUnlocked(item.Key);
				bool isPurchased = !item.Value.IsNeedPurchased || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSkillPurchased(item.Key);
				singleIlluSkillCtrl.LoadSkill(parentPanel, item.Value, flag, isPurchased);
				parentPanel.AddShowingCtrl(singleIlluSkillCtrl);
				if (flag)
				{
					num++;
				}
			}
			parentPanel.ShowProgressInfo(allPlayerOccupationSkillCardDatas.Count, num);
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SingleIlluSkillCtrl allShowingSkillIlluCtrl in parentPanel.allShowingSkillIlluCtrls)
			{
				list.Add(allShowingSkillIlluCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			parentPanel.ParentPanel.SetScrolllbar();
		}

		public void Hide()
		{
			SetBtnInteractive(isActive: true);
		}

		private void SetBtnInteractive(bool isActive)
		{
			btn.interactable = isActive;
		}
	}

	public Sprite NotUnlockedSkillSprite;

	private Action<int, int> unlockProgressAction;

	private SkillIlluHandler knightHandler;

	private SkillIlluHandler archerHandler;

	private SkillIlluHandler currentHandler;

	private Transform skillIlluRoot;

	private Queue<SingleIlluSkillCtrl> allSkillIlluCtrlPools = new Queue<SingleIlluSkillCtrl>();

	private List<SingleIlluSkillCtrl> allShowingSkillIlluCtrls = new List<SingleIlluSkillCtrl>();

	private UIAnim_Common anim;

	public PlayerOccupation CurrentPlayerOccupation => currentHandler.PlayerOccupation;

	public IllustratedBooksUI ParentPanel { get; private set; }

	private void Awake()
	{
		archerHandler = new SkillIlluHandler(this, base.transform.Find("Archer").GetComponent<Button>(), PlayerOccupation.Archer);
		knightHandler = new SkillIlluHandler(this, base.transform.Find("Knight").GetComponent<Button>(), PlayerOccupation.Knight);
		skillIlluRoot = base.transform.Find("SkillShowPanel/Mask/Content");
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(IllustratedBooksUI parentPanel, Action<int, int> unlockProgressAction)
	{
		ParentPanel = parentPanel;
		this.unlockProgressAction = unlockProgressAction;
		base.gameObject.SetActive(value: true);
		archerHandler.OnSetThisHandler();
		anim.StartAnim();
	}

	public void Hide()
	{
		currentHandler?.Hide();
		currentHandler = null;
		base.gameObject.SetActive(value: false);
	}

	private void SetCurrentHandler(SkillIlluHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	public void ShowProgressInfo(int maxAmount, int currentAmount)
	{
		unlockProgressAction(maxAmount, currentAmount);
	}

	public void AddShowingCtrl(SingleIlluSkillCtrl ctrl)
	{
		allShowingSkillIlluCtrls.Add(ctrl);
	}

	public SingleIlluSkillCtrl GetSingleIlluSkillCtrl()
	{
		if (allSkillIlluCtrlPools.Count > 0)
		{
			SingleIlluSkillCtrl singleIlluSkillCtrl = allSkillIlluCtrlPools.Dequeue();
			singleIlluSkillCtrl.gameObject.SetActive(value: true);
			return singleIlluSkillCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleIlluSkillCtrl", "Prefabs", skillIlluRoot).GetComponent<SingleIlluSkillCtrl>();
	}

	public void RecycleAllShowingIlluCtrls()
	{
		if (allShowingSkillIlluCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingSkillIlluCtrls.Count; i++)
			{
				allShowingSkillIlluCtrls[i].gameObject.SetActive(value: false);
				allSkillIlluCtrlPools.Enqueue(allShowingSkillIlluCtrls[i]);
			}
			allShowingSkillIlluCtrls.Clear();
		}
	}
}
