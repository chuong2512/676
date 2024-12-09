using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurchasedItemUI : EscCloseUIView
{
	private abstract class PurchasedHandler
	{
		protected PurchasedItemUI parentPanel;

		protected Button btn;

		private Transform featherRoot;

		public PurchasedHandler(PurchasedItemUI parentPanel, Button btn, Transform featherRoot)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			this.featherRoot = featherRoot;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
			OnSetThisType();
		}

		public void OnSetThisType()
		{
			SetBtnInteractive(isActive: false);
			parentPanel.SetCurrentHandler(this);
			parentPanel.SetFeather(featherRoot);
			OnShow();
		}

		protected abstract void OnShow();

		public void Hide()
		{
			SetBtnInteractive(isActive: true);
			OnHide();
		}

		protected abstract void OnHide();

		private void SetBtnInteractive(bool isActive)
		{
			btn.interactable = isActive;
		}
	}

	private class EquipmentPurchasedHandler : PurchasedHandler
	{
		private EquipmentPurchasedPanel _equipmentPurchasedPanel;

		public EquipmentPurchasedHandler(PurchasedItemUI parentPanel, Button btn, EquipmentPurchasedPanel equipmentPurchasedPanel, Transform featherRoot)
			: base(parentPanel, btn, featherRoot)
		{
			_equipmentPurchasedPanel = equipmentPurchasedPanel;
		}

		protected override void OnShow()
		{
			_equipmentPurchasedPanel.Show(parentPanel);
		}

		protected override void OnHide()
		{
			_equipmentPurchasedPanel.Hide();
		}
	}

	private class SkillPurchasedHandler : PurchasedHandler
	{
		private SkillPurchasedPanel _skillPurchasedPanel;

		public SkillPurchasedHandler(PurchasedItemUI parentPanel, Button btn, SkillPurchasedPanel skillPurchasedPanel, Transform featherRoot)
			: base(parentPanel, btn, featherRoot)
		{
			_skillPurchasedPanel = skillPurchasedPanel;
		}

		protected override void OnShow()
		{
			_skillPurchasedPanel.Show(parentPanel);
		}

		protected override void OnHide()
		{
			_skillPurchasedPanel.Hide();
		}
	}

	private class CardPurchasedHandler : PurchasedHandler
	{
		private CardPurchasedPanel _cardPurchasedPanel;

		public CardPurchasedHandler(PurchasedItemUI parentPanel, Button btn, CardPurchasedPanel cardPurchasedPanel, Transform featherRoot)
			: base(parentPanel, btn, featherRoot)
		{
			_cardPurchasedPanel = cardPurchasedPanel;
		}

		protected override void OnShow()
		{
			_cardPurchasedPanel.Show(parentPanel);
		}

		protected override void OnHide()
		{
			_cardPurchasedPanel.Hide();
		}
	}

	private UIAnim_RightBtnUI anim;

	public Sprite lockMask_CanPurchased;

	public Sprite lockMask_CannotPurchased;

	private Text timeSpaceText;

	private PurchasedHandler currentHandler;

	private EquipmentPurchasedHandler equipPurchasedHandler;

	private SkillPurchasedHandler skillPurchasedHandler;

	private CardPurchasedHandler cardPurchasedHandler;

	private GameObject maskObj;

	private Scrollbar usualScrollbar;

	private Transform featherTrans;

	public override string UIViewName => "PurchasedItemUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SetCurrentTimeSpaceAmount(SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin);
		equipPurchasedHandler.OnSetThisType();
		anim.StartAnim();
		maskObj.gameObject.SetActive(value: false);
		EventManager.RegisterEvent(EventEnum.E_DarkCrystalChanged, OnDarkCrystalChanged);
	}

	public override void HideView()
	{
		EventManager.UnregisterEvent(EventEnum.E_DarkCrystalChanged, OnDarkCrystalChanged);
		currentHandler?.Hide();
		currentHandler = null;
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickReturnBtn();
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public override void OnSpawnUI()
	{
		timeSpaceText = base.transform.Find("Bg/TimeSpaceText").GetComponent<Text>();
		base.transform.Find("Bg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
		equipPurchasedHandler = new EquipmentPurchasedHandler(this, base.transform.Find("Bg/BtnList/EquipBtn").GetComponent<Button>(), base.transform.Find("Bg/EquipmentPanel").GetComponent<EquipmentPurchasedPanel>(), base.transform.Find("Bg/EquipmentPanel/FeatherPoint"));
		skillPurchasedHandler = new SkillPurchasedHandler(this, base.transform.Find("Bg/BtnList/SkillBtn").GetComponent<Button>(), base.transform.Find("Bg/SkillPanel").GetComponent<SkillPurchasedPanel>(), base.transform.Find("Bg/SkillPanel/FeatherPoint"));
		cardPurchasedHandler = new CardPurchasedHandler(this, base.transform.Find("Bg/BtnList/CardBtn").GetComponent<Button>(), base.transform.Find("Bg/CardPanel").GetComponent<CardPurchasedPanel>(), base.transform.Find("Bg/CardPanel/FeatherPoint"));
		anim = GetComponent<UIAnim_RightBtnUI>();
		anim.Init();
		maskObj = base.transform.Find("Bg/Mask").gameObject;
		usualScrollbar = base.transform.Find("Bg/Scrollbar").GetComponent<Scrollbar>();
		featherTrans = base.transform.Find("Bg/Feather");
	}

	private void OnDarkCrystalChanged(EventData data)
	{
		SetCurrentTimeSpaceAmount(SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin);
	}

	private void SetFeather(Transform root)
	{
		featherTrans.SetParent(root);
		featherTrans.localPosition = Vector3.zero;
	}

	public void SetScrolllbar()
	{
		StartCoroutine(SetScrollbar_IE());
	}

	private IEnumerator SetScrollbar_IE()
	{
		yield return null;
		usualScrollbar.value = 1f;
	}

	private void SetCurrentTimeSpaceAmount(int amount)
	{
		timeSpaceText.text = amount.ToString();
	}

	private void SetCurrentHandler(PurchasedHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public void ShowPurchasedVfx(Transform target, float scale)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_ganwujiesuo_cast");
		maskObj.SetActive(value: true);
		vfxBase.transform.position = target.position;
		vfxBase.transform.localScale = Vector3.one * scale;
		vfxBase.Play();
		StartCoroutine(PurchasedVfx_IE());
	}

	private IEnumerator PurchasedVfx_IE()
	{
		yield return new WaitForSeconds(1.2f);
		maskObj.SetActive(value: false);
	}
}
