using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinglePresuppositionCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private static readonly Color ChosenNameColor = "FFE86AFF".HexColorToColor();

	private static readonly Color NormalNameColor = Color.white;

	public Sprite NormalAddSprite;

	public Sprite HighlightAddSprite;

	private Text nameText;

	private bool isChosen;

	private Image bgImg;

	private Image addnewImg;

	private CardPresuppositionUI _cardPresuppositionUi;

	private CardPresuppositionStruct currentCardPresuppositionStruct;

	private List<Tween> tweenList = new List<Tween>();

	private int currentShowingPresuppositionIndex
	{
		get
		{
			if (currentCardPresuppositionStruct != null)
			{
				return currentCardPresuppositionStruct.index;
			}
			return -1;
		}
	}

	private void Awake()
	{
		nameText = base.transform.Find("Name").GetComponent<Text>();
		bgImg = GetComponent<Image>();
		addnewImg = base.transform.Find("AddNew").GetComponent<Image>();
	}

	private IEnumerator StartAnimCo()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		bgImg.fillAmount = 0.15f;
		addnewImg.WithCol(0f);
		nameText.WithCol(0f);
		tweenList.Add(bgImg.DOFillAmount(1f, 0.3f));
		yield return new WaitForSeconds(0.3f);
		tweenList.Add(addnewImg.DOFade(1f, 0.4f));
		tweenList.Add(nameText.DOFade(1f, 0.4f));
	}

	public void EditPresuppositionName()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CreateNewPresuppositionUI", nameText.text) as CreateNewPresuppositionUI).ShowCreateNewPresuppositionPanel(_cardPresuppositionUi.CheckPresuppositionLeague, OnEndEditNewName);
	}

	private void OnEndEditNewName(string newName)
	{
		nameText.text = newName;
		_cardPresuppositionUi.ConfirmVarifyCurrentShowingPresuppositionName(newName);
	}

	public void LoadPresupposition(CardPresuppositionUI presuppositionUi, CardPresuppositionStruct cardPresuppositionStruct)
	{
		currentCardPresuppositionStruct = cardPresuppositionStruct;
		_cardPresuppositionUi = presuppositionUi;
		if (cardPresuppositionStruct == null)
		{
			nameText.gameObject.SetActive(value: false);
			addnewImg.gameObject.SetActive(value: true);
		}
		else
		{
			nameText.gameObject.SetActive(value: true);
			addnewImg.gameObject.SetActive(value: false);
			nameText.text = (cardPresuppositionStruct.isDefault ? cardPresuppositionStruct.Name.LocalizeText() : cardPresuppositionStruct.Name);
		}
		isChosen = false;
		SetNormal();
		StartCoroutine(StartAnimCo());
	}

	public void SetNormal()
	{
		isChosen = false;
		nameText.color = NormalNameColor;
		bgImg.sprite = _cardPresuppositionUi.presuppositionNormal;
	}

	public void SetChosen()
	{
		nameText.color = ChosenNameColor;
		bgImg.sprite = _cardPresuppositionUi.presuppositionChosen;
		isChosen = true;
	}

	public void SetHighlight()
	{
		bgImg.sprite = _cardPresuppositionUi.presuppositionHighlight;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!isChosen)
		{
			SetHighlight();
		}
		if (currentShowingPresuppositionIndex < 0)
		{
			addnewImg.sprite = HighlightAddSprite;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!isChosen)
		{
			SetNormal();
		}
		if (currentShowingPresuppositionIndex < 0)
		{
			addnewImg.sprite = NormalAddSprite;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (currentShowingPresuppositionIndex < 0)
		{
			_cardPresuppositionUi.TryAddNewPresupposition();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("新建卡组");
		}
		else if (!isChosen)
		{
			_cardPresuppositionUi.TryShowChosenPresupposition(currentShowingPresuppositionIndex);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("切换卡组");
		}
	}
}
