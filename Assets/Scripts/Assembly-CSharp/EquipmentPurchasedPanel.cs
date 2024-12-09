using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPurchasedPanel : SerializedMonoBehaviour
{
	private class EquipmentPurchasedHandler
	{
		private EquipmentType _equipmentType;

		private Button btn;

		private EquipmentPurchasedPanel parentPanel;

		public EquipmentPurchasedHandler(EquipmentPurchasedPanel parentPanel, Button btn, EquipmentType equipmentType)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			_equipmentType = equipmentType;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisPanel();
		}

		public void OnSetThisPanel()
		{
			parentPanel.RecycleAllShowingCtrls();
			SetBtnInteracitve(isActive: false);
			parentPanel.SetCurrenEquipPurchasedHandler(this);
			foreach (KeyValuePair<string, ItemPurchasedData> item in DataManager.Instance.GetAllEquipmentPurchasedDatasByType(_equipmentType))
			{
				SinglePurchasedEquipmentCtrl singlePurchasedEquipmentCtrl = parentPanel.GetSinglePurchasedEquipmentCtrl();
				singlePurchasedEquipmentCtrl.transform.SetAsLastSibling();
				singlePurchasedEquipmentCtrl.LoadEquipment(item.Key, parentPanel, _equipmentType, SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEquipmentPurchased(item.Key));
				singlePurchasedEquipmentCtrl.CanvasGroup.DOKill();
				singlePurchasedEquipmentCtrl.CanvasGroup.Fade(0f, 1f);
				parentPanel.AddShowingCtrl(singlePurchasedEquipmentCtrl);
			}
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SinglePurchasedEquipmentCtrl allShowingPurchasedEquipCtrl in parentPanel.allShowingPurchasedEquipCtrls)
			{
				list.Add(allShowingPurchasedEquipCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			parentPanel.parentPanel.SetScrolllbar();
		}

		public void Hide()
		{
			SetBtnInteracitve(isActive: true);
		}

		private void SetBtnInteracitve(bool isActive)
		{
			btn.interactable = isActive;
		}
	}

	public Dictionary<PlayerOccupation, Sprite> OccupationIcon;

	private Transform equipItemRoot;

	private EquipmentPurchasedHandler breasplateHandler;

	private EquipmentPurchasedHandler gloveHandler;

	private EquipmentPurchasedHandler helmetHandler;

	private EquipmentPurchasedHandler mainhandHandler;

	private EquipmentPurchasedHandler suphandHandler;

	private EquipmentPurchasedHandler shoesHandler;

	private EquipmentPurchasedHandler ornamentHandler;

	private EquipmentPurchasedHandler currentHandler;

	private UIAnim_Common anim;

	private PurchasedItemUI parentPanel;

	private Queue<SinglePurchasedEquipmentCtrl> allPurchasedEquipCtrlPools = new Queue<SinglePurchasedEquipmentCtrl>();

	private List<SinglePurchasedEquipmentCtrl> allShowingPurchasedEquipCtrls = new List<SinglePurchasedEquipmentCtrl>();

	public PurchasedItemUI ParentPanel => parentPanel;

	private void Awake()
	{
		equipItemRoot = base.transform.Find("EquipShowPanel/Mask/Content");
		breasplateHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Breastplate").GetComponent<Button>(), EquipmentType.Breastplate);
		gloveHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Glove").GetComponent<Button>(), EquipmentType.Glove);
		helmetHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Helmet").GetComponent<Button>(), EquipmentType.Helmet);
		mainhandHandler = new EquipmentPurchasedHandler(this, base.transform.Find("MainHand").GetComponent<Button>(), EquipmentType.MainHandWeapon);
		suphandHandler = new EquipmentPurchasedHandler(this, base.transform.Find("SupHand").GetComponent<Button>(), EquipmentType.SupHandWeapon);
		shoesHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Shoes").GetComponent<Button>(), EquipmentType.Shoes);
		ornamentHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Ornament").GetComponent<Button>(), EquipmentType.Ornament);
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(PurchasedItemUI parentPanel)
	{
		this.parentPanel = parentPanel;
		base.gameObject.SetActive(value: true);
		helmetHandler.OnSetThisPanel();
		anim.StartAnim();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		currentHandler.Hide();
		currentHandler = null;
	}

	private void SetCurrenEquipPurchasedHandler(EquipmentPurchasedHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	public void AddShowingCtrl(SinglePurchasedEquipmentCtrl ctrl)
	{
		allShowingPurchasedEquipCtrls.Add(ctrl);
	}

	public SinglePurchasedEquipmentCtrl GetSinglePurchasedEquipmentCtrl()
	{
		if (allPurchasedEquipCtrlPools.Count > 0)
		{
			SinglePurchasedEquipmentCtrl singlePurchasedEquipmentCtrl = allPurchasedEquipCtrlPools.Dequeue();
			singlePurchasedEquipmentCtrl.gameObject.SetActive(value: true);
			return singlePurchasedEquipmentCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SinglePurchasedEquipmentCtrl", "Prefabs", equipItemRoot).GetComponent<SinglePurchasedEquipmentCtrl>();
	}

	public void RecycleAllShowingCtrls()
	{
		if (allShowingPurchasedEquipCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingPurchasedEquipCtrls.Count; i++)
			{
				allShowingPurchasedEquipCtrls[i].gameObject.SetActive(value: false);
				allPurchasedEquipCtrlPools.Enqueue(allShowingPurchasedEquipCtrls[i]);
			}
			allShowingPurchasedEquipCtrls.Clear();
		}
	}
}
