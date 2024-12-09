using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GamblingUI : UIView
{
	private const string VfxEquipCardDrag = "effect_general_trail_jinengzhuangbei";

	private const string VfxHealthDrag = "effect_general_trail_shengming";

	private const string VfxGamlingVFx = "赌博水晶球";

	private const string InitVfx = "水晶球";

	public Sprite CardGainResultSprite;

	public Sprite HealthGainResultSprite;

	public Sprite CoinGainResultSprite;

	public Sprite EquipGainResultSprite;

	public Sprite coinGamblingBgSprite_Top;

	public Sprite healthGamblingBgSprite_Top;

	public Sprite coinGamblingBgSprite_Bot;

	public Sprite healthGamblingBgSprite_Bot;

	private UIAnim_Gambling anim;

	private Transform crystalPoint;

	private Text gamblingBtnText;

	private Text playerCoinRemainText;

	private Transform gemCentrePoint;

	private Transform[] gemImgArray;

	private Image[] gemsImg;

	private Transform[] gemImgStartPointArray;

	private Image resultImg;

	private UsualHealthBarCtrl _usualHealthBarCtrl;

	private Button startGamblingBtn;

	private Button quitBtn;

	private Action CollectAction;

	private Action NoViewCollectAction;

	private Image bgImg_Top;

	private Image bgImg_Bot;

	private Action GamblingAction;

	private Image comsumeTypeIconImg;

	private Action GamblingStatusUpdateAction;

	private Image bottomLightImg;

	private bool isEverGambling;

	private Transform bubblePoint;

	private GameObject coinAnim;

	private GameObject healthAnim;

	private Transform gamblingCharacterPoint;

	private Tween resultImgTween;

	[Header("金币占卜界面参数")]
	public float CoinRate_Coin = 0.2f;

	public float HealthUpRate_Coin = 0.09f;

	public float NormalCardRate_Coin = 0.1f;

	public float EliteCardRate_Coin = 0.06f;

	public float ShopCardRate_Coin = 0.1f;

	public float NormalEquipRate_Coin = 0.07f;

	public float ElitEquipRate_Coin = 0.05f;

	public float ShopEquipRate_Coin = 0.05f;

	public float LegendEquipRate_Coin = 0.03f;

	public float NothingRate_Coin = 0.15f;

	public Sprite coin_HighlightSprite;

	public Sprite coin_DiableSprite;

	private static readonly string[] DayEnterBubbleKeys = new string[4] { "GAMBLINGENTERDAYKEY0", "GAMBLINGENTERDAYKEY1", "GAMBLINGENTERDAYKEY2", "GAMBLINGENTERDAYKEY3" };

	private static readonly string[] DayNothingBubbleKeys = new string[2] { "GAMBLINGNOTHINGDAYKEY0", "GAMBLINGNOTHINGDAYKEY1" };

	private GameEvent_14 _gameEvent14;

	[Header("卖血占卜界面参数")]
	public float CoinRate_Health = 0.2f;

	public float NormalCardRate_Health = 0.1f;

	public float EliteCardRate_Health = 0.06f;

	public float ShopCardRate_Health = 0.1f;

	public float NormalEquipRate_Health = 0.07f;

	public float ElitEquipRate_Health = 0.05f;

	public float ShopEquipRate_Health = 0.05f;

	public float LegendEquipRate_Health = 0.03f;

	public float NothingRate_Health = 0.15f;

	public Sprite health_HighlightSprite;

	public Sprite health_DisableSprite;

	private static readonly string[] NightEnterBubbleKeys = new string[5] { "GAMBLINGENTERNIGHTKEY0", "GAMBLINGENTERNIGHTKEY1", "GAMBLINGENTERNIGHTKEY2", "GAMBLINGENTERNIGHTKEY3", "GAMBLINGENTERNIGHTKEY4" };

	private static readonly string[] NightNothingBubbleKeys = new string[4] { "GAMBLINGNOTHINGNIGHTKEY0", "GAMBLINGNOTHINGNIGHTKEY1", "GAMBLINGNOTHINGNIGHTKEY2", "GAMBLINGNOTHINGNIGHTKEY3" };

	private GameEvent_15 _gameEvent15;

	public override string UIViewName => "GamblingUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ResetGemPosAndRotation();
		quitBtn.interactable = true;
		startGamblingBtn.interactable = true;
		resultImg.gameObject.SetActive(value: false);
		UpdatePlayerCoin();
		_usualHealthBarCtrl.LoadHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
		CollectAction = null;
		NoViewCollectAction = null;
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("赌博界面_卖命", isReplaceMainBgm: false);
		anim.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RoomUI.IsAnyBlockInteractiong = false;
		SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
		bottomLightImg.gameObject.SetActive(value: false);
		SingletonDontDestroy<UIManager>.Instance.HideView("BubbleTalkUI");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Coin Gambling UI ...");
	}

	public override void OnSpawnUI()
	{
		bgImg_Top = base.transform.Find("Bg").GetComponent<Image>();
		bgImg_Bot = base.transform.Find("Bg/Bg2").GetComponent<Image>();
		startGamblingBtn = base.transform.Find("Bg/StartGambling").GetComponent<Button>();
		startGamblingBtn.onClick.AddListener(OnClickStartGamling);
		quitBtn = base.transform.Find("Bg/QuitBtn").GetComponent<Button>();
		quitBtn.onClick.AddListener(EndGambling);
		gamblingBtnText = base.transform.Find("Bg/StartGambling/Text").GetComponent<Text>();
		playerCoinRemainText = base.transform.Find("Bg/CoinRemain").GetComponent<Text>();
		gemCentrePoint = base.transform.Find("Bg/GemRoot");
		int childCount = gemCentrePoint.childCount;
		gemImgArray = new Transform[childCount];
		gemsImg = new Image[childCount];
		for (int i = 0; i < childCount; i++)
		{
			gemImgArray[i] = gemCentrePoint.GetChild(i);
			gemsImg[i] = gemImgArray[i].GetComponent<Image>();
		}
		Transform transform = base.transform.Find("Bg/GemPoint");
		gemImgStartPointArray = new Transform[transform.childCount];
		for (int j = 0; j < gemImgStartPointArray.Length; j++)
		{
			gemImgStartPointArray[j] = transform.GetChild(j);
		}
		anim = GetComponent<UIAnim_Gambling>();
		resultImg = base.transform.Find("Bg/ResultImg").GetComponent<Image>();
		resultImg.GetComponent<Button>().onClick.AddListener(OnClickResultImg);
		_usualHealthBarCtrl = base.transform.Find("Bg/UsualHealthBar_Gambling").GetComponent<UsualHealthBarCtrl>();
		comsumeTypeIconImg = base.transform.Find("Bg/ComsumeTypeIcon").GetComponent<Image>();
		bottomLightImg = base.transform.Find("Bg/BottomLight").GetComponent<Image>();
		bubblePoint = base.transform.Find("Bg/BubblePoint");
		OnMouseEventCallback component = resultImg.GetComponent<OnMouseEventCallback>();
		component.EnterEventTrigger.Event.AddListener(OnMouseEnterResultImg);
		component.ExitEventTrigger.Event.AddListener(OnMouseExitResultImg);
		gamblingCharacterPoint = base.transform.Find("Bg/GamblingCharacterPoint");
		crystalPoint = base.transform.Find("Bg/CrystalPoint");
	}

	private void OnMouseEnterResultImg()
	{
		if (resultImgTween != null && resultImgTween.IsActive())
		{
			resultImgTween.Kill();
		}
		resultImgTween = resultImg.transform.DOScale(1.1f, 0.2f);
	}

	private void OnMouseExitResultImg()
	{
		if (resultImgTween != null && resultImgTween.IsActive())
		{
			resultImgTween.Kill();
		}
		resultImgTween = resultImg.transform.DOScale(1f, 0.2f);
	}

	private void OnClickStartGamling()
	{
		GamblingAction();
	}

	private IEnumerator GamblingAnim(Action gamblingAction)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("赌博水晶球");
		vfxBase.transform.position = gemCentrePoint.position;
		vfxBase.Play();
		int i = 0;
		while (i < gemImgArray.Length)
		{
			gemImgArray[i].transform.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InCubic);
			gemImgArray[i].transform.DOScale(0.6f, 0.3f);
			yield return new WaitForSeconds(0.15f);
			int num = i + 1;
			i = num;
		}
		gamblingAction();
	}

	private void ResetGemPosAndRotation()
	{
		for (int i = 0; i < gemImgArray.Length; i++)
		{
			gemImgArray[i].DOComplete();
			gemsImg[i].color = Color.clear;
			gemImgArray[i].localScale = Vector3.zero;
			gemsImg[i].DOColor(Color.white, 0.35f);
			gemImgArray[i].DOScale(1f, 0.35f);
			gemImgArray[i].rotation = Quaternion.identity;
			gemImgArray[i].position = gemImgStartPointArray[i].position;
		}
	}

	private void GamblingResultVfxMove(string VfxName, Action endAction)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(VfxName);
		vfxBase.Play();
		float x = Mathf.Clamp(gemCentrePoint.position.x, resultImg.transform.position.x, UnityEngine.Random.Range(0.4f, 0.6f));
		float y = Mathf.Clamp(gemCentrePoint.position.y, resultImg.transform.position.y, UnityEngine.Random.Range(0.5f, 0.7f));
		vfxBase.transform.TransformMoveByBezier(gemCentrePoint.position, new Vector3(x, y, 0f), resultImg.transform.position, 0.5f, delegate
		{
			quitBtn.interactable = true;
			ResetGemPosAndRotation();
			bottomLightImg.gameObject.SetActive(value: true);
			vfxBase.Recycle();
			endAction?.Invoke();
			GamblingStatusUpdateAction();
		});
	}

	private void GamblingResultVfxMove(string VfxName, Vector3 endPos, Action endAction)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx(VfxName);
		vfxBase.Play();
		float x = Mathf.Clamp(resultImg.transform.position.x, endPos.x, UnityEngine.Random.Range(0.4f, 0.6f));
		float y = Mathf.Clamp(resultImg.transform.position.y, endPos.y, UnityEngine.Random.Range(0.3f, 0.7f));
		vfxBase.transform.TransformMoveByBezier(resultImg.transform.position, new Vector3(x, y, 0f), endPos, 0.4f, delegate
		{
			bottomLightImg.gameObject.SetActive(value: false);
			vfxBase.Recycle();
			endAction?.Invoke();
		});
	}

	private void OnClickResultImg()
	{
		CollectAction?.Invoke();
		CollectAction = null;
		NoViewCollectAction = null;
	}

	private void GainCoinAnim(int amount)
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", playerCoinRemainText.transform.position, delegate
		{
			UpdatePlayerCoin();
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("+" + amount, Color.yellow, Color.black, playerCoinRemainText.transform, isSetParent: false, Vector3.zero);
		});
	}

	private void GainHealthAnim(int health)
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		GamblingResultVfxMove("effect_general_trail_shengming", _usualHealthBarCtrl.transform.position, delegate
		{
			UpdatePlayerHealth();
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("+" + health, Color.green, Color.black, _usualHealthBarCtrl.transform, isSetParent: false, Vector3.zero);
		});
	}

	private void GainCardAnim(string card)
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowCard(card);
	}

	private void GainCardAnim()
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", startGamblingBtn.transform.position, null);
	}

	private void GainEquipAnim(string equipCode)
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EventResultUI") as EventResultUI).ShowEquip(equipCode);
	}

	private void GainEquipAnim()
	{
		resultImg.gameObject.SetActive(value: false);
		bottomLightImg.gameObject.SetActive(value: false);
		GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", startGamblingBtn.transform.position, null);
	}

	private void ShowResultImg(Sprite sprite)
	{
		resultImg.sprite = sprite;
		resultImg.gameObject.SetActive(value: true);
		resultImg.transform.localScale = Vector3.zero;
		resultImg.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
	}

	private void UpdatePlayerCoin()
	{
		playerCoinRemainText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
	}

	private void UpdatePlayerHealth()
	{
		_usualHealthBarCtrl.UpdateHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
	}

	private void EndGambling()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("离开商店");
		if (isEverGambling)
		{
			GameSave.SaveGame();
		}
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		PlayExitSound();
	}

	public void ShowCoinGambling(GameEvent_14 gameEvent14)
	{
		isEverGambling = false;
		_gameEvent14 = gameEvent14;
		bgImg_Top.sprite = coinGamblingBgSprite_Top;
		bgImg_Bot.sprite = coinGamblingBgSprite_Bot;
		if (healthAnim != null)
		{
			healthAnim.gameObject.SetActive(value: false);
		}
		if (coinAnim != null)
		{
			coinAnim.gameObject.SetActive(value: true);
		}
		else
		{
			coinAnim = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("GamblingAnim_Coin", "Prefabs/Room", gamblingCharacterPoint);
			coinAnim.transform.localPosition = Vector3.zero;
		}
		gamblingBtnText.text = string.Format("{0}（-{1}G）", "startgambling".LocalizeText(), _gameEvent14.Coin);
		GamblingAction = OnClickStartGambling_Coin;
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入赌博界面");
		GamblingStatusUpdateAction = UpdateGamblingStatus_Coin;
		UpdateGamblingStatus_Coin();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowWitchBubble(DayEnterBubbleKeys[UnityEngine.Random.Range(0, DayEnterBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
		UnityEngine.Random.InitState(gameEvent14.RandomSeed);
	}

	private void UpdateCoinNeed()
	{
		_gameEvent14.Coin++;
		gamblingBtnText.text = string.Format("{0}（-{1}G）", "startgambling".LocalizeText(), _gameEvent14.Coin);
	}

	private void OnClickStartGambling_Coin()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= _gameEvent14.Coin)
		{
			NoViewCollectAction?.Invoke();
			NoViewCollectAction = null;
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(_gameEvent14.Coin);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("购买商品");
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-" + _gameEvent14.Coin, UIView.CoinColor, Color.black, playerCoinRemainText.transform, isSetParent: false, Vector3.zero);
			UpdatePlayerCoin();
			UpdateCoinNeed();
			startGamblingBtn.interactable = false;
			quitBtn.interactable = false;
			isEverGambling = true;
			StartCoroutine(GamblingAnim(Gambling_Coin));
		}
	}

	private void Gambling_Coin()
	{
		bool flag = true;
		while (true)
		{
			float value = UnityEngine.Random.value;
			float coinRate_Coin = CoinRate_Coin;
			if (value < coinRate_Coin)
			{
				GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", GainCoin);
				break;
			}
			if (value < (coinRate_Coin += HealthUpRate_Coin))
			{
				GamblingResultVfxMove("effect_general_trail_shengming", GainHealth);
				break;
			}
			if (value < (coinRate_Coin += NormalCardRate_Coin))
			{
				List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
				if (list.Count > 0)
				{
					string card3 = list[UnityEngine.Random.Range(0, list.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card3);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += EliteCardRate_Coin))
			{
				List<string> list2 = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(4);
				if (list2.Count > 0)
				{
					string card2 = list2[UnityEngine.Random.Range(0, list2.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card2);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += ShopCardRate_Coin))
			{
				List<string> list3 = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(2);
				if (list3.Count > 0)
				{
					string card = list3[UnityEngine.Random.Range(0, list3.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += NormalEquipRate_Coin))
			{
				List<string> list4 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
				if (list4.Count > 0)
				{
					string equip4 = list4[UnityEngine.Random.Range(0, list4.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip4);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += ElitEquipRate_Coin))
			{
				List<string> list5 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(8);
				if (list5.Count > 0)
				{
					string equip3 = list5[UnityEngine.Random.Range(0, list5.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip3);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += ShopEquipRate_Coin))
			{
				List<string> list6 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(4);
				if (list6.Count > 0)
				{
					string equip2 = list6[UnityEngine.Random.Range(0, list6.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip2);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Coin += LegendEquipRate_Coin))
			{
				List<string> list7 = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
				if (list7.Count > 0)
				{
					string equip = list7[UnityEngine.Random.Range(0, list7.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip);
					});
					break;
				}
				continue;
			}
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowWitchBubble(DayNothingBubbleKeys[UnityEngine.Random.Range(0, DayNothingBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("赌博界面_空手而归");
			UpdateGamblingStatus_Coin();
			quitBtn.interactable = true;
			ResetGemPosAndRotation();
			flag = false;
			break;
		}
		_gameEvent14.RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		UnityEngine.Random.InitState(_gameEvent14.RandomSeed);
		if (flag)
		{
			PlayGamblingSuccessSound();
		}
		else
		{
			PlayGamblingFailedSound();
		}
	}

	public void UpdateGamblingStatus_Coin()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= _gameEvent14.Coin)
		{
			comsumeTypeIconImg.sprite = coin_HighlightSprite;
			startGamblingBtn.interactable = true;
		}
		else
		{
			comsumeTypeIconImg.sprite = coin_DiableSprite;
			startGamblingBtn.interactable = false;
		}
	}

	public void ShowHealthGambling(GameEvent_15 event15)
	{
		_gameEvent15 = event15;
		isEverGambling = false;
		bgImg_Top.sprite = healthGamblingBgSprite_Top;
		bgImg_Bot.sprite = healthGamblingBgSprite_Bot;
		if (healthAnim != null)
		{
			healthAnim.gameObject.SetActive(value: true);
		}
		else
		{
			healthAnim = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("GamblingAnim_Health", "Prefabs/Room", gamblingCharacterPoint);
			healthAnim.transform.localPosition = Vector3.zero;
		}
		if (coinAnim != null)
		{
			coinAnim.gameObject.SetActive(value: false);
		}
		GamblingAction = StartGambling_Health;
		gamblingBtnText.text = "startgambling".LocalizeText() + "（-5HP）";
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入赌博界面");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("蝙蝠群飞远");
		GamblingStatusUpdateAction = UpdateGamblingStatus_Health;
		UpdateGamblingStatus_Health();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowWitchBubble(NightEnterBubbleKeys[UnityEngine.Random.Range(0, NightEnterBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
		UnityEngine.Random.InitState(_gameEvent15.RandomSeed);
	}

	private void StartGambling_Health()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5)
		{
			NoViewCollectAction?.Invoke();
			NoViewCollectAction = null;
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(5);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("心跳");
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-" + 5, Color.red, Color.black, _usualHealthBarCtrl.transform, isSetParent: false, Vector3.zero);
			UpdatePlayerHealth();
			startGamblingBtn.interactable = false;
			quitBtn.interactable = false;
			isEverGambling = true;
			StartCoroutine(GamblingAnim(Gambling_Health));
		}
	}

	private void Gambling_Health()
	{
		bool flag = true;
		while (true)
		{
			float value = UnityEngine.Random.value;
			float coinRate_Health = CoinRate_Health;
			if (value < coinRate_Health)
			{
				GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", GainCoin);
				break;
			}
			if (value < (coinRate_Health += NormalCardRate_Health))
			{
				List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
				if (list.Count > 0)
				{
					string card3 = list[UnityEngine.Random.Range(0, list.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card3);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += EliteCardRate_Health))
			{
				List<string> list2 = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(4);
				if (list2.Count > 0)
				{
					string card2 = list2[UnityEngine.Random.Range(0, list2.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card2);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += ShopCardRate_Health))
			{
				List<string> list3 = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(2);
				if (list3.Count > 0)
				{
					string card = list3[UnityEngine.Random.Range(0, list3.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainCard(card);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += NormalEquipRate_Health))
			{
				List<string> list4 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
				if (list4.Count > 0)
				{
					string equip4 = list4[UnityEngine.Random.Range(0, list4.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip4);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += ElitEquipRate_Health))
			{
				List<string> list5 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(8);
				if (list5.Count > 0)
				{
					string equip3 = list5[UnityEngine.Random.Range(0, list5.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip3);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += ShopEquipRate_Health))
			{
				List<string> list6 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(4);
				if (list6.Count > 0)
				{
					string equip2 = list6[UnityEngine.Random.Range(0, list6.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip2);
					});
					break;
				}
				continue;
			}
			if (value < (coinRate_Health += LegendEquipRate_Health))
			{
				List<string> list7 = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
				if (list7.Count > 0)
				{
					string equip = list7[UnityEngine.Random.Range(0, list7.Count)];
					GamblingResultVfxMove("effect_general_trail_jinengzhuangbei", delegate
					{
						GainEquip(equip);
					});
					break;
				}
				continue;
			}
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowShopBubble(NightNothingBubbleKeys[UnityEngine.Random.Range(0, NightNothingBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("赌博界面_空手而归");
			UpdateGamblingStatus_Health();
			quitBtn.interactable = true;
			ResetGemPosAndRotation();
			flag = false;
			break;
		}
		_gameEvent15.RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		UnityEngine.Random.InitState(_gameEvent15.RandomSeed);
		if (flag)
		{
			PlayGamblingSuccessSound();
		}
		else
		{
			PlayGamblingFailedSound();
		}
	}

	public void UpdateGamblingStatus_Health()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5)
		{
			comsumeTypeIconImg.sprite = health_HighlightSprite;
			startGamblingBtn.interactable = true;
		}
		else
		{
			comsumeTypeIconImg.sprite = health_DisableSprite;
			startGamblingBtn.interactable = false;
		}
	}

	private void PlayExitSound()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("VoiceAct/赌博 - 离场");
	}

	private void PlayGamblingFailedSound()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("VoiceAct/赌博 - 没赌到叹气");
	}

	private void PlayGamblingSuccessSound()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound(SoundStatic.GamblingSuccessSoundNames[UnityEngine.Random.Range(0, SoundStatic.GamblingSuccessSoundNames.Length)]);
	}

	private void GainCoin()
	{
		ShowResultImg(CoinGainResultSprite);
		int amount = UnityEngine.Random.Range(10, 21);
		Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(amount);
		NoViewCollectAction = (CollectAction = delegate
		{
			GainCoinAnim(amount);
		});
	}

	private void GainHealth()
	{
		ShowResultImg(HealthGainResultSprite);
		int health = UnityEngine.Random.Range(10, 20);
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(health);
		NoViewCollectAction = (CollectAction = delegate
		{
			GainHealthAnim(health);
		});
	}

	private void GainCard(string card)
	{
		ShowResultImg(CardGainResultSprite);
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(card, 1, isNew: true);
		CollectAction = delegate
		{
			GainCardAnim(card);
		};
		NoViewCollectAction = GainCardAnim;
	}

	private void GainEquip(string equipCode)
	{
		ShowResultImg(EquipGainResultSprite);
		Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(equipCode);
		CollectAction = delegate
		{
			GainEquipAnim(equipCode);
		};
		NoViewCollectAction = GainEquipAnim;
	}
}
