using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCardUI : UIView
{
	private Transform cardRoot;

	private Action<List<string>> comfirmAction;

	private Action<List<UsualCard>> comfirmAction_UsualCard;

	private List<ChooseCardCtrl> chooseList = new List<ChooseCardCtrl>();

	private int maxChooseAmount;

	private bool isMustEqualLimit;

	private Text titleText;

	private string baseTitleStr;

	private Transform keyRoot;

	private RectTransform keyRootRect;

	private Queue<ChooseCardCtrl> allChooseCardPool = new Queue<ChooseCardCtrl>();

	private HashSet<ChooseCardCtrl> allShowingChooseCards = new HashSet<ChooseCardCtrl>();

	private Queue<KeyCtrl> allKeyPools = new Queue<KeyCtrl>();

	private List<KeyCtrl> allShowingKeys = new List<KeyCtrl>();

	public override string UIViewName => "ChooseCardUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllChooseCards();
		comfirmAction = null;
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Choose Card UI...");
	}

	public override void OnSpawnUI()
	{
		cardRoot = base.transform.Find("BgMask/Bg/Mask/Root");
		base.transform.Find("BgMask/ConfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickConfirmBtn);
		titleText = base.transform.Find("BgMask/Title").GetComponent<Text>();
		keyRoot = base.transform.Find("BgMask/KeyRoot");
		keyRootRect = keyRoot.GetComponent<RectTransform>();
	}

	private void OnClickConfirmBtn()
	{
		if (!isMustEqualLimit || chooseList.Count == maxChooseAmount)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < chooseList.Count; i++)
			{
				list.Add(chooseList[i].currentCardCode);
			}
			comfirmAction?.Invoke(list);
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}

	public void ShowChooseCard(string title, List<string> allCards, int chooseAmountLimit, bool isMustEqualLimit, Action<List<string>> comfirmAction)
	{
		chooseList.Clear();
		baseTitleStr = title;
		this.isMustEqualLimit = isMustEqualLimit;
		maxChooseAmount = chooseAmountLimit;
		this.comfirmAction = comfirmAction;
		UpdateTitle();
		for (int i = 0; i < allCards.Count; i++)
		{
			ChooseCardCtrl chooseCard = GetChooseCard();
			chooseCard.LoadCard(this, allCards[i]);
			allShowingChooseCards.Add(chooseCard);
		}
	}

	private void UpdateTitle()
	{
		titleText.text = $"{baseTitleStr}（{chooseList.Count}/{maxChooseAmount}）";
	}

	public void OnChooseCardWithAmount(ChooseCardCtrl card)
	{
		if (!chooseList.Contains(card))
		{
			if (chooseList.Count == maxChooseAmount)
			{
				chooseList[0].SetNormal();
				chooseList.RemoveAt(0);
			}
			chooseList.Add(card);
			card.SetHighlight();
			UpdateTitle();
		}
	}

	private ChooseCardCtrl GetChooseCard()
	{
		if (allChooseCardPool.Count > 0)
		{
			ChooseCardCtrl chooseCardCtrl = allChooseCardPool.Dequeue();
			chooseCardCtrl.gameObject.SetActive(value: true);
			return chooseCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("ChooseCardCtrl", "Prefabs", cardRoot).GetComponent<ChooseCardCtrl>();
	}

	private void RecycleAllChooseCards()
	{
		if (allShowingChooseCards.Count <= 0)
		{
			return;
		}
		foreach (ChooseCardCtrl allShowingChooseCard in allShowingChooseCards)
		{
			allShowingChooseCard.gameObject.SetActive(value: false);
			allChooseCardPool.Enqueue(allShowingChooseCard);
		}
		allShowingChooseCards.Clear();
	}

	public void ShowKeys(List<KeyValuePair> allKeys, Transform root)
	{
		keyRoot.position = root.position;
		if (allKeys != null && allKeys.Count > 0)
		{
			for (int i = 0; i < allKeys.Count; i++)
			{
				KeyCtrl keyCtrl = GetKeyCtrl(keyRoot);
				keyCtrl.LoadKey(allKeys[i].key.LocalizeText(), allKeys[i].value.LocalizeText());
				allShowingKeys.Add(keyCtrl);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(keyRootRect);
		}
	}

	private KeyCtrl GetKeyCtrl(Transform root)
	{
		if (allKeyPools.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyPools.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			keyCtrl.transform.SetParent(root);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", root).GetComponent<KeyCtrl>();
	}

	public void HideKeys()
	{
		if (allShowingKeys.Count > 0)
		{
			for (int i = 0; i < allShowingKeys.Count; i++)
			{
				allShowingKeys[i].gameObject.SetActive(value: false);
				allKeyPools.Enqueue(allShowingKeys[i]);
				allShowingKeys[i].transform.SetParent(base.transform);
			}
			allShowingKeys.Clear();
		}
	}
}
