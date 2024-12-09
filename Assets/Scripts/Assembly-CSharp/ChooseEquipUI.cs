using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseEquipUI : UIView
{
	private int maxChooseLimit;

	private bool isMustEqualLimit;

	private Action<List<string>> callback;

	private List<string> equipChooesList = new List<string>();

	private List<ChooseEquipCtrl> equipCtrlsChooseList = new List<ChooseEquipCtrl>();

	private Text titleText;

	private string baseTitleStr;

	private CanvasGroup canvasGroup;

	private Queue<ChooseEquipCtrl> allEquipedSlotPools = new Queue<ChooseEquipCtrl>();

	private Dictionary<string, ChooseEquipCtrl> allShowingEquipedSlots = new Dictionary<string, ChooseEquipCtrl>();

	private RectTransform mainHandRect;

	private RectTransform mainHandRootRect;

	private Transform mainHandRoot;

	private Image mainhandEquipedIcon;

	private RectTransform supHandRect;

	private RectTransform supHandRootRect;

	private Transform supHandRoot;

	private Image suphandEquipedIcon;

	private RectTransform helmetRect;

	private RectTransform helmetRootRect;

	private Transform helmetRoot;

	private Image helmetEquipedIcon;

	private RectTransform breastplateRect;

	private RectTransform breastplateRootRect;

	private Transform breastplateRoot;

	private Image breastplateEquipedIcon;

	private RectTransform shoesRect;

	private RectTransform shoesRootRect;

	private Transform shoesRoot;

	private Image shoesEquipedIcon;

	private RectTransform gloveRect;

	private RectTransform gloveRootRect;

	private Transform gloveRoot;

	private Image gloveEquipedIcon;

	private RectTransform ornamentRect;

	private RectTransform ornamentRootRect;

	private Transform ornamentRoot;

	private Image ornamentEquipedIcon;

	public override string UIViewName => "ChooseEquipUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		canvasGroup.blocksRaycasts = true;
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllEquipedSlots();
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Choose Equip UI...");
	}

	public override void OnSpawnUI()
	{
		InitMainHand();
		InitSupHand();
		InitHelmet();
		InitBreastplate();
		InitShoes();
		InitGlove();
		InitOrnament();
		canvasGroup = GetComponent<CanvasGroup>();
		titleText = base.transform.Find("BgMask/Title").GetComponent<Text>();
		base.transform.Find("BgMask/ComfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickConfirm);
	}

	public void ShowAllEquipments(string title, int chooseLimit, bool isMustEqualLimit, Action<List<string>> callback)
	{
		equipChooesList.Clear();
		equipCtrlsChooseList.Clear();
		LoadEquipedMainHand();
		LoadEquipedSupHand();
		LoadEquipedBreastplate();
		LoadEquipedHelmet();
		LoadEquipedShoes();
		LoadEquipedGlove();
		LoadEquipedOrnament();
		baseTitleStr = title;
		maxChooseLimit = chooseLimit;
		this.isMustEqualLimit = isMustEqualLimit;
		this.callback = callback;
		UpdateTitle();
		PlayerInventory playerInventory = Singleton<GameManager>.Instance.Player.PlayerInventory;
		LoadMainHand(playerInventory.AllMainHands);
		LoadSupHand(playerInventory.AllSupHands);
		LoadHelmet(playerInventory.AllHelmets);
		LoadGlove(playerInventory.AllGloves);
		LoadShoes(playerInventory.AllShoes);
		LoadBreastplate(playerInventory.AllBreasplates);
		LoadOrnament(playerInventory.AllOrnaments);
		MainHandSizeFilter();
		SupHandSizeFilter();
		BreastplateSizeFilter();
		HelmetSizeFilter();
		ShoesSizeFilter();
		GloveSizeFilter();
		OrnamentSizeFilter();
	}

	private void UpdateTitle()
	{
		titleText.text = ((maxChooseLimit == -1) ? $"{baseTitleStr} ({equipCtrlsChooseList.Count})" : $"{baseTitleStr}（{equipChooesList.Count}/{maxChooseLimit}）");
	}

	private void OnClickConfirm()
	{
		if (!isMustEqualLimit || maxChooseLimit == equipChooesList.Count)
		{
			canvasGroup.blocksRaycasts = false;
			StartCoroutine(Confirm_IE());
		}
	}

	private IEnumerator Confirm_IE()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("事件_献祭技能装备");
		for (int i = 0; i < equipCtrlsChooseList.Count; i++)
		{
			equipCtrlsChooseList[i].BurnEquip();
		}
		yield return new WaitForSeconds(2f);
		callback?.Invoke(equipChooesList);
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public void OnChooseEquiped(ChooseEquipCtrl ctrl)
	{
		if (equipChooesList.Contains(ctrl.currentEquipCode))
		{
			ctrl.SetHighlightActive(isActive: false);
			equipChooesList.Remove(ctrl.currentEquipCode);
			equipCtrlsChooseList.Remove(ctrl);
			UpdateTitle();
			return;
		}
		if (maxChooseLimit > 0 && equipChooesList.Count == maxChooseLimit)
		{
			string key = equipChooesList[0];
			allShowingEquipedSlots[key].SetHighlightActive(isActive: false);
			equipChooesList.RemoveAt(0);
			equipCtrlsChooseList.RemoveAt(0);
		}
		ctrl.SetHighlightActive(isActive: true);
		equipChooesList.Add(ctrl.currentEquipCode);
		equipCtrlsChooseList.Add(ctrl);
		UpdateTitle();
	}

	private ChooseEquipCtrl GetEquipedSlot(Transform root)
	{
		if (allEquipedSlotPools.Count > 0)
		{
			ChooseEquipCtrl chooseEquipCtrl = allEquipedSlotPools.Dequeue();
			chooseEquipCtrl.transform.SetParent(root);
			chooseEquipCtrl.gameObject.SetActive(value: true);
			return chooseEquipCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EquipedChoosenSlot", "Prefabs", root).GetComponent<ChooseEquipCtrl>();
	}

	private void RecycleAllEquipedSlots()
	{
		if (allShowingEquipedSlots.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, ChooseEquipCtrl> allShowingEquipedSlot in allShowingEquipedSlots)
		{
			allShowingEquipedSlot.Value.gameObject.SetActive(value: false);
			allEquipedSlotPools.Enqueue(allShowingEquipedSlot.Value);
		}
		allShowingEquipedSlots.Clear();
	}

	private void InitMainHand()
	{
		mainHandRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedMainHand").GetComponent<RectTransform>();
		mainHandRoot = mainHandRect.Find("Root");
		mainHandRootRect = mainHandRoot.GetComponent<RectTransform>();
		mainhandEquipedIcon = mainHandRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedMainHand()
	{
		mainhandEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.MainHandWeapon.ImageName, "Sprites/Equipment");
	}

	private void LoadMainHand(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(mainHandRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void MainHandSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(mainHandRootRect);
		mainHandRect.sizeDelta = new Vector2(mainHandRect.sizeDelta.x, (mainHandRootRect.sizeDelta.y == 0f) ? mainHandRect.sizeDelta.y : mainHandRootRect.sizeDelta.y);
	}

	private void InitSupHand()
	{
		supHandRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedSupHand").GetComponent<RectTransform>();
		supHandRoot = supHandRect.Find("Root");
		supHandRootRect = supHandRoot.GetComponent<RectTransform>();
		suphandEquipedIcon = supHandRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedSupHand()
	{
		suphandEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.SupHandWeapon.ImageName, "Sprites/Equipment");
	}

	private void LoadSupHand(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(supHandRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void SupHandSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(supHandRootRect);
		supHandRect.sizeDelta = new Vector2(supHandRect.sizeDelta.x, (supHandRootRect.sizeDelta.y == 0f) ? supHandRect.sizeDelta.y : supHandRootRect.sizeDelta.y);
	}

	private void InitHelmet()
	{
		helmetRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedHelmet").GetComponent<RectTransform>();
		helmetRoot = helmetRect.Find("Root");
		helmetRootRect = helmetRoot.GetComponent<RectTransform>();
		helmetEquipedIcon = helmetRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedHelmet()
	{
		helmetEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.Helmet.ImageName, "Sprites/Equipment");
	}

	private void LoadHelmet(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(helmetRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void HelmetSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(helmetRootRect);
		helmetRect.sizeDelta = new Vector2(helmetRect.sizeDelta.x, (helmetRootRect.sizeDelta.y == 0f) ? helmetRect.sizeDelta.y : helmetRootRect.sizeDelta.y);
	}

	private void InitBreastplate()
	{
		breastplateRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedBreastplate").GetComponent<RectTransform>();
		breastplateRoot = breastplateRect.Find("Root");
		breastplateRootRect = breastplateRoot.GetComponent<RectTransform>();
		breastplateEquipedIcon = breastplateRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedBreastplate()
	{
		breastplateEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.Breastplate.ImageName, "Sprites/Equipment");
	}

	private void LoadBreastplate(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(breastplateRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void BreastplateSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(breastplateRootRect);
		breastplateRect.sizeDelta = new Vector2(breastplateRect.sizeDelta.x, (breastplateRootRect.sizeDelta.y == 0f) ? breastplateRect.sizeDelta.y : breastplateRootRect.sizeDelta.y);
	}

	private void InitShoes()
	{
		shoesRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedShoes").GetComponent<RectTransform>();
		shoesRoot = shoesRect.Find("Root");
		shoesRootRect = shoesRoot.GetComponent<RectTransform>();
		shoesEquipedIcon = shoesRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedShoes()
	{
		shoesEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.Shoes.ImageName, "Sprites/Equipment");
	}

	private void LoadShoes(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(shoesRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void ShoesSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(shoesRootRect);
		shoesRect.sizeDelta = new Vector2(shoesRect.sizeDelta.x, (shoesRootRect.sizeDelta.y == 0f) ? shoesRect.sizeDelta.y : shoesRootRect.sizeDelta.y);
	}

	private void InitGlove()
	{
		gloveRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedGlove").GetComponent<RectTransform>();
		gloveRoot = gloveRect.Find("Root");
		gloveRootRect = gloveRoot.GetComponent<RectTransform>();
		gloveEquipedIcon = gloveRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedGlove()
	{
		gloveEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.Glove.ImageName, "Sprites/Equipment");
	}

	private void LoadGlove(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(gloveRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void GloveSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(gloveRootRect);
		gloveRect.sizeDelta = new Vector2(gloveRect.sizeDelta.x, (gloveRootRect.sizeDelta.y == 0f) ? gloveRect.sizeDelta.y : gloveRootRect.sizeDelta.y);
	}

	private void InitOrnament()
	{
		ornamentRect = base.transform.Find("BgMask/Bg/Mask/Root/EquipedOrnament").GetComponent<RectTransform>();
		ornamentRoot = ornamentRect.Find("Root");
		ornamentRootRect = ornamentRoot.GetComponent<RectTransform>();
		ornamentEquipedIcon = ornamentRect.Find("Icon").GetComponent<Image>();
	}

	private void LoadEquipedOrnament()
	{
		ornamentEquipedIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(Singleton<GameManager>.Instance.Player.PlayerEquipment.Ornament.ImageName, "Sprites/Equipment");
	}

	private void LoadOrnament(List<string> codes)
	{
		for (int i = 0; i < codes.Count; i++)
		{
			ChooseEquipCtrl equipedSlot = GetEquipedSlot(ornamentRoot);
			equipedSlot.LoadEquip(this, codes[i]);
			allShowingEquipedSlots.Add(codes[i], equipedSlot);
		}
	}

	private void OrnamentSizeFilter()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(ornamentRootRect);
		ornamentRect.sizeDelta = new Vector2(ornamentRect.sizeDelta.x, (ornamentRootRect.sizeDelta.y == 0f) ? ornamentRect.sizeDelta.y : ornamentRootRect.sizeDelta.y);
	}
}
