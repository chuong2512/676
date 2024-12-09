using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseOccupationUI : UIView
{
	private Transform infomationRoot;

	private Image illustrationImg;

	private Image nameBottomImg;

	private Text nameText;

	private Text maxHealthText;

	private Text initMoneyText;

	private Text occupationDesText;

	private Text specificNameText;

	private Text specificDesText;

	private Transform prophesyBgTrans;

	private Text prophesyDesText;

	private Text prophesyTmpDesText;

	private Button purchaseBtn;

	private Image prophesyBtnImg;

	private Transform presuppositionRoot;

	private Button[] allSinglePresuppositionItems;

	private Text[] allSinglePresuppositionNameTexts;

	private Text currentShowingPresuppositionNameText;

	private Button startBtn;

	private Text comingSoonText;

	private Button leftMoveBtn;

	private Button rightMoveBtn;

	private UIAnimBase animBase;

	public Sprite comingSoonSprite;

	private RectTransform prophesyBookRectTransform;

	private static readonly PlayerOccupation[] OCCUPATION_ARRAY = new PlayerOccupation[3]
	{
		PlayerOccupation.Archer,
		PlayerOccupation.Knight,
		PlayerOccupation.None
	};

	private static readonly Color LOCKED_COLOR = "E5E3E2FF".HexColorToColor();

	private static readonly Color UNLOCKED_COLOR = Color.white;

	private bool isUnlockProphesy;

	private bool isShowingPresuppositionList;

	private int currentShowingOccupationIndex;

	private int currentChosenPresuppositionIndex;

	private AudioController _audioController;

	public override string UIViewName => "ChooseOccupationUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		_audioController = SingletonDontDestroy<AudioManager>.Instance.GetUnuseSoundAudioController();
		_audioController.IsPrivate = true;
		OnShowChooseOccupationUI();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		HidePresuppositionList();
		RecycleAudioController();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		infomationRoot = base.transform.Find("Bg/AllInfomation");
		illustrationImg = base.transform.Find("Bg/IllustrationImg").GetComponent<Image>();
		nameBottomImg = infomationRoot.Find("NameBottom").GetComponent<Image>();
		nameText = infomationRoot.Find("NameBottom/Name").GetComponent<Text>();
		maxHealthText = infomationRoot.Find("MaxHealthBottom/Amount").GetComponent<Text>();
		initMoneyText = infomationRoot.Find("InitMoneyBottom/Amount").GetComponent<Text>();
		occupationDesText = infomationRoot.Find("OccupationDes").GetComponent<Text>();
		specificNameText = infomationRoot.Find("SpecificName").GetComponent<Text>();
		specificDesText = infomationRoot.Find("SpecificDes").GetComponent<Text>();
		prophesyBgTrans = infomationRoot.Find("ProphesyDesTmpContent");
		prophesyDesText = prophesyBgTrans.Find("ProphesyDesBg/Content").GetComponent<Text>();
		prophesyTmpDesText = prophesyBgTrans.GetComponent<Text>();
		presuppositionRoot = infomationRoot.Find("PresuppositionList");
		allSinglePresuppositionItems = new Button[presuppositionRoot.childCount];
		allSinglePresuppositionNameTexts = new Text[presuppositionRoot.childCount];
		for (int i = 0; i < allSinglePresuppositionItems.Length; i++)
		{
			allSinglePresuppositionItems[i] = presuppositionRoot.GetChild(i).GetComponent<Button>();
			allSinglePresuppositionNameTexts[i] = presuppositionRoot.GetChild(i).Find("Text").GetComponent<Text>();
			int index = i;
			allSinglePresuppositionItems[index].onClick.AddListener(delegate
			{
				OnClickChosenPresupposition(index);
			});
		}
		currentShowingPresuppositionNameText = infomationRoot.Find("PresuppositionButton/PresuppositionName").GetComponent<Text>();
		prophesyBtnImg = base.transform.Find("Bg/AllInfomation/ProphesyBtn").GetComponent<Image>();
		infomationRoot.Find("PresuppositionButton").GetComponent<Button>().onClick.AddListener(OnClickPresuppositionBtn);
		infomationRoot.Find("PurchaseBtn").GetComponent<Button>().onClick.AddListener(OnClickPurchaseBtn);
		leftMoveBtn = base.transform.Find("Bg/LeftMoveBtn").GetComponent<Button>();
		leftMoveBtn.onClick.AddListener(OnClickLeftMoveBtn);
		rightMoveBtn = base.transform.Find("Bg/RightMoveBtn").GetComponent<Button>();
		rightMoveBtn.onClick.AddListener(OnClickRightMoveBtn);
		purchaseBtn = infomationRoot.Find("PurchaseBtn").GetComponent<Button>();
		startBtn = base.transform.Find("Bg/StartBtn").GetComponent<Button>();
		startBtn.onClick.AddListener(OnClickStartBtn);
		base.transform.Find("Bg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
		comingSoonText = base.transform.Find("Bg/ComingSoon").GetComponent<Text>();
		animBase = GetComponent<UIAnimBase>();
		OnMouseEventCallback component = base.transform.Find("Bg/AllInfomation/ProphesyBtn").GetComponent<OnMouseEventCallback>();
		prophesyBookRectTransform = base.transform.Find("Bg/AllInfomation/ProphesyBtn").GetComponent<RectTransform>();
		component.EnterEventTrigger.Event.AddListener(ShowProphesyBookHint);
		component.ExitEventTrigger.Event.AddListener(HideProphesyBookHint);
	}

	private void ShowProphesyBookHint()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(prophesyBookRectTransform, "PROPHESYBOOKHOVERHINT".LocalizeText(), null));
	}

	private void HideProphesyBookHint()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	private void OnShowChooseOccupationUI()
	{
		leftMoveBtn.gameObject.SetActive(value: false);
		rightMoveBtn.gameObject.SetActive(value: true);
		currentShowingOccupationIndex = 0;
		LoadOccupationInfo(OCCUPATION_ARRAY[0]);
	}

	private void LoadOccupationInfo(PlayerOccupation occupation)
	{
		if (occupation == PlayerOccupation.None)
		{
			LoadComingSoon();
		}
		else
		{
			LoadNormalOccupationInfo(occupation);
		}
		animBase.StartAnim();
	}

	private void LoadNormalOccupationInfo(PlayerOccupation occupation)
	{
		comingSoonText.gameObject.SetActive(value: false);
		infomationRoot.gameObject.SetActive(value: true);
		isShowingPresuppositionList = false;
		currentChosenPresuppositionIndex = 0;
		OccupationData occupationData = DataManager.Instance.GetOccupationData(occupation);
		OccupationInitSetting playerOccupationInitSetting = DataManager.Instance.GetPlayerOccupationInitSetting(occupation);
		illustrationImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.ChooseOccuUIIllustrationSpriteName, occupationData.DefaultSpritePath);
		nameText.text = occupationData.OccupationNameKey.LocalizeText();
		nameBottomImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.ChooseOccuUINameBottomSpriteName, occupationData.DefaultSpritePath);
		occupationDesText.text = occupationData.OccupationSimplifyDesKey.LocalizeText();
		specificNameText.text = occupationData.OccupationSpecificNameKey.LocalizeText();
		specificDesText.text = occupationData.OccupationSimplifySpecificDesKey.LocalizeText();
		maxHealthText.text = playerOccupationInitSetting.MaxHealth.ToString();
		initMoneyText.text = playerOccupationInitSetting.InitMoney.ToString();
		if (Varification.CheckPlayerOccupationUnlocked(occupation))
		{
			SetOccupationUnlocked();
		}
		else
		{
			SetOccupationLocked();
		}
		string text = occupationData.OccupationProphesyKey.LocalizeText();
		prophesyDesText.text = text;
		prophesyTmpDesText.text = text.Replace(LocalizationManager.Instance.colorPallet["<关键词>"], "").Replace("</color>", "");
		CardPresuppositionStruct cardPresuppositionStruct = SingletonDontDestroy<Game>.Instance.CurrentUserData.GetAllCardPresuppositionsByOccupation(occupation)[0];
		currentShowingPresuppositionNameText.text = (cardPresuppositionStruct.isDefault ? cardPresuppositionStruct.Name.LocalizeText() : cardPresuppositionStruct.Name);
		PlayOccupationSound(occupationData);
	}

	private void PlayOccupationSound(OccupationData data)
	{
		if (_audioController.IsPlaying)
		{
			_audioController.PauseAudio();
		}
		string[] array = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? data.ChooseSounds : data.ChooseEnSounds);
		string soundName = array[Random.Range(0, array.Length)];
		AudioClip clip = SingletonDontDestroy<ResourceManager>.Instance.LoadSound(soundName);
		_audioController.PlayAudio(clip, isLoop: false);
	}

	private void RecycleAudioController()
	{
		_audioController.IsPrivate = false;
		SingletonDontDestroy<AudioManager>.Instance.RecycleAudioController(_audioController);
		_audioController = null;
	}

	private void LoadComingSoon()
	{
		infomationRoot.gameObject.SetActive(value: false);
		comingSoonText.gameObject.SetActive(value: true);
		illustrationImg.sprite = comingSoonSprite;
	}

	private void SetOccupationLocked()
	{
		purchaseBtn.gameObject.SetActive(value: true);
		illustrationImg.color = LOCKED_COLOR;
		isUnlockProphesy = false;
		prophesyBtnImg.gameObject.SetActive(value: true);
		startBtn.interactable = false;
	}

	private void SetOccupationUnlocked()
	{
		purchaseBtn.gameObject.SetActive(value: false);
		illustrationImg.color = UNLOCKED_COLOR;
		isUnlockProphesy = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard;
		prophesyBtnImg.gameObject.SetActive(!isUnlockProphesy);
		prophesyTmpDesText.gameObject.SetActive(isUnlockProphesy);
		startBtn.interactable = true;
	}

	private void OnClickChosenPresupposition(int index)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("选人界面_修改预设按钮");
		if (currentChosenPresuppositionIndex != index)
		{
			currentChosenPresuppositionIndex = index;
			PlayerOccupation playerOccupation = OCCUPATION_ARRAY[currentShowingOccupationIndex];
			CardPresuppositionStruct cardPresuppositionStruct = SingletonDontDestroy<Game>.Instance.CurrentUserData.GetAllCardPresuppositionsByOccupation(playerOccupation)[index];
			currentShowingPresuppositionNameText.text = (cardPresuppositionStruct.isDefault ? cardPresuppositionStruct.Name.LocalizeText() : cardPresuppositionStruct.Name);
			HidePresuppositionList();
		}
	}

	private void OnClickRightMoveBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
		HidePresuppositionList();
		if (currentShowingOccupationIndex < OCCUPATION_ARRAY.Length - 1)
		{
			currentShowingOccupationIndex++;
			LoadOccupationInfo(OCCUPATION_ARRAY[currentShowingOccupationIndex]);
			if (currentShowingOccupationIndex >= OCCUPATION_ARRAY.Length - 1)
			{
				rightMoveBtn.gameObject.SetActive(value: false);
				startBtn.interactable = false;
			}
			else if (currentShowingOccupationIndex == 1)
			{
				leftMoveBtn.gameObject.SetActive(value: true);
			}
		}
	}

	private void OnClickLeftMoveBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
		HidePresuppositionList();
		if (currentShowingOccupationIndex > 0)
		{
			currentShowingOccupationIndex--;
			LoadOccupationInfo(OCCUPATION_ARRAY[currentShowingOccupationIndex]);
			if (currentShowingOccupationIndex <= 0)
			{
				leftMoveBtn.gameObject.SetActive(value: false);
			}
			else if (currentShowingOccupationIndex == OCCUPATION_ARRAY.Length - 2)
			{
				startBtn.interactable = true;
				rightMoveBtn.gameObject.SetActive(value: true);
			}
		}
	}

	private void OnClickPurchaseBtn()
	{
	}

	private void OnClickPresuppositionBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("选人界面_修改预设按钮");
		if (isShowingPresuppositionList)
		{
			HidePresuppositionList();
		}
		else
		{
			ShowPresuppositionList();
		}
	}

	private void HidePresuppositionList()
	{
		isShowingPresuppositionList = false;
		presuppositionRoot.gameObject.SetActive(value: false);
	}

	private void ShowPresuppositionList()
	{
		isShowingPresuppositionList = true;
		presuppositionRoot.gameObject.SetActive(value: true);
		LoadCurrentOccupationPresupposition();
	}

	private void LoadCurrentOccupationPresupposition()
	{
		PlayerOccupation playerOccupation = OCCUPATION_ARRAY[currentShowingOccupationIndex];
		List<CardPresuppositionStruct> allCardPresuppositionsByOccupation = SingletonDontDestroy<Game>.Instance.CurrentUserData.GetAllCardPresuppositionsByOccupation(playerOccupation);
		for (int i = 0; i < allCardPresuppositionsByOccupation.Count; i++)
		{
			allSinglePresuppositionItems[i].gameObject.SetActive(value: true);
			allSinglePresuppositionNameTexts[i].text = (allCardPresuppositionsByOccupation[i].isDefault ? allCardPresuppositionsByOccupation[i].Name.LocalizeText() : allCardPresuppositionsByOccupation[i].Name);
		}
		for (int j = allCardPresuppositionsByOccupation.Count; j < allSinglePresuppositionItems.Length; j++)
		{
			allSinglePresuppositionItems[j].gameObject.SetActive(value: false);
		}
	}

	private void OnClickStartBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("选人界面_开始轮回按钮");
		SingletonDontDestroy<Game>.Instance.playerOccupation = OCCUPATION_ARRAY[currentShowingOccupationIndex];
		SingletonDontDestroy<Game>.Instance.presuppositionIndex = currentChosenPresuppositionIndex;
		SingletonDontDestroy<Game>.Instance.SwitchScene(2);
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameMenuUI");
	}
}
