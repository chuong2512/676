using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpUI : EscCloseUIView
{
	private class HelpUI_Handler
	{
		private static readonly Color NormalColor = "673122FF".HexColorToColor();

		private static readonly Color ChosenColor = "FFF3D8FF".HexColorToColor();

		private Button btn;

		private HelpUI parentUI;

		private Text itemText;

		private Outline itemTextOutline;

		private CompleteHelpData currentHelpCodeData;

		public HelpUI_Handler(HelpUI parentUi, Button btn, Text itemText, string helpCode)
		{
			this.btn = btn;
			parentUI = parentUi;
			this.itemText = itemText;
			itemTextOutline = itemText.GetComponent<Outline>();
			currentHelpCodeData = DataManager.Instance.GetCompleteHelpData(helpCode);
			itemText.text = currentHelpCodeData.HelpCode.LocalizeText();
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisType();
		}

		public void OnSetThisType()
		{
			btn.interactable = false;
			itemTextOutline.enabled = true;
			itemText.color = ChosenColor;
			parentUI.SwitchHandler(this);
			parentUI.LoadHelpData(currentHelpCodeData);
		}

		public void Hide()
		{
			parentUI.RecycleAllItems();
			itemTextOutline.enabled = false;
			itemText.color = NormalColor;
			btn.interactable = true;
		}
	}

	private UIAnim_HelpUI anim;

	private Transform helpItemRoot;

	private RectTransform helpItemRectRoot;

	private Scrollbar _scrollbar;

	private Queue<HelpContentLineCtrl> helpLinePools = new Queue<HelpContentLineCtrl>();

	private List<HelpContentLineCtrl> allShowingHelpLines = new List<HelpContentLineCtrl>();

	private Queue<SingleHelpContentCtrl> helpContentPools = new Queue<SingleHelpContentCtrl>();

	private List<SingleHelpContentCtrl> allShowingHelpContents = new List<SingleHelpContentCtrl>();

	private HelpUI_Handler currentHandler;

	private HelpUI_Handler battleHandler;

	private HelpUI_Handler exploreHandler;

	private HelpUI_Handler otherHandler;

	public override string UIViewName => "HelpUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入帮助界面");
		base.gameObject.SetActive(value: true);
		battleHandler.OnSetThisType();
		anim.StartAnim();
	}

	public override void HideView()
	{
		currentHandler?.Hide();
		currentHandler = null;
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		Init();
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickQuitBtn();
	}

	private void Init()
	{
		InitUsualFunction();
		InitHandler();
		anim = GetComponent<UIAnim_HelpUI>();
	}

	private void InitUsualFunction()
	{
		helpItemRoot = base.transform.Find("Bg/ContentRoot/Mask/Content");
		helpItemRectRoot = helpItemRoot.GetComponent<RectTransform>();
		base.transform.Find("Bg/QuitBtn").GetComponent<Button>().onClick.AddListener(OnClickQuitBtn);
		_scrollbar = base.transform.Find("Bg/ContentRoot/Scrollbar").GetComponent<Scrollbar>();
	}

	private void OnClickQuitBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void LoadHelpData(CompleteHelpData helpData)
	{
		for (int i = 0; i < helpData.HelpDatas.Count; i++)
		{
			AddSingleHelpData(helpData.HelpDatas[i]);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(helpItemRectRoot);
		StartCoroutine(SetScrollbar_IE());
	}

	private IEnumerator SetScrollbar_IE()
	{
		yield return null;
		_scrollbar.value = 1f;
	}

	private void AddSingleHelpData(SingleHelpData data)
	{
		AddHelpLines(data.TitleKey.LocalizeText());
		for (int i = 0; i < data.GuideTips.Count; i++)
		{
			AddHelpContent(data.GuideTips[i]);
		}
	}

	private void AddHelpLines(string title)
	{
		HelpContentLineCtrl helpContentLineCtrl = GetHelpContentLineCtrl();
		helpContentLineCtrl.transform.SetAsLastSibling();
		helpContentLineCtrl.SetTitle(title);
		allShowingHelpLines.Add(helpContentLineCtrl);
	}

	private HelpContentLineCtrl GetHelpContentLineCtrl()
	{
		if (helpLinePools.Count > 0)
		{
			HelpContentLineCtrl helpContentLineCtrl = helpLinePools.Dequeue();
			helpContentLineCtrl.gameObject.SetActive(value: true);
			return helpContentLineCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("HelpContent_Line", "Prefabs", helpItemRoot).GetComponent<HelpContentLineCtrl>();
	}

	private void AddHelpContent(string guideTipsCode)
	{
		SingleHelpContentCtrl singleHelpContent = GetSingleHelpContent();
		singleHelpContent.transform.SetAsLastSibling();
		singleHelpContent.LoadContent(guideTipsCode);
		allShowingHelpContents.Add(singleHelpContent);
	}

	private SingleHelpContentCtrl GetSingleHelpContent()
	{
		if (helpContentPools.Count > 0)
		{
			SingleHelpContentCtrl singleHelpContentCtrl = helpContentPools.Dequeue();
			singleHelpContentCtrl.gameObject.SetActive(value: true);
			return singleHelpContentCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleHelpContent", "Prefabs", helpItemRoot).GetComponent<SingleHelpContentCtrl>();
	}

	private void RecycleAllItems()
	{
		if (allShowingHelpLines.Count > 0)
		{
			for (int i = 0; i < allShowingHelpLines.Count; i++)
			{
				allShowingHelpLines[i].gameObject.SetActive(value: false);
				helpLinePools.Enqueue(allShowingHelpLines[i]);
			}
			allShowingHelpLines.Clear();
		}
		if (allShowingHelpContents.Count > 0)
		{
			for (int j = 0; j < allShowingHelpContents.Count; j++)
			{
				allShowingHelpContents[j].gameObject.SetActive(value: false);
				helpContentPools.Enqueue(allShowingHelpContents[j]);
			}
			allShowingHelpContents.Clear();
		}
	}

	private void InitHandler()
	{
		battleHandler = new HelpUI_Handler(this, base.transform.Find("Bg/ItemRoot/SingleHelpItem_Battle").GetComponent<Button>(), base.transform.Find("Bg/ItemRoot/SingleHelpItem_Battle/ItemText").GetComponent<Text>(), "Battle");
		exploreHandler = new HelpUI_Handler(this, base.transform.Find("Bg/ItemRoot/SingleHelpItem_Explore").GetComponent<Button>(), base.transform.Find("Bg/ItemRoot/SingleHelpItem_Explore/ItemText").GetComponent<Text>(), "Explore");
		otherHandler = new HelpUI_Handler(this, base.transform.Find("Bg/ItemRoot/SingleHelpItem_Other").GetComponent<Button>(), base.transform.Find("Bg/ItemRoot/SingleHelpItem_Other/ItemText").GetComponent<Text>(), "Other");
	}

	private void SwitchHandler(HelpUI_Handler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}
}
