using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : UIView, ILocalization
{
	public Sprite CNTitleSprite;

	public Sprite ENTitleSprite;

	private Image maskImg;

	private Button continueBtn;

	private Text userNameText;

	private Image arrowImg;

	private Image titleImg;

	private Transform[] allArrowReference;

	private Button shopBtn;

	private Button presuppositionBtn;

	private Button recordBtn;

	private Button illustratedBooksBtn;

	private CanvasGroup _canvasGroup;

	private Button EABlockRaycast;

	private Transform boardTrans;

	private Text presuppositionBtnText;

	private Text shopBtnText;

	private Text userDataBtnText;

	private Text helpingBtnText;

	private Text illustratedBookBtnText;

	private Text recordBtnText;

	private Text newerGuideBtnText;

	private Text newGameBtnText;

	private Text continueBtnText;

	private Text settingBtnText;

	private Text quitBtnText;

	private Transform bgTransform;

	public override string UIViewName => "GameMenuUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowGameMenuUI();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Game Menu UI");
	}

	public override void OnSpawnUI()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		base.transform.Find("Bg/StartNewGameBtn").GetComponent<Button>().onClick.AddListener(OnClickStartNewGameBtn);
		continueBtn = base.transform.Find("Bg/ContinueBtn").GetComponent<Button>();
		continueBtn.onClick.AddListener(OnClickContinueBtn);
		base.transform.Find("Bg/SettingBtn").GetComponent<Button>().onClick.AddListener(OnClickSettingBtn);
		userNameText = base.transform.Find("Bg/UserDataBtn/UserName").GetComponent<Text>();
		maskImg = base.transform.Find("Mask").GetComponent<Image>();
		base.transform.Find("Bg/QuitBtn").GetComponent<Button>().onClick.AddListener(OnClickQuitBtn);
		arrowImg = base.transform.Find("Bg/Arrow").GetComponent<Image>();
		Transform transform = base.transform.Find("Bg/ArrowReference");
		allArrowReference = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			allArrowReference[i] = transform.GetChild(i);
		}
		shopBtn = base.transform.Find("Bg/ShopBtn").GetComponent<Button>();
		shopBtn.onClick.AddListener(OnClickShopBtn);
		presuppositionBtn = base.transform.Find("Bg/PresuppositionBtn").GetComponent<Button>();
		presuppositionBtn.onClick.AddListener(OnShowPresuppositionUI);
		base.transform.Find("Bg/UserDataBtn").GetComponent<Button>().onClick.AddListener(OnClickUserDataBtn);
		base.transform.Find("Bg/HelpingBtn").GetComponent<Button>().onClick.AddListener(OnClickHelpingBtn);
		illustratedBooksBtn = base.transform.Find("Bg/IllustratedBooksBtn").GetComponent<Button>();
		illustratedBooksBtn.onClick.AddListener(OnClickIllustratedBooksBtn);
		recordBtn = base.transform.Find("Bg/RecordBtn").GetComponent<Button>();
		recordBtn.onClick.AddListener(OnClickRecordBtn);
		base.transform.Find("Bg/NewGuideBtn").GetComponent<Button>().onClick.AddListener(OnClickNewGuideBtn);
		InitForAnim();
		Localization();
	}

	public void Localization()
	{
		base.transform.Find("Bg/TitleImg").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNTitleSprite : ENTitleSprite);
	}

	private void ShowGameMenuUI()
	{
		StartCoroutine(ShowGameMenuUIAnim_IE());
	}

	private void ShowEvilDragoKrystal()
	{
		if (SingletonDontDestroy<Game>.Instance.CurrentUserData != null && SingletonDontDestroy<Game>.Instance.CurrentUserData.IsDefeatEvilDrag && !SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked("Plot_1"))
		{
			SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockPlot("Plot_1");
			SingletonDontDestroy<UIManager>.Instance.ShowView("PlotUI", "Plot_1", null);
		}
	}

	private void CheckAppData()
	{
		if (SingletonDontDestroy<Game>.Instance.AppData == null)
		{
			CreateNewAppData();
		}
	}

	private void CreateNewAppData()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameMenuUI");
		SingletonDontDestroy<Game>.Instance.CreateNewAppData();
		GameSave.DeleteOldSaveData();
	}

	private void InitForAnim()
	{
		bgTransform = base.transform.Find("Bg");
		presuppositionBtnText = bgTransform.Find("PresuppositionBtn/Text").GetComponent<Text>();
		shopBtnText = bgTransform.Find("ShopBtn/Text").GetComponent<Text>();
		userDataBtnText = bgTransform.Find("UserDataBtn/Text").GetComponent<Text>();
		helpingBtnText = bgTransform.Find("HelpingBtn/Text").GetComponent<Text>();
		illustratedBookBtnText = bgTransform.Find("IllustratedBooksBtn/Text").GetComponent<Text>();
		recordBtnText = bgTransform.Find("RecordBtn/Text").GetComponent<Text>();
		newerGuideBtnText = bgTransform.Find("NewGuideBtn/Text").GetComponent<Text>();
		newGameBtnText = bgTransform.Find("StartNewGameBtn").GetComponent<Text>();
		SetBtnMouseEventCallback(newGameBtnText, 0);
		continueBtnText = bgTransform.Find("ContinueBtn").GetComponent<Text>();
		SetBtnMouseEventCallback(continueBtnText, 1);
		settingBtnText = bgTransform.Find("SettingBtn").GetComponent<Text>();
		SetBtnMouseEventCallback(settingBtnText, 2);
		quitBtnText = bgTransform.Find("QuitBtn").GetComponent<Text>();
		SetBtnMouseEventCallback(quitBtnText, 3);
	}

	private void SetBtnMouseEventCallback(Text text, int index)
	{
		OnMouseEventCallback component = text.GetComponent<OnMouseEventCallback>();
		component.EnterEventTrigger.Event.AddListener(delegate
		{
			ArrowSet(index, text.GetComponent<Button>());
		});
		component.ExitEventTrigger.Event.AddListener(HideArrow);
	}

	private IEnumerator ShowGameMenuUIAnim_IE()
	{
		_canvasGroup.blocksRaycasts = false;
		ResetGameMenuUI();
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("标题界面");
		SetContinuteBtnInteractive(GameSave.IsHaveSaveData);
		userNameText.text = ((SingletonDontDestroy<Game>.Instance.CurrentUserData != null) ? SingletonDontDestroy<Game>.Instance.CurrentUserData.UserName : string.Empty);
		SetFunctionButtons(SingletonDontDestroy<Game>.Instance.CurrentUserData != null && SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEverFinishGame);
		bgTransform.DOLocalRotate(Vector3.zero, 1.2f);
		bgTransform.DOScale(1f, 1.2f);
		maskImg.DOColor(Color.clear, 2f);
		yield return new WaitForSeconds(1.2f);
		maskImg.gameObject.SetActive(value: false);
		HandleFunctionBtnTextAnim(helpingBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(userDataBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(recordBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(illustratedBookBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(shopBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(presuppositionBtnText);
		yield return new WaitForSeconds(0.1f);
		HandleFunctionBtnTextAnim(newerGuideBtnText);
		yield return new WaitForSeconds(0.1f);
		_canvasGroup.blocksRaycasts = true;
		EABlockRaycast = base.transform.Find("EAHint").GetComponent<Button>();
		EABlockRaycast.onClick.AddListener(CloseEABoard);
		boardTrans = EABlockRaycast.transform.Find("FrameBG");
		BoardOpenAnim();
	}

	public void OnConfirmSwitchUserData()
	{
		ShowGameMenuUI();
	}

	private void SetContinuteBtnInteractive(bool isInteractive)
	{
		continueBtn.interactable = isInteractive;
	}

	private void HandleFunctionBtnTextAnim(Text text)
	{
		float endValue = text.transform.localPosition.y + 30f;
		text.DOFade(1f, 0.5f);
		text.transform.DOLocalMoveY(endValue, 0.5f);
	}

	private void ResetGameMenuUI()
	{
		maskImg.gameObject.SetActive(value: true);
		maskImg.color = Color.black;
		bgTransform.localScale = Vector3.one * 1.3f;
		bgTransform.localRotation = Quaternion.Euler(0f, 0f, 6f);
		presuppositionBtnText.color = GetTransparentColor(presuppositionBtnText.color);
		shopBtnText.color = GetTransparentColor(shopBtnText.color);
		userDataBtnText.color = GetTransparentColor(userDataBtnText.color);
		helpingBtnText.color = GetTransparentColor(helpingBtnText.color);
		illustratedBookBtnText.color = GetTransparentColor(illustratedBookBtnText.color);
		recordBtnText.color = GetTransparentColor(recordBtnText.color);
		newerGuideBtnText.color = GetTransparentColor(newerGuideBtnText.color);
		presuppositionBtnText.transform.localPosition += Vector3.down * 30f;
		shopBtnText.transform.localPosition += Vector3.down * 30f;
		userDataBtnText.transform.localPosition += Vector3.down * 30f;
		helpingBtnText.transform.localPosition += Vector3.down * 30f;
		illustratedBookBtnText.transform.localPosition += Vector3.down * 30f;
		recordBtnText.transform.localPosition += Vector3.down * 30f;
		newerGuideBtnText.transform.localPosition += Vector3.down * 30f;
	}

	private Color GetTransparentColor(Color targetColor)
	{
		return new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
	}

	private void SetFunctionButtons(bool isActiveSelf)
	{
		shopBtn.gameObject.SetActive(isActiveSelf);
		presuppositionBtn.gameObject.SetActive(isActiveSelf);
		recordBtn.gameObject.SetActive(isActiveSelf);
		illustratedBooksBtn.gameObject.SetActive(isActiveSelf);
	}

	public void ArrowSet(int index, Button btn)
	{
		if (btn.interactable)
		{
			arrowImg.gameObject.SetActive(value: true);
			arrowImg.transform.position = allArrowReference[index].position;
		}
	}

	public void HideArrow()
	{
		arrowImg.gameObject.SetActive(value: false);
	}

	private void OnClickNewGuideBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入新手指引界面");
		GuideSystem.Instance.StartBattleGuide();
	}

	private void OnClickRecordBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("RecordUI");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入战绩界面");
	}

	private void OnClickIllustratedBooksBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("IllustratedBooksUI");
	}

	private void OnClickHelpingBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("HelpUI");
	}

	private void OnClickUserDataBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入用户档案");
		SingletonDontDestroy<UIManager>.Instance.ShowView("UserDataUI");
	}

	private void OnClickShopBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入黯晶袋");
		SingletonDontDestroy<UIManager>.Instance.ShowView("PurchasedItemUI");
	}

	public void SetUserName(string name)
	{
		userNameText.text = name;
	}

	private void OnClickQuitBtn()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ConfirmExitGame".LocalizeText(), OnConfirmQuitGame);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void OnConfirmQuitGame()
	{
		Application.Quit();
	}

	private void OnShowPresuppositionUI()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("CardPresuppositionUI");
	}

	private void OnClickStartNewGameBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		if (GameSave.IsHaveSaveData)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("startNewGameDeleteDataQuestion".LocalizeText(), OnComfirmDeleteOldDataAndStartNewGame);
		}
		else
		{
			OnComfirmStartNewGame();
		}
	}

	private void OnComfirmStartNewGame()
	{
		SingletonDontDestroy<Game>.Instance.IsTryLoadSaveData = false;
		SceneManager.FirstEnterMenu = false;
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseOccupationUI");
	}

	private void OnComfirmDeleteOldDataAndStartNewGame()
	{
		GameSave.DeleteOldSaveData();
		SingletonDontDestroy<Game>.Instance.IsTryLoadSaveData = false;
		SingletonDontDestroy<Game>.Instance.CurrentUserData.SetPreEndMapLevel(0);
		GameSave.SaveUserData();
		SceneManager.FirstEnterMenu = false;
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseOccupationUI");
	}

	private void OnClickContinueBtn()
	{
		SingletonDontDestroy<Game>.Instance.IsTryLoadSaveData = true;
		maskImg.color = Color.clear;
		maskImg.gameObject.SetActive(value: true);
		maskImg.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			SingletonDontDestroy<Game>.Instance.SwitchScene(2);
		});
		SceneManager.FirstEnterMenu = false;
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void OnClickSettingBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("SettingUI", true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void BoardOpenAnim()
	{
		if (SceneManager.FirstEnterMenu)
		{
			SceneManager.FirstEnterMenu = false;
			EABlockRaycast.gameObject.SetActive(value: true);
			EABlockRaycast.enabled = false;
			boardTrans.localScale = Vector3.zero;
			boardTrans.DOScale(1f, 0.3f).OnComplete(delegate
			{
				EABlockRaycast.enabled = true;
			});
		}
		else
		{
			EABlockRaycast.gameObject.SetActive(value: false);
			ShowEvilDragoKrystal();
		}
	}

	private void CloseEABoard()
	{
		boardTrans.DOKill();
		boardTrans.DOScale(0f, 0.2f).OnComplete(delegate
		{
			CheckAppData();
			EABlockRaycast.gameObject.SetActive(value: false);
			ShowEvilDragoKrystal();
		});
	}
}
