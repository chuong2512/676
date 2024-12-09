using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : EscCloseUIView
{
	private Scrollbar scrollbar;

	private Transform bgTrans;

	private Button returnBtn;

	private Button creditBtn;

	private ExtentSlider soundVolumeSlider;

	private ExtentSlider musicVolumeSlider;

	private Dropdown resolutionDropdown;

	private Dropdown maxframeDropdown;

	private Dropdown windowtypeDropdown;

	private Dropdown languageDropdown;

	private Transform languageRoot;

	public override string UIViewName => "SettingUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		LoadPlayerSetting();
		bgTrans.localScale = Vector3.zero;
		bgTrans.DOScale(1f, 0.2f);
		bool flag = (bool)objs[0];
		returnBtn.gameObject.SetActive(!flag);
		if (flag && SingletonDontDestroy<SettingManager>.Instance.Language == 0)
		{
			creditBtn.gameObject.SetActive(value: true);
		}
		else
		{
			creditBtn.gameObject.SetActive(value: false);
		}
		languageRoot.gameObject.SetActive(flag);
		SetScrollbar();
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickCloseBtn();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Setting UI...");
	}

	public override void OnSpawnUI()
	{
		InitUsualFunction();
		InitSound();
		InitGraphic();
		InitLanguage();
		scrollbar = base.transform.Find("Mask/Bg/SettingItemRoot/Scrollbar").GetComponent<Scrollbar>();
	}

	private void LoadPlayerSetting()
	{
		LoadPlayerSoundSetting();
		LoadPlayerGraphicSetting();
		LoadPlayerLanguageSetting();
	}

	private void SetScrollbar()
	{
		StartCoroutine(SetScrollbar_IE());
	}

	private IEnumerator SetScrollbar_IE()
	{
		yield return null;
		scrollbar.value = 1f;
	}

	private void InitUsualFunction()
	{
		returnBtn = base.transform.Find("Mask/Bg/ReturnBtn").GetComponent<Button>();
		returnBtn.onClick.AddListener(OnClickReturnBtn);
		creditBtn = base.transform.Find("Mask/Bg/CreditBtn").GetComponent<Button>();
		creditBtn.onClick.AddListener(OnClickCreditBtn);
		base.transform.Find("Mask/Bg/CloseBtn").GetComponent<Button>().onClick.AddListener(OnClickCloseBtn);
		bgTrans = base.transform.Find("Mask/Bg");
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ConfirmReturnMenu?".LocalizeText(), OnConfirmReturn);
	}

	private void OnClickCreditBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("NameListUI");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void OnConfirmReturn()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI") as MaskUI).ShowMask(delegate
		{
			BattleEnvironmentManager.Instance.HideBg();
			Singleton<EnemyController>.Instance.RecycleAllEnemy();
			SingletonDontDestroy<UIManager>.Instance.HideAllShowingView();
			SingletonDontDestroy<Game>.Instance.SwitchScene(1);
		});
		EventManager.BroadcastEvent(EventEnum.E_BackToMenu, null);
	}

	private void InitSound()
	{
		Transform transform = base.transform.Find("Mask/Bg/SettingItemRoot/Mask/Content/Sound");
		soundVolumeSlider = transform.Find("SoundVolume/Slider").GetComponent<ExtentSlider>();
		soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
		soundVolumeSlider.MOnPointUp.AddListener(OnSliderPointerUp);
		musicVolumeSlider = transform.Find("MusicVolume/Slider").GetComponent<ExtentSlider>();
		musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
		musicVolumeSlider.MOnPointUp.AddListener(OnSliderPointerUp);
	}

	private void LoadPlayerSoundSetting()
	{
		soundVolumeSlider.value = (SingletonDontDestroy<SettingManager>.Instance.SoundVolume);
		musicVolumeSlider.value = (SingletonDontDestroy<SettingManager>.Instance.MusicVolume);
	}

	private void OnMusicVolumeChanged(float value)
	{
		SingletonDontDestroy<SettingManager>.Instance.ChangeMusicVolume(value);
		SingletonDontDestroy<AudioManager>.Instance.ChangeMusicVolume(value);
	}

	private void OnSliderPointerUp()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private void OnSoundVolumeChanged(float value)
	{
		SingletonDontDestroy<SettingManager>.Instance.ChangeSoundVolume(value);
		SingletonDontDestroy<AudioManager>.Instance.ChangeSoundVolume(value);
	}

	private void OnClickCloseBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用_关闭按钮");
		SingletonDontDestroy<SettingManager>.Instance.SaveCurrentSettingToSettingFile();
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void InitGraphic()
	{
		Transform transform = base.transform.Find("Mask/Bg/SettingItemRoot/Mask/Content/Graphic");
		resolutionDropdown = transform.Find("Resolution/Dropdown").GetComponent<Dropdown>();
		resolutionDropdown.onValueChanged.AddListener(ResolutionOnValueChanged);
		maxframeDropdown = transform.Find("MaxFrame/Dropdown").GetComponent<Dropdown>();
		maxframeDropdown.onValueChanged.AddListener(MaxFrameOnValueChanged);
		windowtypeDropdown = transform.Find("WindowModel/Dropdown").GetComponent<Dropdown>();
		windowtypeDropdown.onValueChanged.AddListener(WindowTypeOnValueChanged);
		resolutionDropdown.options = SingletonDontDestroy<WindowsSetting>.Instance.GetAllDropdownOptionDatas();
		List<Dropdown.OptionData> list = new List<Dropdown.OptionData>(3);
		list.Add(new Dropdown.OptionData
		{
			text = "30"
		});
		list.Add(new Dropdown.OptionData
		{
			text = "60"
		});
		list.Add(new Dropdown.OptionData
		{
			text = "None".LocalizeText()
		});
		maxframeDropdown.options = list;
		List<Dropdown.OptionData> list2 = new List<Dropdown.OptionData>(3);
		list2.Add(new Dropdown.OptionData
		{
			text = "FullScreen".LocalizeText()
		});
		list2.Add(new Dropdown.OptionData
		{
			text = "FullScreenNoFrame".LocalizeText()
		});
		list2.Add(new Dropdown.OptionData
		{
			text = "WindowsModel".LocalizeText()
		});
		windowtypeDropdown.options = list2;
	}

	private void LoadPlayerGraphicSetting()
	{
		resolutionDropdown.value = (SingletonDontDestroy<WindowsSetting>.Instance.GetCurrentDropdownValue());
		maxframeDropdown.value = (SingletonDontDestroy<SettingManager>.Instance.MaxFrame);
		windowtypeDropdown.value =(SingletonDontDestroy<SettingManager>.Instance.WindowType);
	}

	private void ResolutionOnValueChanged(int value)
	{
		SingletonDontDestroy<WindowsSetting>.Instance.OnResolutionDropdownValueChanged(value);
	}

	private void MaxFrameOnValueChanged(int value)
	{
		SingletonDontDestroy<SettingManager>.Instance.ChangeMaxFrame(value);
		GameFrameSetting.Instance.SetSettingGameFrame();
	}

	private void WindowTypeOnValueChanged(int value)
	{
		SingletonDontDestroy<SettingManager>.Instance.ChangeWindowType(value);
		SingletonDontDestroy<WindowsSetting>.Instance.SetWindowType(broadcastEvent: true);
	}

	private void InitLanguage()
	{
		languageRoot = base.transform.Find("Mask/Bg/SettingItemRoot/Mask/Content/Language");
		languageDropdown = languageRoot.Find("Language/Dropdown").GetComponent<Dropdown>();
		languageDropdown.onValueChanged.AddListener(LanguageOnValueChanged);
	}

	private void LoadPlayerLanguageSetting()
	{
	//	languageDropdown.SetValueWithoutNotify(SingletonDontDestroy<SettingManager>.Instance.Language);
        languageDropdown.value = (SingletonDontDestroy<SettingManager>.Instance.Language);
    }

	private void LanguageOnValueChanged(int value)
	{
		SingletonDontDestroy<SettingManager>.Instance.ChangeLanguage(value);
		SingletonDontDestroy<SettingManager>.Instance.SaveCurrentSettingToSettingFile();
		LocalizationManager.Instance.InitLocalizationManager("CD_EN.json");
		SingletonDontDestroy<UIManager>.Instance.DestoryAllView();
		SingletonDontDestroy<Game>.Instance.SwitchGameState(new GameMenuState());
	}
}
