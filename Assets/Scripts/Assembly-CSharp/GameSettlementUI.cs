using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameSettlementUI : UIView
{
	private const float MaskBlackAlpha = 0.7450981f;

	public Sprite ChangedSpaceTimeSprite;

	public Sprite lockMask_CanUnlockCard;

	public Sprite lockMask_CannotUnlockCard;

	private Text spaceTimeText;

	private Image occupationImg;

	private ScrollRect cardUnlockScrollRect;

	private Transform cardUnlockRoot;

	private CardUnlockPanelBase currentUnlockPanel;

	private Image maskImg;

	private Image spaceTimeImg;

	private Scrollbar _scrollbar;

	private Dictionary<PlayerOccupation, CardUnlockPanelBase> allSpawnedUnlockPanel = new Dictionary<PlayerOccupation, CardUnlockPanelBase>();

	private int allSpaceTimeAmount;

	public override string UIViewName => "GameSettlementUI";

	public override string UILayerName => "NormalLayer";

	public int AllSpaceTimeAmount => allSpaceTimeAmount;

	public override void ShowView(params object[] objsR)
	{
		base.gameObject.SetActive(value: true);
		EventManager.BroadcastEvent(EventEnum.E_ShowUIView, new SimpleEventData
		{
			stringValue = UIViewName
		});
		LoadCardUnlockUI();
	}

	public override void HideView()
	{
		currentUnlockPanel.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		Transform transform = base.transform.Find("Bg");
		spaceTimeText = transform.Find("SpaceTimeBottom/Amount").GetComponent<Text>();
		occupationImg = transform.Find("OccupationImg").GetComponent<Image>();
		cardUnlockScrollRect = transform.Find("CardUnlockPanel").GetComponent<ScrollRect>();
		cardUnlockRoot = transform.Find("CardUnlockPanel/Mask");
		base.transform.Find("Bg/BackBtn").GetComponent<Button>().onClick.AddListener(OnClickBackBtn);
		maskImg = base.transform.Find("Bg/Mask").GetComponent<Image>();
		spaceTimeImg = base.transform.Find("Bg/Mask/SpaceIcon").GetComponent<Image>();
		_scrollbar = base.transform.Find("Bg/CardUnlockPanel/Scrollbar").GetComponent<Scrollbar>();
	}

	private void SetSpaceTimeIcon(bool isActive)
	{
		spaceTimeImg.gameObject.SetActive(value: true);
	}

	private void LoadCardUnlockUI()
	{
		Player player = Singleton<GameManager>.Instance.Player;
		OccupationData occupationData = DataManager.Instance.GetOccupationData(player.PlayerOccupation);
		ShowPlayerFixedInfo(occupationData);
		LoadOccupationUnlockCardPanel(player, occupationData);
		ShowPlayerProgressInfo_IE(player);
		SetScrollbar();
	}

	private void SetScrollbar()
	{
		StartCoroutine(SetScrollbar_IE());
	}

	private IEnumerator SetScrollbar_IE()
	{
		yield return null;
		_scrollbar.value = 1f;
	}

	public void ShowMask(float alpha, bool isSpaceIconActive)
	{
		maskImg.gameObject.SetActive(value: true);
		maskImg.color = new Color(0f, 0f, 0f, alpha);
		spaceTimeImg.gameObject.SetActive(isSpaceIconActive);
	}

	public void HideMask()
	{
		maskImg.gameObject.SetActive(value: false);
		maskImg.color = Color.clear;
		spaceTimeImg.gameObject.SetActive(value: true);
	}

	private void LoadOccupationUnlockCardPanel(Player player, OccupationData occupationData)
	{
		if (allSpawnedUnlockPanel.TryGetValue(player.PlayerOccupation, out currentUnlockPanel))
		{
			currentUnlockPanel.gameObject.SetActive(value: true);
		}
		else
		{
			GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(occupationData.OccupationUnlockPrefabName, occupationData.DefaultPrefabPath, cardUnlockRoot);
			currentUnlockPanel = gameObject.GetComponent<CardUnlockPanelBase>();
			allSpawnedUnlockPanel.Add(player.PlayerOccupation, currentUnlockPanel);
		}
		currentUnlockPanel.LoadInitCard(new CardUnlockPanelBase.NormalCardUnlockPanelHandler(this));
		cardUnlockScrollRect.content = currentUnlockPanel.GetComponent<RectTransform>();
	}

	private void ShowPlayerFixedInfo(OccupationData occupationData)
	{
		allSpaceTimeAmount = 0;
		occupationImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.UnlockCardBottomSpriteName, occupationData.DefaultSpritePath);
	}

	private void ResetAllVariables()
	{
		spaceTimeText.text = "0";
		HideMask();
	}

	private void ShowPlayerProgressInfo_IE(Player player)
	{
		ResetAllVariables();
		AddSpaceTime(player.TimePiecesAmount);
	}

	private void OnClickBackBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ComfirmBackToMenu".LocalizeText(), OnConfirmBackToMenu);
	}

	private void OnConfirmBackToMenu()
	{
		if (allSpaceTimeAmount > 0)
		{
			StartCoroutine(ShowSpaceTimeChangedAnim());
		}
		else
		{
			BackToMenu();
		}
	}

	private IEnumerator ShowSpaceTimeChangedAnim()
	{
		ShowMask(0.7450981f, isSpaceIconActive: true);
		maskImg.DOFade(0.7f, 1f).OnComplete(delegate
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_daibizhuanhuan_cast");
			vfxBase.transform.position = spaceTimeImg.transform.position;
			vfxBase.Play();
			spaceTimeImg.sprite = ChangedSpaceTimeSprite;
		});
		yield return new WaitForSeconds(2f);
		BackToMenu();
	}

	private void BackToMenu()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowMask(delegate
		{
			SingletonDontDestroy<UIManager>.Instance.HideAllShowingView();
			SingletonDontDestroy<Game>.Instance.SwitchScene(1);
		});
		if (allSpaceTimeAmount > 0)
		{
			SingletonDontDestroy<Game>.Instance.CurrentUserData.AddCoin(allSpaceTimeAmount, isAutoSave: true);
		}
	}

	private void AddSpaceTime(int amount)
	{
		allSpaceTimeAmount += amount;
		spaceTimeText.text = allSpaceTimeAmount.ToString();
		Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("+" + amount, Color.white, Color.black, spaceTimeText.transform, isSetParent: false, Vector3.zero);
	}

	public void ComsumeSpaceTime(int amount)
	{
		allSpaceTimeAmount -= amount;
		spaceTimeText.text = allSpaceTimeAmount.ToString();
		Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-" + amount, Color.white, Color.black, spaceTimeText.transform, isSetParent: false, Vector3.zero);
	}
}
