using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI_EquipPanel : MonoBehaviour
{
	private Transform equipPanel;

	private Action equipPanelHideAction;

	private int currentEquipPanelShowIndex;

	private Transform equipSlotRoot;

	private Queue<EquipSlotCtrl> allEquipSlotPool = new Queue<EquipSlotCtrl>();

	private Dictionary<string, EquipSlotCtrl> allShowingEquipSlots = new Dictionary<string, EquipSlotCtrl>();

	private string currentShowingEquipCode;

	private Action<string> ShowingEquipmentInfoAction;

	private BagUI parentUI;

	private Scrollbar _scrollbar;

	private Button helmetBtn;

	private Image newHelmetIconImg;

	private int newHelmetAmount;

	private Button breastplateBtn;

	private Image newBreastplateIconImg;

	private int newBreastplateAmount;

	private Button gloveBtn;

	private Image newGloveIconImg;

	private int newGloveAmount;

	private Button shoesBtn;

	private Image newShoesIconImg;

	private int newShoesAmount;

	private Button ornamentBtn;

	private Image newOrnamentIconImg;

	private int newOrnamentAmount;

	private Button mainhandBtn;

	private Image newMainHandIconImg;

	private int newMainHandAmount;

	private Button suphandBtn;

	private Image newSupHandIconImg;

	private int newSupHandAmount;

	public void InitEquipPanel(BagUI parentUI)
	{
		this.parentUI = parentUI;
		equipPanel = base.transform;
		equipPanel.Find("ButtonList/Helmet").GetComponent<Button>().onClick.AddListener(OnClickShowHelmetEquip);
		equipPanel.Find("ButtonList/Breastplate").GetComponent<Button>().onClick.AddListener(OnClickShowBreastplateEquip);
		equipPanel.Find("ButtonList/Glove").GetComponent<Button>().onClick.AddListener(OnClickShowGloveEquip);
		equipPanel.Find("ButtonList/Shoes").GetComponent<Button>().onClick.AddListener(OnClickShowShoesEquip);
		equipPanel.Find("ButtonList/Ornament").GetComponent<Button>().onClick.AddListener(OnClickShowOrnamentEquip);
		equipPanel.Find("ButtonList/MainHand").GetComponent<Button>().onClick.AddListener(OnClickShowMainHandEquip);
		equipPanel.Find("ButtonList/SupHand").GetComponent<Button>().onClick.AddListener(OnClickShowSupHandEquip);
		equipSlotRoot = equipPanel.Find("EquipList/Mask/Content");
		_scrollbar = base.transform.Find("EquipList/Scrollbar").GetComponent<Scrollbar>();
		InitEquipHelmetPanel();
		InitEquipBreasplatePanel();
		InitEquipGlovePanel();
		InitEquipShoesPanel();
		InitEquipOrnamentPanel();
		InitEquipMainHandPanel();
		InitEquipSupHandPanel();
	}

	public void Show()
	{
		equipPanel.gameObject.SetActive(value: true);
		parentUI.AddGuideTips(new List<string>(1) { "Code_9_1" });
		SingletonDontDestroy<UIManager>.Instance.ShowView("CharacterInfoUI");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("装备界面");
		SetEquipNewIcon();
		ShowEquip_Helmet();
		EventManager.BroadcastEvent(EventEnum.E_OnOpenBagEquipPanel, null);
		parentUI.uiAnim.StartEquipPanel(allShowingEquipSlots);
	}

	public void Hide()
	{
		equipPanel.gameObject.SetActive(value: false);
		equipPanelHideAction?.Invoke();
		equipPanelHideAction = null;
		SingletonDontDestroy<UIManager>.Instance.HideView("CharacterInfoUI");
	}

	private void SetScrollbar()
	{
		StartCoroutine(SetScrollbar_IE());
	}

	private IEnumerator SetScrollbar_IE()
	{
		yield return null;
		_scrollbar.value = 1f;
	}

	private void SetEquipNewIcon()
	{
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		Dictionary<EquipmentType, bool> dictionary = new Dictionary<EquipmentType, bool>
		{
			{
				EquipmentType.Helmet,
				false
			},
			{
				EquipmentType.Breastplate,
				false
			},
			{
				EquipmentType.Ornament,
				false
			},
			{
				EquipmentType.MainHandWeapon,
				false
			},
			{
				EquipmentType.SupHandWeapon,
				false
			},
			{
				EquipmentType.Shoes,
				false
			},
			{
				EquipmentType.Glove,
				false
			}
		};
		foreach (string item in allNewEquipments)
		{
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item);
			dictionary[equipmentCardAttr.EquipmentType] = true;
		}
		newHelmetIconImg.gameObject.SetActive(dictionary[EquipmentType.Helmet]);
		newBreastplateIconImg.gameObject.SetActive(dictionary[EquipmentType.Breastplate]);
		newOrnamentIconImg.gameObject.SetActive(dictionary[EquipmentType.Ornament]);
		newGloveIconImg.gameObject.SetActive(dictionary[EquipmentType.Glove]);
		newShoesIconImg.gameObject.SetActive(dictionary[EquipmentType.Shoes]);
		newMainHandIconImg.gameObject.SetActive(dictionary[EquipmentType.MainHandWeapon]);
		newSupHandIconImg.gameObject.SetActive(dictionary[EquipmentType.SupHandWeapon]);
	}

	private void OnClickShowHelmetEquip()
	{
		if (currentEquipPanelShowIndex != 0)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_Helmet();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowBreastplateEquip()
	{
		if (currentEquipPanelShowIndex != 1)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_Breastplate();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowGloveEquip()
	{
		if (currentEquipPanelShowIndex != 2)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_Glove();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowShoesEquip()
	{
		if (currentEquipPanelShowIndex != 3)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_Shoes();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowOrnamentEquip()
	{
		if (currentEquipPanelShowIndex != 4)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_Ornament();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowMainHandEquip()
	{
		if (currentEquipPanelShowIndex != 5)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_MainHand();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void OnClickShowSupHandEquip()
	{
		if (currentEquipPanelShowIndex != 6)
		{
			equipPanelHideAction?.Invoke();
			equipPanelHideAction = null;
			ShowEquip_SupHand();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			parentUI.uiAnim.ResetEquipSlotAnim(allShowingEquipSlots);
		}
	}

	private void PlayEquipSound(string soundName)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound(soundName);
	}

	private EquipSlotCtrl GetEquipSlot()
	{
		if (allEquipSlotPool.Count > 0)
		{
			EquipSlotCtrl equipSlotCtrl = allEquipSlotPool.Dequeue();
			equipSlotCtrl.gameObject.SetActive(value: true);
			return equipSlotCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EquipSlot", "Prefabs", equipSlotRoot).GetComponent<EquipSlotCtrl>();
	}

	private void RecycleAllEquipSlot()
	{
		if (allShowingEquipSlots.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, EquipSlotCtrl> allShowingEquipSlot in allShowingEquipSlots)
		{
			allShowingEquipSlot.Value.gameObject.SetActive(value: false);
			allEquipSlotPool.Enqueue(allShowingEquipSlot.Value);
		}
		allShowingEquipSlots.Clear();
	}

	public void ShowEquipInfo(string equipCode)
	{
		ShowingEquipmentInfoAction(equipCode);
	}

	private void InitEquipHelmetPanel()
	{
		helmetBtn = equipPanel.Find("ButtonList/Helmet").GetComponent<Button>();
		newHelmetIconImg = equipPanel.Find("ButtonList/Helmet/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_Helmet()
	{
		currentEquipPanelShowIndex = 0;
		equipPanelHideAction = HideEquip_Helmet;
		helmetBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowHelmetInfo;
		LoadEquip_Helmet();
	}

	private void HideEquip_Helmet()
	{
		RecycleAllEquipSlot();
		helmetBtn.interactable = true;
	}

	private void LoadEquip_Helmet()
	{
		List<string> allHelmets = Singleton<GameManager>.Instance.Player.PlayerInventory.AllHelmets;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newHelmetAmount = 0;
		for (int i = 0; i < allHelmets.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allHelmets[i]);
			equipSlot.LoadEquip(this, RemoveNewHelmet, allHelmets[i], flag);
			if (flag)
			{
				newHelmetAmount++;
			}
			allShowingEquipSlots.Add(allHelmets[i], equipSlot);
		}
		SetScrollbar();
	}

	private void ShowHelmetInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingHelmet, isInteractive: true);
	}

	private void RemoveNewHelmet(string equipCode)
	{
		if (newHelmetAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newHelmetAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newHelmetAmount == 0)
		{
			newHelmetIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingHelmet()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.Helmet.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipHelmet(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.Helmet, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		equipSlotCtrl.LoadEquip(this, RemoveNewHelmet, cardCode, isNew: false);
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipBreasplatePanel()
	{
		breastplateBtn = equipPanel.Find("ButtonList/Breastplate").GetComponent<Button>();
		newBreastplateIconImg = equipPanel.Find("ButtonList/Breastplate/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_Breastplate()
	{
		currentEquipPanelShowIndex = 1;
		equipPanelHideAction = HideEquip_Breastplate;
		breastplateBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowingBreastplateInfo;
		LoadEquip_Breastplate();
	}

	private void HideEquip_Breastplate()
	{
		RecycleAllEquipSlot();
		breastplateBtn.interactable = true;
	}

	private void LoadEquip_Breastplate()
	{
		List<string> allBreasplates = Singleton<GameManager>.Instance.Player.PlayerInventory.AllBreasplates;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newBreastplateAmount = 0;
		for (int i = 0; i < allBreasplates.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allBreasplates[i]);
			equipSlot.LoadEquip(this, RemoveNewBreastplate, allBreasplates[i], flag);
			if (flag)
			{
				newBreastplateAmount++;
			}
			allShowingEquipSlots.Add(allBreasplates[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowingBreastplateInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingBreastplate, isInteractive: true);
	}

	private void RemoveNewBreastplate(string equipCode)
	{
		if (newBreastplateAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newBreastplateAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newBreastplateAmount == 0)
		{
			newBreastplateIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingBreastplate()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.Breastplate.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipBreastplate(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.Breastplate, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewBreastplate, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipGlovePanel()
	{
		gloveBtn = equipPanel.Find("ButtonList/Glove").GetComponent<Button>();
		newGloveIconImg = equipPanel.Find("ButtonList/Glove/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_Glove()
	{
		currentEquipPanelShowIndex = 2;
		equipPanelHideAction = HideEquip_Glove;
		gloveBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowGloveInfo;
		LoadEquip_Glove();
	}

	private void HideEquip_Glove()
	{
		RecycleAllEquipSlot();
		gloveBtn.interactable = true;
	}

	private void LoadEquip_Glove()
	{
		List<string> allGloves = Singleton<GameManager>.Instance.Player.PlayerInventory.AllGloves;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newGloveAmount = 0;
		for (int i = 0; i < allGloves.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allGloves[i]);
			equipSlot.LoadEquip(this, RemoveNewGlove, allGloves[i], flag);
			if (flag)
			{
				newGloveAmount++;
			}
			allShowingEquipSlots.Add(allGloves[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowGloveInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingGlove, isInteractive: true);
	}

	private void RemoveNewGlove(string equipCode)
	{
		if (newGloveAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newGloveAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newGloveAmount == 0)
		{
			newGloveIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingGlove()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.Glove.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipGlove(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.Glove, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewGlove, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipShoesPanel()
	{
		shoesBtn = equipPanel.Find("ButtonList/Shoes").GetComponent<Button>();
		newShoesIconImg = equipPanel.Find("ButtonList/Shoes/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_Shoes()
	{
		currentEquipPanelShowIndex = 3;
		equipPanelHideAction = HideEquip_Shoes;
		shoesBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowShoesInfo;
		LoadEquip_Shoes();
	}

	private void HideEquip_Shoes()
	{
		RecycleAllEquipSlot();
		shoesBtn.interactable = true;
	}

	private void LoadEquip_Shoes()
	{
		List<string> allShoes = Singleton<GameManager>.Instance.Player.PlayerInventory.AllShoes;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newShoesAmount = 0;
		for (int i = 0; i < allShoes.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allShoes[i]);
			equipSlot.LoadEquip(this, RemoveNewShoes, allShoes[i], flag);
			if (flag)
			{
				newShoesAmount++;
			}
			allShowingEquipSlots.Add(allShoes[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowShoesInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingShoes, isInteractive: true);
	}

	private void RemoveNewShoes(string equipCode)
	{
		if (newShoesAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newShoesAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newShoesAmount == 0)
		{
			newShoesIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingShoes()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.Shoes.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipShoes(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.Shoes, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewShoes, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipOrnamentPanel()
	{
		ornamentBtn = equipPanel.Find("ButtonList/Ornament").GetComponent<Button>();
		newOrnamentIconImg = equipPanel.Find("ButtonList/Ornament/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_Ornament()
	{
		currentEquipPanelShowIndex = 4;
		equipPanelHideAction = HideEquip_Ornament;
		ornamentBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowOrnamentInfo;
		LoadEquip_Ornament();
	}

	private void HideEquip_Ornament()
	{
		RecycleAllEquipSlot();
		ornamentBtn.interactable = true;
	}

	private void LoadEquip_Ornament()
	{
		List<string> allOrnaments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllOrnaments;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newOrnamentAmount = 0;
		for (int i = 0; i < allOrnaments.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allOrnaments[i]);
			equipSlot.LoadEquip(this, RemoveNewOrnament, allOrnaments[i], flag);
			if (flag)
			{
				newOrnamentAmount++;
			}
			allShowingEquipSlots.Add(allOrnaments[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowOrnamentInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingOrnament, isInteractive: true);
	}

	private void RemoveNewOrnament(string equipCode)
	{
		if (newOrnamentAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newOrnamentAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newOrnamentAmount == 0)
		{
			newOrnamentIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingOrnament()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.Ornament.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipOrnament(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.Ornament, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewOrnament, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipMainHandPanel()
	{
		mainhandBtn = equipPanel.Find("ButtonList/MainHand").GetComponent<Button>();
		newMainHandIconImg = equipPanel.Find("ButtonList/MainHand/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_MainHand()
	{
		currentEquipPanelShowIndex = 5;
		equipPanelHideAction = HideEquip_MainHand;
		mainhandBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowMainHandInfo;
		LoadEquip_MainHand();
	}

	private void HideEquip_MainHand()
	{
		RecycleAllEquipSlot();
		mainhandBtn.interactable = true;
	}

	private void LoadEquip_MainHand()
	{
		List<string> allMainHands = Singleton<GameManager>.Instance.Player.PlayerInventory.AllMainHands;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newMainHandAmount = 0;
		for (int i = 0; i < allMainHands.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allMainHands[i]);
			equipSlot.LoadEquip(this, RemoveNewMainHand, allMainHands[i], flag);
			if (flag)
			{
				newMainHandAmount++;
			}
			allShowingEquipSlots.Add(allMainHands[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowMainHandInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingMainHand, isInteractive: true);
	}

	private void RemoveNewMainHand(string equipCode)
	{
		if (newMainHandAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newMainHandAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newMainHandAmount == 0)
		{
			newMainHandIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingMainHand()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.MainHandWeapon.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipMainHandWeapon(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.MainHandWeapon, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewMainHand, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}

	private void InitEquipSupHandPanel()
	{
		suphandBtn = equipPanel.Find("ButtonList/SupHand").GetComponent<Button>();
		newSupHandIconImg = equipPanel.Find("ButtonList/SupHand/NewImg").GetComponent<Image>();
	}

	private void ShowEquip_SupHand()
	{
		currentEquipPanelShowIndex = 6;
		equipPanelHideAction = HideEquip_SupHand;
		suphandBtn.interactable = false;
		ShowingEquipmentInfoAction = ShowSupHandInfo;
		LoadEquip_SupHand();
	}

	private void HideEquip_SupHand()
	{
		RecycleAllEquipSlot();
		suphandBtn.interactable = true;
	}

	private void LoadEquip_SupHand()
	{
		List<string> allSupHands = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSupHands;
		HashSet<string> allNewEquipments = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments;
		newSupHandAmount = 0;
		for (int i = 0; i < allSupHands.Count; i++)
		{
			EquipSlotCtrl equipSlot = GetEquipSlot();
			bool flag = allNewEquipments.Contains(allSupHands[i]);
			equipSlot.LoadEquip(this, RemoveNewSupHand, allSupHands[i], flag);
			if (flag)
			{
				newSupHandAmount++;
			}
			allShowingEquipSlots.Add(allSupHands[i], equipSlot);
		}
		SetScrollbar();
	}

	public void ShowSupHandInfo(string equipCode)
	{
		currentShowingEquipCode = equipCode;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadContrastEquipment(equipCode, "Equip".LocalizeText(), ConfirmEquipCurrentShowingSupHand, isInteractive: true);
	}

	private void RemoveNewSupHand(string equipCode)
	{
		if (newSupHandAmount <= 0 || !Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewEquipment(equipCode))
		{
			return;
		}
		newSupHandAmount--;
		allShowingEquipSlots[equipCode].SetNewIconActive(isActive: false);
		if (newSupHandAmount == 0)
		{
			newSupHandIconImg.gameObject.SetActive(value: false);
			if (Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewEquipments.Count == 0)
			{
				parentUI.CancelBagNewEquipImg();
			}
		}
	}

	private void ConfirmEquipCurrentShowingSupHand()
	{
		parentUI.SetChanged(isChanged: true);
		EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard(currentShowingEquipCode);
		string cardCode = Singleton<GameManager>.Instance.Player.PlayerEquipment.SupHandWeapon.CardCode;
		Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipSupHandWeapon(equipmentCard);
		Singleton<GameManager>.Instance.Player.PlayerInventory.EquipEquipment(EquipmentType.SupHandWeapon, cardCode, equipmentCard.CardCode);
		EquipSlotCtrl equipSlotCtrl = allShowingEquipSlots[currentShowingEquipCode];
		allShowingEquipSlots.Remove(currentShowingEquipCode);
		equipSlotCtrl.LoadEquip(this, RemoveNewSupHand, cardCode, isNew: false);
		allShowingEquipSlots.Add(cardCode, equipSlotCtrl);
		PlayEquipSound(equipmentCard.EquipmentCardAttr.EquipSoundName);
	}
}
