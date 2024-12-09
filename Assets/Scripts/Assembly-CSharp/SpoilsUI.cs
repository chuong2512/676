using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SpoilsUI : UIView, ILocalization
{
	public Sprite[] CNSprites;

	public Sprite[] ENSprites;

	private Transform spoilsRoot;

	private Image nothingImg;

	private UIAnimBase anim;

	public GameObject levelUpShining;

	private Action comfirmAction;

	private Text moneyAmountText;

	private const float FullLevelImgMoveTime = 1.2f;

	private Text expAmountText;

	private Image expImg;

	private Image levelUpImg;

	private SpoilCardItemCtrl spoilCardItem;

	private SpoilEquipItemCtrl spoilEquipItem;

	private bool isGetCard;

	private bool isGetEquip;

	public override string UIViewName => "SpoilsUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌_战利品界面");
		anim.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		comfirmAction = null;
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Spoils UI ...");
	}

	public override void OnSpawnUI()
	{
		spoilsRoot = base.transform.Find("Mask/Bg/ItemRoot/Mask/Content");
		base.transform.Find("Mask/Bg/ComfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickComfirmBtn);
		nothingImg = base.transform.Find("Mask/Bg/NothingImg").GetComponent<Image>();
		anim = GetComponent<UIAnimBase>();
		InitMoneyPanel();
		InitExpPanel();
		InitItemGetPanel();
		Localization();
	}

	public void Localization()
	{
		base.transform.Find("Mask/Bg/DecorationImg").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNSprites[0] : ENSprites[0]);
		base.transform.Find("Mask/Bg/ItemGetTitle/Image").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNSprites[1] : ENSprites[1]);
		base.transform.Find("Mask/Bg/NothingImg").GetComponent<Image>().sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? CNSprites[2] : ENSprites[2]);
	}

	private void OnClickComfirmBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("获得金钱");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("点击_通用");
		comfirmAction?.Invoke();
		SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
		if (isGetCard || isGetEquip)
		{
			MoveParticalToBagIcon();
		}
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
        AdsManager.Instance?.ShowInterAds();
	}

	private void MoveParticalToBagIcon()
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_jinengzhuangbei");
		vfxBase.Play();
		Vector3 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.ViewportToWorldPoint(Vector2.one * 0.5f);
		vector.z = -5f;
		vfxBase.transform.position = vector;
		RoomUI roomUi = SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI;
		Vector3 position = roomUi.bagBtn.transform.position;
		position.z = -5f;
		float x = Mathf.Clamp(vector.x, position.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = (vector.y + position.y) / 2f + UnityEngine.Random.Range(4f, 8f);
		vfxBase.transform.TransformMoveByBezier(vector, new Vector3(x, y, -5f), position, 0.7f, delegate
		{
			roomUi.BagBtnHint();
			vfxBase.Recycle();
		});
	}

	public void SetComfirmAction(Action comfirmAction)
	{
		this.comfirmAction = comfirmAction;
	}

	private void InitMoneyPanel()
	{
		moneyAmountText = base.transform.Find("Mask/Bg/MoneyEarn/Description").GetComponent<Text>();
	}

	public void SetMoneyText(int baseAmount, int extraAmount)
	{
		moneyAmountText.text = ((extraAmount > 0) ? $"+{baseAmount + extraAmount}({baseAmount}+{extraAmount})" : ("+" + baseAmount));
	}

	private void InitExpPanel()
	{
		expAmountText = base.transform.Find("Mask/Bg/ExpEarn/Description").GetComponent<Text>();
		expImg = base.transform.Find("Mask/Bg/ExpEarn/ExpBg/ExpImg").GetComponent<Image>();
		levelUpImg = base.transform.Find("Mask/Bg/ExpEarn/LevelUpImg").GetComponent<Image>();
	}

	public void SetExpGain(float preRate, float currentRate, int levelUpAmount, int baseExp, int extraExp = 0)
	{
		levelUpShining.SetActive(value: false);
		expImg.DOKill();
		levelUpImg.gameObject.SetActive(value: false);
		expAmountText.text = ((extraExp > 0) ? $"+{baseExp + extraExp}({baseExp}+{extraExp})" : ("+" + baseExp));
		if (preRate == currentRate && levelUpAmount == 0)
		{
			expImg.fillAmount = preRate;
		}
		else
		{
			StartCoroutine(ExpGain_IE(preRate, currentRate, levelUpAmount));
		}
	}

	private IEnumerator ExpGain_IE(float preRate, float currentRate, int levelAmount)
	{
		yield return new WaitForSeconds(0.5f);
		if (levelAmount > 0)
		{
			bool isMoving = false;
			while (levelAmount > 0)
			{
				isMoving = true;
				expImg.fillAmount = preRate;
				expImg.DOFillAmount(1f, (1f - preRate) * 1.2f).OnComplete(delegate
				{
					levelUpImg.gameObject.SetActive(value: true);
					levelUpShining.SetActive(value: true);
					levelUpImg.transform.localScale = Vector3.zero;
					levelUpImg.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).OnComplete(delegate
					{
						isMoving = false;
					});
					levelUpImg.transform.DOPunchPosition(Vector3.one * 20f, 0.5f);
					int num = levelAmount;
					levelAmount = num - 1;
					preRate = 0f;
				});
				while (isMoving)
				{
					yield return null;
				}
			}
			expImg.fillAmount = preRate;
			expImg.DOFillAmount(currentRate, currentRate * 1.2f).SetEase(Ease.OutQuint);
		}
		else
		{
			expImg.fillAmount = preRate;
			expImg.DOFillAmount(currentRate, (currentRate - preRate) * 1.2f).SetEase(Ease.OutQuint);
		}
	}

	private void InitItemGetPanel()
	{
		spoilCardItem = base.transform.Find("Mask/Bg/ItemRoot/SpoilCardItem").GetComponent<SpoilCardItemCtrl>();
		spoilEquipItem = base.transform.Find("Mask/Bg/ItemRoot/SpoilEquipItem").GetComponent<SpoilEquipItemCtrl>();
	}

	public void SetSpoilEquipItem(string equipCode)
	{
		if (equipCode.IsNullOrEmpty())
		{
			isGetEquip = false;
			spoilEquipItem.gameObject.SetActive(value: false);
		}
		else
		{
			isGetEquip = true;
			spoilEquipItem.gameObject.SetActive(value: true);
			spoilEquipItem.LoadEquip(equipCode);
		}
	}

	public void SetSpoilCardItem(string cardCode, int amount)
	{
		if (cardCode.IsNullOrEmpty())
		{
			spoilCardItem.gameObject.SetActive(value: false);
			isGetCard = false;
		}
		else
		{
			isGetCard = true;
			spoilCardItem.gameObject.SetActive(value: true);
			spoilCardItem.LoadCard(cardCode, amount);
		}
	}

	public void SetNothingActive(bool isActive)
	{
		nothingImg.gameObject.SetActive(isActive);
	}
}
