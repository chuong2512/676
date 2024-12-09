using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDetailHoverHint : BaseHoverHint
{
	private Dictionary<BuffRoundType, Action<int>> TypeHandlers;

	public Sprite roundSprite;

	public Sprite layerSprite;

	private Text buffNameText;

	private Transform roundHintTrans;

	private Image roundIconImg;

	private Text roundText;

	private Text descriptionText;

	protected override void OnAwake()
	{
		buffNameText = base.transform.Find("BuffName").GetComponent<Text>();
		roundHintTrans = base.transform.Find("RoundHint");
		roundIconImg = roundHintTrans.Find("Icon").GetComponent<Image>();
		roundText = roundHintTrans.Find("Round").GetComponent<Text>();
		descriptionText = base.transform.Find("Description").GetComponent<Text>();
		TypeHandlers = new Dictionary<BuffRoundType, Action<int>>(3)
		{
			{
				BuffRoundType.None,
				HandlerNoneType
			},
			{
				BuffRoundType.Round,
				HandleRoundType
			},
			{
				BuffRoundType.Layer,
				HandleLayerType
			}
		};
	}

	public void LoadBuffDetail(string buffName, string buffDescription, BuffRoundType roundType, int hintAmount)
	{
		buffNameText.text = buffName;
		descriptionText.text = buffDescription;
		TypeHandlers[roundType](hintAmount);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}

	private void HandleRoundType(int hintAmount)
	{
		roundHintTrans.gameObject.SetActive(value: true);
		roundIconImg.sprite = roundSprite;
		roundText.text = hintAmount + "Round".LocalizeText();
	}

	private void HandleLayerType(int hintAmount)
	{
		roundHintTrans.gameObject.SetActive(value: true);
		roundIconImg.sprite = layerSprite;
		roundText.text = hintAmount + "Layer".LocalizeText();
	}

	private void HandlerNoneType(int hintAmount)
	{
		roundHintTrans.gameObject.SetActive(value: false);
	}
}
