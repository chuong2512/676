using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProphesyCardUI : UIView
{
	private static readonly string[] DefaultProphesy = new string[2] { "PC_5", "PC_6" };

	private Text spaceTimeAmountText;

	private Button refreshBtn;

	private Button confirmBtn;

	private ProphesyCardCtrl[] allProphesyCtrls;

	private const int RefreshNeedAmount = 10;

	private bool isCanRefresh;

	private int prophesyAmount;

	private int prophesyLevel;

	private float[] ThreeCardPointXArray;

	private float[] TwoCardPointXArray;

	private Transform cardStartPoint;

	private Transform cardEndPoint;

	private Transform spaceTimeShowPoint;

	public Color spaceTimeTextColor;

	private Action closeAction;

	private AudioController _controller;

	public Text title;

	public Text currentTitle;

	public Image spaceTimeImg;

	public Text spaceTimeAmount;

	public CanvasGroup refreshBtnCg;

	public CanvasGroup comfirmBtnTxt;

	public Text refreshBtnTxt;

	public Image refreshSpaceImg;

	public Text refreshCostTxt;

	public Image bgImg;

	private List<Tween> tweenList = new List<Tween>();

	public override string UIViewName => "ProphesyCardUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowProphesyUI((int)objs[0]);
		closeAction = (Action)objs[1];
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		spaceTimeAmountText = base.transform.Find("Root/Bg/Title/SpaceTime/Amount").GetComponent<Text>();
		refreshBtn = base.transform.Find("Root/Bg/RefreshBtn").GetComponent<Button>();
		refreshBtn.onClick.AddListener(OnClickRefreshBtn);
		confirmBtn = base.transform.Find("Root/Bg/ComfirmBtn").GetComponent<Button>();
		confirmBtn.onClick.AddListener(OnClickConfirmBtn);
		allProphesyCtrls = new ProphesyCardCtrl[3];
		ThreeCardPointXArray = new float[3];
		for (int i = 0; i < allProphesyCtrls.Length; i++)
		{
			allProphesyCtrls[i] = base.transform.Find("Root/Bg/Canvas/CardRoot").GetChild(i).GetComponent<ProphesyCardCtrl>();
			ThreeCardPointXArray[i] = allProphesyCtrls[i].transform.position.x;
		}
		TwoCardPointXArray = new float[2]
		{
			(ThreeCardPointXArray[0] + ThreeCardPointXArray[1]) / 2f,
			(ThreeCardPointXArray[1] + ThreeCardPointXArray[2]) / 2f
		};
		cardStartPoint = base.transform.Find("Root/Bg/Canvas/CardRoot/StartPoint");
		cardEndPoint = base.transform.Find("Root/Bg/Canvas/CardRoot/EndPoint");
		spaceTimeShowPoint = base.transform.Find("Root/Bg/Title/SpaceTime/ShowPoint");
	}

	public void ShowProphesyUI(int level)
	{
		PlaySound();
		isCanRefresh = level > 1 || (SingletonDontDestroy<Game>.Instance.isTest && SingletonDontDestroy<Game>.Instance.testProphesyCardCodes != null && SingletonDontDestroy<Game>.Instance.testProphesyCardCodes.Length != 0);
		prophesyAmount = ((level > 1) ? 3 : 2);
		prophesyLevel = level;
		refreshBtn.interactable = false;
		confirmBtn.interactable = false;
		spaceTimeAmountText.text = SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin.ToString();
		if (isCanRefresh)
		{
			SetThreeCardStartPoints();
			LoadProphesyCards(prophesyAmount, level);
		}
		else
		{
			SetTwoCardStartPoints();
			LoadDefaultProphesyCards();
		}
		StartAnim();
	}

	private void PlaySound()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("预言牌选择_开场声效");
		SingletonDontDestroy<AudioManager>.Instance.PlayerSound_Loop("预言牌选择_紫光背景声效");
		_controller = SingletonDontDestroy<AudioManager>.Instance.GetLoopSoundAudioController("预言牌选择_紫光背景声效");
	}

	private void FadeSound()
	{
		_controller.M_AudioSource.DOFade(0f, 2f).OnComplete(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.StopLoopSound("预言牌选择_紫光背景声效");
		});
	}

	private void SetThreeCardStartPoints()
	{
		for (int i = 0; i < allProphesyCtrls.Length; i++)
		{
			Vector3 position = new Vector3(ThreeCardPointXArray[i], cardStartPoint.position.y, allProphesyCtrls[i].transform.position.z);
			allProphesyCtrls[i].transform.position = position;
			allProphesyCtrls[i].ShowBack();
			allProphesyCtrls[i].ShowCard();
		}
	}

	private void SetTwoCardStartPoints()
	{
		for (int i = 0; i < 2; i++)
		{
			Vector3 position = new Vector3(TwoCardPointXArray[i], cardStartPoint.position.y, allProphesyCtrls[i].transform.position.z);
			allProphesyCtrls[i].transform.position = position;
			allProphesyCtrls[i].ShowBack();
			allProphesyCtrls[i].ShowCard();
		}
	}

	private void LoadProphesyCards(int amount, int level)
	{
		ProphesyCardData[] array = RandomGetProphesyCardDatas(amount, level);
		for (int i = 0; i < array.Length; i++)
		{
			allProphesyCtrls[i].LoadProphesyCardData(array[i]);
		}
		for (int j = array.Length; j < allProphesyCtrls.Length; j++)
		{
			allProphesyCtrls[j].Hide();
		}
	}

	private void LoadDefaultProphesyCards()
	{
		for (int i = 0; i < DefaultProphesy.Length; i++)
		{
			ProphesyCardData prophesyCardDataByCardData = DataManager.Instance.GetProphesyCardDataByCardData(DefaultProphesy[i]);
			allProphesyCtrls[i].LoadProphesyCardData(prophesyCardDataByCardData);
		}
		allProphesyCtrls[2].Hide();
	}

	private ProphesyCardData[] RandomGetProphesyCardDatas(int amount, int level)
	{
		if (SingletonDontDestroy<Game>.Instance.isTest && SingletonDontDestroy<Game>.Instance.testProphesyCardCodes != null && SingletonDontDestroy<Game>.Instance.testProphesyCardCodes.Length != 0)
		{
			prophesyAmount = SingletonDontDestroy<Game>.Instance.testProphesyCardCodes.Length;
			ProphesyCardData[] array = new ProphesyCardData[SingletonDontDestroy<Game>.Instance.testProphesyCardCodes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = DataManager.Instance.GetAllProphesyCardDatas()[SingletonDontDestroy<Game>.Instance.testProphesyCardCodes[i]];
			}
			return array;
		}
		ProphesyCardData[] array2 = new ProphesyCardData[amount];
		Dictionary<string, ProphesyCardData> allProphesyCardDatas = DataManager.Instance.GetAllProphesyCardDatas();
		List<string> list = new List<string>(allProphesyCardDatas.Count);
		List<string> list2 = new List<string>(allProphesyCardDatas.Count);
		foreach (KeyValuePair<string, ProphesyCardData> item in allProphesyCardDatas)
		{
			if (item.Value.PlayerOccupation == PlayerOccupation.None || item.Value.PlayerOccupation == SingletonDontDestroy<Game>.Instance.playerOccupation)
			{
				if (item.Value.Level <= 0)
				{
					list.Add(item.Key);
				}
				else if (item.Value.Level <= level)
				{
					list2.Add(item.Key);
				}
			}
		}
		string[] array3 = list.RandomFromList(amount - 1);
		for (int j = 0; j < array3.Length; j++)
		{
			array2[j] = allProphesyCardDatas[array3[j]];
		}
		array2[amount - 1] = allProphesyCardDatas[list2[UnityEngine.Random.Range(0, list2.Count)]];
		return array2;
	}

	private void OnClickRefreshBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("预言牌选择_置牌按钮");
		refreshBtn.interactable = false;
		confirmBtn.interactable = false;
		ComsumeSpaceTime();
		StartCoroutine(RefreshCards_IE());
	}

	private IEnumerator RefreshCards_IE()
	{
		for (int i = 0; i < prophesyAmount; i++)
		{
			ProphesyCardCtrl prophesyCardCtrl = allProphesyCtrls[i];
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_yuyanPaiShengxiao");
			vfxBase.Play();
			vfxBase.transform.position = prophesyCardCtrl.transform.position;
			prophesyCardCtrl.CardBurn();
		}
		yield return new WaitForSeconds(1.1f);
		LoadProphesyCards(prophesyAmount, prophesyLevel);
		if (prophesyAmount == 2)
		{
			SetTwoCardStartPoints();
		}
		else
		{
			SetThreeCardStartPoints();
		}
		StartCoroutine(ShowProphesyCardsAnim_IE());
	}

	private void ComsumeSpaceTime()
	{
		SingletonDontDestroy<Game>.Instance.CurrentUserData.ComsumeCoin(10, isAutoSave: true);
		ShowSpaceTimeComsumeAnim();
		SetRefreshBtnInteractive();
	}

	private void SetRefreshBtnInteractive()
	{
		refreshBtn.interactable = isCanRefresh && SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin >= 10;
		Color color3 = (refreshBtnTxt.color = (refreshCostTxt.color = (refreshBtn.interactable ? Color.white : Color.gray)));
	}

	private void ShowSpaceTimeComsumeAnim()
	{
		spaceTimeAmountText.text = SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin.ToString();
		Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-10", spaceTimeTextColor, Color.black, spaceTimeShowPoint, isSetParent: false, Vector3.zero);
	}

	private void OnClickConfirmBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("预言牌选择_确认按钮");
		FadeSound();
		List<string> list = new List<string>(prophesyAmount);
		for (int i = 0; i < prophesyAmount; i++)
		{
			list.Add(allProphesyCtrls[i].CurrentProphesyCode);
		}
		SingletonDontDestroy<Game>.Instance.ProphesyCardCodes = list;
		Singleton<GameManager>.Instance.Player.SetPlayerProphesyCards(list);
		StartCoroutine(ConfirmProphesyCard_IE(list));
	}

	private IEnumerator ConfirmProphesyCard_IE(List<string> allCodes)
	{
		CharacterInfoUI characterInfoUi = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		characterInfoUi.LoadPlayerProphesyInfo(allCodes, isShow: false);
		confirmBtn.interactable = false;
		refreshBtn.interactable = false;
		int i = 0;
		while (i < allCodes.Count)
		{
			ProphesyCardCtrl tmp = allProphesyCtrls[i];
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_yuyanPaiShengxiao");
			vfxBase.Play();
			vfxBase.transform.position = tmp.transform.position;
			int index = i;
			bool isFinal = i == allCodes.Count - 1;
			tmp.CardBurn(delegate
			{
				MoveSingleCard(tmp, characterInfoUi, index, isFinal ? closeAction : null);
			});
			yield return new WaitForSeconds(0.1f);
			int num = i + 1;
			i = num;
		}
		yield return new WaitForSeconds(1f);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("成长界面_解锁牌");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void MoveSingleCard(ProphesyCardCtrl ctrl, CharacterInfoUI characterInfoUi, int index, Action endAction)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_ProphesyCard");
		vfxBase.Play();
		Vector3 position = ctrl.transform.position;
		position.z = -5f;
		vfxBase.transform.position = position;
		Vector3 endPos = characterInfoUi.GetProphesyCardCtrlByIndex(index).position;
		endPos.z = -5f;
		float x = Mathf.Clamp(position.x, endPos.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = (position.y + endPos.y) / 2f + UnityEngine.Random.Range(4f, 8f);
		vfxBase.transform.TransformMoveByBezier(position, new Vector3(x, y, -5f), endPos, 0.7f, delegate
		{
			Singleton<GameManager>.Instance.Player.ActivePlayerProphesy(ctrl.CurrentProphesyCode);
			characterInfoUi.ActiveProphesyCardCtrlByIndex(index);
			vfxBase.Recycle();
			VfxBase vfxBase2 = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_prophesyCard_hit");
			vfxBase2.transform.position = endPos;
			vfxBase2.Play();
			endAction?.Invoke();
		});
	}

	public void StartAnim()
	{
		ResetAnim();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private void ResetAnim()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		title.WithCol(0f);
		currentTitle.WithCol(0f);
		spaceTimeImg.WithCol(0f);
		spaceTimeAmount.WithCol(0f);
		refreshBtnCg.alpha = 0f;
		comfirmBtnTxt.alpha = 0f;
		refreshSpaceImg.WithCol(0f);
		refreshCostTxt.WithCol(0f);
		bgImg.material.SetFloat("_ColPower", 0f);
	}

	private IEnumerator StartAnimCo()
	{
		bgImg.material.DOFloat(1.6f, "_ColPower", 1f).SetEase(Ease.InQuint);
		SingletonDontDestroy<CameraController>.Instance.ActivatePostEffect(PostEffectType.RadialBlur, new RadialBlueEffectArgs(0.02f, 0.6f, 3, Vector3.left * 0.5f, 0.001f, 0.35f, 0f));
		yield return new WaitForSeconds(1f);
		tweenList.Add(title.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.2f);
		tweenList.Add(spaceTimeAmount.DOFade(1f, 0.5f));
		tweenList.Add(currentTitle.DOFade(1f, 0.5f));
		tweenList.Add(spaceTimeImg.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.2f);
		StartCoroutine(ShowProphesyCardsAnim_IE());
		yield return new WaitForSeconds(1f);
		tweenList.Add(refreshBtnCg.DOFade(1f, 0.5f));
		tweenList.Add(refreshSpaceImg.DOFade(1f, 0.5f));
		tweenList.Add(refreshCostTxt.DOFade(1f, 0.5f));
		tweenList.Add(comfirmBtnTxt.DOFade(1f, 0.5f));
	}

	private IEnumerator ShowProphesyCardsAnim_IE()
	{
		int i = 0;
		while (i < prophesyAmount)
		{
			int index = i;
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("预言牌选择_落牌");
			allProphesyCtrls[index].transform.DOMoveY(cardEndPoint.position.y, 0.6f).OnComplete(delegate
			{
				allProphesyCtrls[index].TurnToFront();
			}).SetEase(Ease.OutBack);
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		yield return new WaitForSeconds(0.5f);
		confirmBtn.interactable = true;
		SetRefreshBtnInteractive();
	}
}
