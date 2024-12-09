using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProphesyCardCtrl : MonoBehaviour
{
	private const string AssetPath = "Sprites/Prophesy";

	private static readonly Dictionary<PlayerOccupation, string> CardFront = new Dictionary<PlayerOccupation, string>
	{
		{
			PlayerOccupation.None,
			"预言牌正面_默认"
		},
		{
			PlayerOccupation.Knight,
			"预言牌正面_圣骑士"
		},
		{
			PlayerOccupation.Archer,
			"预言牌正面_弓箭手"
		}
	};

	private static readonly Dictionary<PlayerOccupation, string> CardBack = new Dictionary<PlayerOccupation, string>
	{
		{
			PlayerOccupation.None,
			"预言牌背面_默认"
		},
		{
			PlayerOccupation.Knight,
			"预言牌背面_圣骑士"
		},
		{
			PlayerOccupation.Archer,
			"预言牌背面_弓箭手"
		}
	};

	private Image cardImg;

	private Text nameText;

	private Image iconImg;

	private Text descriptionText;

	private Image backImg;

	private Image backIconImg;

	private bool isShowingHighlight;

	public CanvasGroup cardCanvasGroup;

	public string CurrentProphesyCode { get; private set; }

	private void Awake()
	{
		cardCanvasGroup = GetComponent<CanvasGroup>();
		cardImg = GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		descriptionText = base.transform.Find("Description").GetComponent<Text>();
		backImg = base.transform.Find("Back").GetComponent<Image>();
		backIconImg = base.transform.Find("Back/Icon").GetComponent<Image>();
	}

	public void CardBurn(Action act = null)
	{
		Text text = descriptionText;
		Image image = iconImg;
		bool flag2 = (nameText.enabled = false);
		bool flag4 = (image.enabled = flag2);
		text.enabled = flag4;
		cardImg.material.DOFloat(0f, "_ThreshHold", 1f).OnComplete(delegate
		{
			act?.Invoke();
		});
	}

	public void LoadProphesyCardData(ProphesyCardData prophesyCardData)
	{
		cardImg.material.SetFloat("_ThreshHold", 1f);
		Text text = descriptionText;
		Image image = iconImg;
		bool flag2 = (nameText.enabled = true);
		bool flag4 = (image.enabled = flag2);
		text.enabled = flag4;
		CurrentProphesyCode = prophesyCardData.CardCode;
		nameText.text = prophesyCardData.NameKey.LocalizeText();
		Sprite sprite3 = (backIconImg.sprite = (iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(prophesyCardData.IconName, "Sprites/Prophesy")));
		descriptionText.text = prophesyCardData.DescriptionKey.LocalizeText();
		cardImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(CardFront[prophesyCardData.PlayerOccupation], "Sprites/Prophesy");
		backImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(CardBack[prophesyCardData.PlayerOccupation], "Sprites/Prophesy");
		base.gameObject.SetActive(value: true);
	}

	public void ShowBack()
	{
		backImg.gameObject.SetActive(value: true);
	}

	public void HideBack()
	{
		backImg.gameObject.SetActive(value: false);
	}

	public void TurnToFront()
	{
		TurnAnim(HideBack);
	}

	public void TurnToBack()
	{
		TurnAnim(ShowBack);
	}

	public void FadeCard(float time)
	{
		cardCanvasGroup.DOFade(0f, time);
	}

	public void ShowCard()
	{
		cardCanvasGroup.alpha = 1f;
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		CurrentProphesyCode = string.Empty;
	}

	private void TurnAnim(Action middleAction)
	{
		base.transform.DOScaleX(0f, 0.2f).OnComplete(delegate
		{
			middleAction?.Invoke();
			base.transform.DOScaleX(1f, 0.2f);
		});
	}
}
