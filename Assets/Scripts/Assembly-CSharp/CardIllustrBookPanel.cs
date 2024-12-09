using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardIllustrBookPanel : MonoBehaviour, IllustratedBooksUI.IIlluPanel
{
	private abstract class CardIlluBookHandler
	{
		private Button btn;

		protected CardIllustrBookPanel parentPanel;

		public CardIlluBookHandler(CardIllustrBookPanel parentPanel, Button btn)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisHandler();
		}

		public void OnSetThisHandler()
		{
			SetInteractive(isActive: false);
			parentPanel.SetCurrentHandler(this);
			OnShow();
		}

		protected abstract void OnShow();

		public void Hide()
		{
			SetInteractive(isActive: true);
			parentPanel.RecycleAllShowingCardCtrls();
			OnHide();
		}

		protected virtual void OnHide()
		{
		}

		private void SetInteractive(bool isActive)
		{
			btn.interactable = isActive;
		}
	}

	private class CommonCardBookHandler : CardIlluBookHandler
	{
		public CommonCardBookHandler(CardIllustrBookPanel parentPanel, Button btn)
			: base(parentPanel, btn)
		{
		}

		protected override void OnShow()
		{
			int num = 0;
			parentPanel.ActiveCommonCardUnlockPanel();
			Dictionary<string, SpecialUsualCardAttr> allSpecialUsualCardDatas = DataManager.Instance.GetAllSpecialUsualCardDatas();
			foreach (KeyValuePair<string, SpecialUsualCardAttr> item in allSpecialUsualCardDatas)
			{
				SingleIlluCardCtrl singeIlluCardCtrl = parentPanel.GetSingeIlluCardCtrl();
				singeIlluCardCtrl.transform.SetAsLastSibling();
				parentPanel.AddShowingCtrl(singeIlluCardCtrl);
				bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSpecialUsualCardIlluUnlocked(item.Key);
				bool isPurchased = !item.Value.IsNeedPurchased || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSpecialUsualCardPurchased(item.Key);
				singeIlluCardCtrl.LoadCard(parentPanel, item.Value, flag, isPurchased);
				if (flag)
				{
					num++;
				}
			}
			parentPanel.ShowProgressInfo(allSpecialUsualCardDatas.Count, num);
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SingleIlluCardCtrl allShowingCardCtrl in parentPanel.allShowingCardCtrls)
			{
				list.Add(allShowingCardCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			parentPanel.ParentPanel.SetScrolllbar();
		}

		protected override void OnHide()
		{
			base.OnHide();
			parentPanel.DeactiveCommonCardUnlockPanel();
		}
	}

	private class OccupationCardBookHandler : CardIlluBookHandler
	{
		private RectTransform unlockPanelTarget;

		private CardUnlockPanelBase _cardUnlockPanelBase;

		private PlayerOccupation playerOccupation;

		public OccupationCardBookHandler(CardIllustrBookPanel parentPanel, Button btn, PlayerOccupation playerOccupation)
			: base(parentPanel, btn)
		{
			this.playerOccupation = playerOccupation;
		}

		protected override void OnShow()
		{
			parentPanel.ActiveCardUnlockPanel();
			parentPanel.SetUnlockCardPanelScrollRectContent(GetUnlockPanelTarget());
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SingleIlluCardCtrl allShowingCardCtrl in parentPanel.allShowingCardCtrls)
			{
				list.Add(allShowingCardCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			parentPanel.ParentPanel.SetScrolllbar();
		}

		private RectTransform GetUnlockPanelTarget()
		{
			if (unlockPanelTarget == null)
			{
				OccupationData occupationData = DataManager.Instance.GetOccupationData(playerOccupation);
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(occupationData.OccupationUnlockPrefabName, occupationData.DefaultPrefabPath, parentPanel.CardUnlockRoot);
				_cardUnlockPanelBase = gameObject.GetComponent<CardUnlockPanelBase>();
				unlockPanelTarget = gameObject.GetComponent<RectTransform>();
			}
			_cardUnlockPanelBase.LoadInitCard(new CardUnlockPanelBase.CheckCardUnlockPanelHandler());
			unlockPanelTarget.gameObject.SetActive(value: true);
			return unlockPanelTarget;
		}

		protected override void OnHide()
		{
			base.OnHide();
			parentPanel.DeactiveCardUnlockPanel();
			unlockPanelTarget.gameObject.SetActive(value: false);
		}
	}

	public Sprite CardNotUnlockedSprite;

	private Action<int, int> unlockProgressAction;

	private Transform commonCardRoot;

	private Transform cardUnlockRoot;

	private Transform commonCardPanel;

	private ScrollRect cardUnlockScrollRect;

	private CardIlluBookHandler commonCardHandler;

	private CardIlluBookHandler archerUnlockCardHandler;

	private CardIlluBookHandler knightUnlockCardHandler;

	private CardIlluBookHandler currentHandler;

	private UIAnim_Common anim;

	private Queue<SingleIlluCardCtrl> allIlluCardCtrlPools = new Queue<SingleIlluCardCtrl>();

	private List<SingleIlluCardCtrl> allShowingCardCtrls = new List<SingleIlluCardCtrl>();

	public Transform CardUnlockRoot => cardUnlockRoot;

	public IllustratedBooksUI ParentPanel { get; private set; }

	private void Awake()
	{
		commonCardRoot = base.transform.Find("CommonCardShowPanel/Mask/Content");
		cardUnlockRoot = base.transform.Find("CardUnlockPanel/Mask");
		commonCardPanel = base.transform.Find("CommonCardShowPanel");
		cardUnlockScrollRect = base.transform.Find("CardUnlockPanel").GetComponent<ScrollRect>();
		commonCardHandler = new CommonCardBookHandler(this, base.transform.Find("BothHand").GetComponent<Button>());
		archerUnlockCardHandler = new OccupationCardBookHandler(this, base.transform.Find("Archer").GetComponent<Button>(), PlayerOccupation.Archer);
		knightUnlockCardHandler = new OccupationCardBookHandler(this, base.transform.Find("Knight").GetComponent<Button>(), PlayerOccupation.Knight);
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(IllustratedBooksUI parentPanel, Action<int, int> unlockProgressAction)
	{
		ParentPanel = parentPanel;
		base.gameObject.SetActive(value: true);
		this.unlockProgressAction = unlockProgressAction;
		commonCardHandler.OnSetThisHandler();
		anim.StartAnim();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		currentHandler?.Hide();
		currentHandler = null;
	}

	private void SetCurrentHandler(CardIlluBookHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	public void ShowProgressInfo(int maxAmount, int currentAmount)
	{
		unlockProgressAction(maxAmount, currentAmount);
	}

	public void ActiveCommonCardUnlockPanel()
	{
		commonCardPanel.gameObject.SetActive(value: true);
	}

	public void DeactiveCommonCardUnlockPanel()
	{
		commonCardPanel.gameObject.SetActive(value: false);
	}

	public void ActiveCardUnlockPanel()
	{
		cardUnlockScrollRect.gameObject.SetActive(value: true);
	}

	public void DeactiveCardUnlockPanel()
	{
		cardUnlockScrollRect.gameObject.SetActive(value: false);
	}

	public void SetUnlockCardPanelScrollRectContent(RectTransform target)
	{
		cardUnlockScrollRect.content = target;
	}

	public void AddShowingCtrl(SingleIlluCardCtrl ctrl)
	{
		allShowingCardCtrls.Add(ctrl);
	}

	public SingleIlluCardCtrl GetSingeIlluCardCtrl()
	{
		if (allIlluCardCtrlPools.Count > 0)
		{
			SingleIlluCardCtrl singleIlluCardCtrl = allIlluCardCtrlPools.Dequeue();
			singleIlluCardCtrl.gameObject.SetActive(value: true);
			return singleIlluCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleIlluCardCtrl", "Prefabs", commonCardRoot).GetComponent<SingleIlluCardCtrl>();
	}

	public void RecycleAllShowingCardCtrls()
	{
		if (allShowingCardCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingCardCtrls.Count; i++)
			{
				allShowingCardCtrls[i].gameObject.SetActive(value: false);
				allIlluCardCtrlPools.Enqueue(allShowingCardCtrls[i]);
			}
			allShowingCardCtrls.Clear();
		}
	}
}
