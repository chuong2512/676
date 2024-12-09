using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentIlluBookPanel : SerializedMonoBehaviour, IllustratedBooksUI.IIlluPanel
{
	private class EquipmentIlluBookHandler
	{
		private Button btn;

		private EquipmentIlluBookPanel parentPanel;

		private EquipmentType equipmentType;

		public EquipmentIlluBookHandler(EquipmentIlluBookPanel parentPanel, Button btn, EquipmentType equipmentType)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			this.equipmentType = equipmentType;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisHandler();
		}

		public void OnSetThisHandler()
		{
			SetBtnInteractive(isActive: false);
			ShowAllEquipments();
			parentPanel.SetCurrentHandler(this);
		}

		private void ShowAllEquipments()
		{
			int num = 0;
			parentPanel.RecycleAllIlluEquipmentCtrl();
			Dictionary<string, EquipmentCardAttr> singleTypeEquipmentCardDatas = DataManager.Instance.GetSingleTypeEquipmentCardDatas(equipmentType);
			foreach (KeyValuePair<string, EquipmentCardAttr> item in singleTypeEquipmentCardDatas)
			{
				SingleIlluEquipmentCtrl singleIlluEquiomentCtrl = parentPanel.GetSingleIlluEquiomentCtrl();
				singleIlluEquiomentCtrl.transform.SetAsLastSibling();
				bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockEquipmentIllu(item.Key);
				bool isPurchased = !item.Value.IsNeedPurchased || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEquipmentPurchased(item.Key);
				singleIlluEquiomentCtrl.LoadEquipment(item.Value, parentPanel, flag ? parentPanel.showMaterial : parentPanel.hideMaterial, flag, isPurchased);
				parentPanel.AddShowingIlluEquipCtrl(singleIlluEquiomentCtrl);
				if (flag)
				{
					num++;
				}
			}
			parentPanel.ShowProgressInfo(singleTypeEquipmentCardDatas.Count, num);
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SingleIlluEquipmentCtrl allShowingIlluEquipCtrl in parentPanel.allShowingIlluEquipCtrls)
			{
				list.Add(allShowingIlluEquipCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			parentPanel.ParentPanel.SetScrolllbar();
		}

		public void Hide()
		{
			SetBtnInteractive(isActive: true);
		}

		private void SetBtnInteractive(bool isActive)
		{
			btn.interactable = isActive;
		}
	}

	public Material showMaterial;

	public Material hideMaterial;

	public Dictionary<PlayerOccupation, Sprite> OccupationIcon;

	private EquipmentIlluBookHandler helmetHandler;

	private EquipmentIlluBookHandler breastplateHandler;

	private EquipmentIlluBookHandler gloveHandler;

	private EquipmentIlluBookHandler shoesHandler;

	private EquipmentIlluBookHandler ornamentHandler;

	private EquipmentIlluBookHandler mainhandHandler;

	private EquipmentIlluBookHandler suphandHandler;

	private Transform equipItemRoot;

	private UIAnim_Common anim;

	private EquipmentIlluBookHandler currentHandler;

	private Action<int, int> unlockProgressAction;

	private Queue<SingleIlluEquipmentCtrl> allIlluEquipCtrlPools = new Queue<SingleIlluEquipmentCtrl>();

	private List<SingleIlluEquipmentCtrl> allShowingIlluEquipCtrls = new List<SingleIlluEquipmentCtrl>();

	public IllustratedBooksUI ParentPanel { get; private set; }

	private void Awake()
	{
		helmetHandler = new EquipmentIlluBookHandler(this, base.transform.Find("Helmet").GetComponent<Button>(), EquipmentType.Helmet);
		breastplateHandler = new EquipmentIlluBookHandler(this, base.transform.Find("Breastplate").GetComponent<Button>(), EquipmentType.Breastplate);
		gloveHandler = new EquipmentIlluBookHandler(this, base.transform.Find("Glove").GetComponent<Button>(), EquipmentType.Glove);
		shoesHandler = new EquipmentIlluBookHandler(this, base.transform.Find("Shoes").GetComponent<Button>(), EquipmentType.Shoes);
		ornamentHandler = new EquipmentIlluBookHandler(this, base.transform.Find("Ornament").GetComponent<Button>(), EquipmentType.Ornament);
		mainhandHandler = new EquipmentIlluBookHandler(this, base.transform.Find("MainHand").GetComponent<Button>(), EquipmentType.MainHandWeapon);
		suphandHandler = new EquipmentIlluBookHandler(this, base.transform.Find("SupHand").GetComponent<Button>(), EquipmentType.SupHandWeapon);
		equipItemRoot = base.transform.Find("EquipShowPanel/Mask/Content");
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(IllustratedBooksUI parentPanel, Action<int, int> unlockProgressAction)
	{
		ParentPanel = parentPanel;
		this.unlockProgressAction = unlockProgressAction;
		base.gameObject.SetActive(value: true);
		helmetHandler.OnSetThisHandler();
		anim.StartAnim();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		currentHandler?.Hide();
		currentHandler = null;
	}

	public void ShowProgressInfo(int maxAmount, int currentAmount)
	{
		unlockProgressAction(maxAmount, currentAmount);
	}

	private void SetCurrentHandler(EquipmentIlluBookHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	public void AddShowingIlluEquipCtrl(SingleIlluEquipmentCtrl ctrl)
	{
		allShowingIlluEquipCtrls.Add(ctrl);
	}

	public SingleIlluEquipmentCtrl GetSingleIlluEquiomentCtrl()
	{
		if (allIlluEquipCtrlPools.Count > 0)
		{
			SingleIlluEquipmentCtrl singleIlluEquipmentCtrl = allIlluEquipCtrlPools.Dequeue();
			singleIlluEquipmentCtrl.gameObject.SetActive(value: true);
			return singleIlluEquipmentCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleIlluEquipmentCtrl", "Prefabs", equipItemRoot).GetComponent<SingleIlluEquipmentCtrl>();
	}

	public void RecycleAllIlluEquipmentCtrl()
	{
		if (allShowingIlluEquipCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingIlluEquipCtrls.Count; i++)
			{
				allShowingIlluEquipCtrls[i].gameObject.SetActive(value: false);
				allIlluEquipCtrlPools.Enqueue(allShowingIlluEquipCtrls[i]);
			}
			allShowingIlluEquipCtrls.Clear();
		}
	}
}
