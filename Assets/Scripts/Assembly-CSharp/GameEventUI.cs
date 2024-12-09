using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameEventUI : UIView
{
	private const string GameEventAssetPath = "Sprites/GameEvent";

	private const int OptionMaxAmount = 6;

	private Image eventIllustration;

	private Text titleText;

	private Text contentText;

	private SingleEventBtnCtrl[] allEventBtnCtrls;

	private UsualHealthBarCtrl _healthBarCtrl;

	private Text coinAmountText;

	private UIAnim_GameEvent uiAnimGameEvent;

	private CanvasGroup m_CanvasGroup;

	private bool isShowingUI;

	private RectTransform eventBgRectTransform;

	public override string UIViewName => "GameEventUI";

	public override string UILayerName => "NormalLayer";

	public SingleEventBtnCtrl[] AllEventBtnCtrls => allEventBtnCtrls;

	public UsualHealthBarCtrl HealthBarCtrl => _healthBarCtrl;

	public Transform CoinAmountTrans => coinAmountText.transform;

	public bool isBtnActive { get; private set; }

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		m_CanvasGroup.DOFade(0f, 0.4f).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Game event ui...");
	}

	public override void OnSpawnUI()
	{
		_healthBarCtrl = base.transform.Find("Mask/EventBg/DetailPanel/UsualHealthBar").GetComponent<UsualHealthBarCtrl>();
		coinAmountText = base.transform.Find("Mask/EventBg/DetailPanel/CoinAmount").GetComponent<Text>();
		eventIllustration = base.transform.Find("Mask/EventBg/DetailPanel/EventIllustration/Mask/Illustration").GetComponent<Image>();
		titleText = base.transform.Find("Mask/EventBg/DetailPanel/Title").GetComponent<Text>();
		contentText = base.transform.Find("Mask/EventBg/DetailPanel/Content").GetComponent<Text>();
		allEventBtnCtrls = new SingleEventBtnCtrl[6];
		eventBgRectTransform = base.transform.Find("Mask/EventBg").GetComponent<RectTransform>();
		uiAnimGameEvent = base.transform.GetComponent<UIAnim_GameEvent>();
		uiAnimGameEvent.SetBtnTrueAct = delegate
		{
			isBtnActive = true;
		};
		Transform transform = base.transform.Find("Mask/EventBg/EventBtnList");
		for (int i = 0; i < 6; i++)
		{
			allEventBtnCtrls[i] = transform.GetChild(i).GetComponent<SingleEventBtnCtrl>();
		}
		m_CanvasGroup = GetComponent<CanvasGroup>();
	}

	public void SetBtnActive(bool isActive)
	{
		for (int i = 0; i < allEventBtnCtrls.Length; i++)
		{
			allEventBtnCtrls[i].SetBtnInteractive(isActive);
		}
	}

	public void LoadEvent(string eventCode, List<Action> options, List<bool> optionInteracitve, List<bool> occupationLimit)
	{
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(eventCode);
		SetEventUIBaseInfo(gameEventData);
		for (int i = 0; i < options.Count; i++)
		{
			allEventBtnCtrls[i].LoadBtnInfo(this, gameEventData.OptionDatas[i], options[i], optionInteracitve[i]);
			allEventBtnCtrls[i].gameObject.SetActive(occupationLimit[i]);
		}
		for (int j = options.Count; j < allEventBtnCtrls.Length; j++)
		{
			allEventBtnCtrls[j].gameObject.SetActive(value: false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(eventBgRectTransform);
		isBtnActive = false;
		uiAnimGameEvent.StartAnim();
	}

	public void LoadEvent(string eventCode, List<Action> options, List<OptionData> optionDatas, List<bool> optionInteracitve, List<bool> occupationLimit)
	{
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(eventCode);
		SetEventUIBaseInfo(gameEventData);
		for (int i = 0; i < options.Count; i++)
		{
			allEventBtnCtrls[i].gameObject.SetActive(occupationLimit[i]);
			allEventBtnCtrls[i].LoadBtnInfo(this, optionDatas[i], options[i], optionInteracitve[i]);
		}
		for (int j = options.Count; j < allEventBtnCtrls.Length; j++)
		{
			allEventBtnCtrls[j].gameObject.SetActive(value: false);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(eventBgRectTransform);
		isBtnActive = false;
		uiAnimGameEvent.StartAnim();
	}

	private void SetEventUIBaseInfo(GameEventData data)
	{
		eventIllustration.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.IllustrationName, "Sprites/GameEvent");
		titleText.text = data.TitleKey.LocalizeText();
		contentText.text = data.ContentKey.LocalizeText();
		UpdatePlayerCoinAmount();
		_healthBarCtrl.LoadHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
	}

	public void SetBtnInteractive(int index, bool interavtive)
	{
		allEventBtnCtrls[index].SetBtnInteractive(interavtive);
	}

	public void UpdatePlayerCoinAmount()
	{
		coinAmountText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
	}
}
