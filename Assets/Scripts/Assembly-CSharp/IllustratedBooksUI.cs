using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IllustratedBooksUI : EscCloseUIView
{
	private class IllustaredBooksPanelHandler
	{
		private Button btn;

		private IllustratedBooksUI parentPanel;

		private IIlluPanel illuPanel;

		private Transform featherPoint;

		public IllustaredBooksPanelHandler(IllustratedBooksUI parentPanel, Button btn, IIlluPanel illuPanel, Transform featherPoint)
		{
			this.btn = btn;
			this.parentPanel = parentPanel;
			this.illuPanel = illuPanel;
			this.featherPoint = featherPoint;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
			OnSetThisType();
		}

		public void OnSetThisType()
		{
			parentPanel.SetCurrentHandler(this);
			parentPanel.SetFeather(featherPoint);
			SetInteractive(isActive: false);
			illuPanel.Show(parentPanel, ProgressAction);
		}

		public void Hide()
		{
			SetInteractive(isActive: true);
			illuPanel.Hide();
		}

		private void SetInteractive(bool isActive)
		{
			btn.interactable = isActive;
		}

		private void ProgressAction(int maxAmount, int currentAmount)
		{
			parentPanel.SetUnlockProgressInfo(maxAmount, currentAmount);
		}
	}

	public interface IIlluPanel
	{
		void Show(IllustratedBooksUI parentUI, Action<int, int> unlockProgressAction);

		void Hide();
	}

	private UIAnim_RightBtnUI anim;

	private const string UNLOCKEDKEY = "Unlocked";

	private Image unlockProgressBar;

	private Text unlockProgressText;

	private IllustaredBooksPanelHandler currentPanelHandler;

	private IllustaredBooksPanelHandler monsterIlluBookHandler;

	private IllustaredBooksPanelHandler equipIlluBookHandler;

	private IllustaredBooksPanelHandler skillIlluBookHandler;

	private IllustaredBooksPanelHandler cardIlluBookHandler;

	private IllustaredBooksPanelHandler plotIlluBookHandler;

	private Scrollbar usualScrollbar;

	private Transform featherTrans;

	public override string UIViewName => "IllustratedBooksUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入图鉴界面");
		base.gameObject.SetActive(value: true);
		monsterIlluBookHandler.OnSetThisType();
		anim.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		currentPanelHandler?.Hide();
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
		monsterIlluBookHandler = new IllustaredBooksPanelHandler(this, base.transform.Find("Bg/BtnList/MonsterBtn").GetComponent<Button>(), base.transform.Find("Bg/MonsterPanel").GetComponent<MonsterIlluBookPanel>(), base.transform.Find("Bg/MonsterPanel/FeatherPoint"));
		equipIlluBookHandler = new IllustaredBooksPanelHandler(this, base.transform.Find("Bg/BtnList/EquipBtn").GetComponent<Button>(), base.transform.Find("Bg/EquipmentPanel").GetComponent<EquipmentIlluBookPanel>(), base.transform.Find("Bg/EquipmentPanel/FeatherPoint"));
		skillIlluBookHandler = new IllustaredBooksPanelHandler(this, base.transform.Find("Bg/BtnList/SkillBtn").GetComponent<Button>(), base.transform.Find("Bg/SkillPanel").GetComponent<SkillIlluBookPanel>(), base.transform.Find("Bg/SkillPanel/FeatherPoint"));
		cardIlluBookHandler = new IllustaredBooksPanelHandler(this, base.transform.Find("Bg/BtnList/CardBtn").GetComponent<Button>(), base.transform.Find("Bg/CardPanel").GetComponent<CardIllustrBookPanel>(), base.transform.Find("Bg/CardPanel/FeatherPoint"));
		plotIlluBookHandler = new IllustaredBooksPanelHandler(this, base.transform.Find("Bg/BtnList/PlotBtn").GetComponent<Button>(), base.transform.Find("Bg/PlotPanel").GetComponent<PlotIlluBookPanel>(), base.transform.Find("Bg/PlotPanel/FeatherPoint"));
		base.transform.Find("Bg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
		unlockProgressBar = base.transform.Find("Bg/UnlockProgressBottom/ProgressBar").GetComponent<Image>();
		unlockProgressText = base.transform.Find("Bg/UnlockProgressBottom/UnlockText").GetComponent<Text>();
		anim = GetComponent<UIAnim_RightBtnUI>();
		anim.Init();
		usualScrollbar = base.transform.Find("Bg/Scrollbar").GetComponent<Scrollbar>();
		featherTrans = base.transform.Find("Bg/Feather");
	}

	public void SetFeather(Transform featherPoint)
	{
		featherTrans.SetParent(featherPoint);
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

	private void SetCurrentHandler(IllustaredBooksPanelHandler handler)
	{
		currentPanelHandler?.Hide();
		currentPanelHandler = handler;
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public void SetUnlockProgressInfo(int maxAmount, int currentAmount)
	{
		float fillAmount = (float)currentAmount / (float)maxAmount;
		unlockProgressBar.fillAmount = fillAmount;
		unlockProgressText.text = string.Format("{0} {1}/{2}", "Unlocked".LocalizeText(), currentAmount, maxAmount);
	}
}
