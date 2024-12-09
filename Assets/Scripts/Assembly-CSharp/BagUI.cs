using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : EscCloseUIView
{
	private abstract class BagUI_Handler
	{
		private BagUI parentPanel;

		private Button btn;

		private ImageExtent _imageExtent;

		protected BagUI_Handler(BagUI parentPanel, Button btn)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			_imageExtent = btn.GetComponent<ImageExtent>();
			_imageExtent.material = Object.Instantiate(_imageExtent.material);
			_imageExtent.toggleTint = true;
			btn.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			if (parentPanel.CheckSwitchPanel())
			{
				Show();
			}
		}

		public void Hide()
		{
			_imageExtent.toggleTint = true;
			_imageExtent.SetMaterialDirty();
			btn.interactable = true;
			OnHide();
		}

		protected abstract void OnHide();

		public void Show()
		{
			_imageExtent.toggleTint = false;
			_imageExtent.SetMaterialDirty();
			btn.interactable = false;
			OnShow();
			parentPanel.SwitchHandler(this);
		}

		protected abstract void OnShow();

		public abstract string HideCheck();
	}

	private class BagUI_CardHandler : BagUI_Handler
	{
		private BagUI_CardPanel _cardPanel;

		public BagUI_CardHandler(BagUI parentPanel, BagUI_CardPanel cardPanel, GameObject dragCardObj, UsualNoDesCardInfo cardInfo, Button btn)
			: base(parentPanel, btn)
		{
			_cardPanel = cardPanel;
			_cardPanel.InitCardPanel(parentPanel, dragCardObj, cardInfo);
		}

		protected override void OnHide()
		{
			_cardPanel.Hide();
			Singleton<GameHintManager>.Instance.RecycleAll();
		}

		protected override void OnShow()
		{
			_cardPanel.Show();
		}

		public override string HideCheck()
		{
			return _cardPanel.CloseBagCardPanelCheck();
		}
	}

	private class BagUI_SkillHandler : BagUI_Handler
	{
		private BagUI_SkillPanel _bagUiSkillPanel;

		public BagUI_SkillHandler(BagUI parentPanel, BagUI_SkillPanel bagUiSkillPanel, GameObject dragSkillObj, Image dragSkillIconImg, Button btn)
			: base(parentPanel, btn)
		{
			_bagUiSkillPanel = bagUiSkillPanel;
			_bagUiSkillPanel.InitSkillPanel(parentPanel, dragSkillObj, dragSkillIconImg);
		}

		protected override void OnHide()
		{
			_bagUiSkillPanel.Hide();
		}

		protected override void OnShow()
		{
			_bagUiSkillPanel.Show();
		}

		public override string HideCheck()
		{
			return string.Empty;
		}
	}

	private class BagUI_EquipHandler : BagUI_Handler
	{
		private BagUI_EquipPanel _bagUiEquipPanel;

		public BagUI_EquipHandler(BagUI parentPanel, BagUI_EquipPanel bagUiEquipPanel, Button btn)
			: base(parentPanel, btn)
		{
			_bagUiEquipPanel = bagUiEquipPanel;
			_bagUiEquipPanel.InitEquipPanel(parentPanel);
		}

		protected override void OnHide()
		{
			_bagUiEquipPanel.Hide();
		}

		protected override void OnShow()
		{
			_bagUiEquipPanel.Show();
		}

		public override string HideCheck()
		{
			return string.Empty;
		}
	}

	private static bool isAnyChanged;

	[HideInInspector]
	public UIAnim_BagUI uiAnim;

	private GuideTipsBtn _guideTipsBtn;

	private Text coinAmount;

	private Image cardsNewImg;

	private Image skillNewImg;

	private Image equipNewImg;

	private BagUI_Handler currentHandler;

	private BagUI_CardHandler _bagUiCardHandler;

	private BagUI_SkillHandler _bagUiSkillHandler;

	private BagUI_EquipHandler _bagUiEquipHandler;

	public override string UIViewName => "BagUI";

	public override string UILayerName => "NormalLayer";

	public Transform bubbleHintPoint { get; private set; }

	public void SetChanged(bool isChanged)
	{
		isAnyChanged = isChanged;
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		EventManager.RegisterEvent(EventEnum.E_PlayerCoinUpdate, UpdatePlayerCoin);
		UpdatePlayerCoin(null);
		isAnyChanged = false;
		InitBagButtons();
		uiAnim.StartAnim();
		_bagUiEquipHandler.Show();
	}

	public override void HideView()
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerCoinUpdate, UpdatePlayerCoin);
		base.gameObject.SetActive(value: false);
		currentHandler?.Hide();
		currentHandler = null;
		if (isAnyChanged)
		{
			GameSave.SaveGame();
		}
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickReturnBtn();
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Bag UI...");
	}

	public override void OnSpawnUI()
	{
		_guideTipsBtn = base.transform.Find("GuideTipsBtn").GetComponent<GuideTipsBtn>();
		bubbleHintPoint = base.transform.Find("BubbleHintPoint");
		uiAnim = GetComponent<UIAnim_BagUI>();
		uiAnim.Init();
		InitMenu();
		InitHandler();
	}

	public void AddGuideTips(List<string> tipsList)
	{
		_guideTipsBtn.AddGuideTips(tipsList);
	}

	private void InitMenu()
	{
		cardsNewImg = base.transform.Find("Bg/ButtonList/Cards/NewImg").GetComponent<Image>();
		skillNewImg = base.transform.Find("Bg/ButtonList/Skill/NewImg").GetComponent<Image>();
		equipNewImg = base.transform.Find("Bg/ButtonList/Equip/NewImg").GetComponent<Image>();
		coinAmount = base.transform.Find("Bg/CoinAmount").GetComponent<Text>();
		base.transform.Find("Bg/ButtonList/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
	}

	private void InitBagButtons()
	{
		Player player = Singleton<GameManager>.Instance.Player;
		cardsNewImg.gameObject.SetActive(player.PlayerInventory.AllNewCards.Count > 0);
		skillNewImg.gameObject.SetActive(player.PlayerInventory.AllNewSkills.Count > 0);
		equipNewImg.gameObject.SetActive(player.PlayerInventory.AllNewEquipments.Count > 0);
	}

	public void CancelBagNewCardImg()
	{
		cardsNewImg.gameObject.SetActive(value: false);
	}

	public void CancelBagNewSkillImg()
	{
		skillNewImg.gameObject.SetActive(value: false);
	}

	public void CancelBagNewEquipImg()
	{
		equipNewImg.gameObject.SetActive(value: false);
	}

	private void OnClickReturnBtn()
	{
		string text = currentHandler.HideCheck();
		if (text != string.Empty)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowOneChosenSystemHint(text, null);
			return;
		}
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("关闭背包窗口");
		SingletonDontDestroy<UIManager>.Instance.HideView("BagUI");
		SingletonDontDestroy<UIManager>.Instance.ShowView("RoomUI");
	}

	public void UpdatePlayerCoin(EventData data)
	{
		coinAmount.text = $"{Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney}G";
	}

	private void InitHandler()
	{
		_bagUiCardHandler = new BagUI_CardHandler(this, base.transform.Find("CardPanel").GetComponent<BagUI_CardPanel>(), base.transform.Find("BagCard").gameObject, base.transform.Find("BagCard/UsualNoDesCard").GetComponent<UsualNoDesCardInfo>(), base.transform.Find("Bg/ButtonList/Cards").GetComponent<Button>());
		_bagUiSkillHandler = new BagUI_SkillHandler(this, base.transform.Find("SkillPanel").GetComponent<BagUI_SkillPanel>(), base.transform.Find("SkillSlot").gameObject, base.transform.Find("SkillSlot/Icon").GetComponent<Image>(), base.transform.Find("Bg/ButtonList/Skill").GetComponent<Button>());
		_bagUiEquipHandler = new BagUI_EquipHandler(this, base.transform.Find("EquipPanel").GetComponent<BagUI_EquipPanel>(), base.transform.Find("Bg/ButtonList/Equip").GetComponent<Button>());
	}

	private void SwitchHandler(BagUI_Handler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	private bool CheckSwitchPanel()
	{
		if (currentHandler != null)
		{
			string text = currentHandler.HideCheck();
			if (text != string.Empty)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowOneChosenSystemHint(text, null);
				return false;
			}
		}
		return true;
	}
}
