using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPurchasedPanel : MonoBehaviour
{
	private class SkillPurchasedHandler
	{
		private SkillPurchasedPanel parentPanel;

		private Button btn;

		private PlayerOccupation playerOccupation;

		public SkillPurchasedHandler(SkillPurchasedPanel parentPanel, Button btn, PlayerOccupation playerOccupation)
		{
			this.parentPanel = parentPanel;
			this.btn = btn;
			this.playerOccupation = playerOccupation;
			btn.onClick.AddListener(Show);
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisPanel();
		}

		public void OnSetThisPanel()
		{
			parentPanel.RecycleAllShowingPurchasedCtrls();
			foreach (KeyValuePair<string, ItemPurchasedData> item in DataManager.Instance.GetAllPurchasedDatasByOccupation(playerOccupation))
			{
				SinglePurchasedSkillCtrl singlePurchasedSkillCtrl = parentPanel.GetSinglePurchasedSkillCtrl();
				singlePurchasedSkillCtrl.transform.SetAsLastSibling();
				parentPanel.AddShowingCtrl(singlePurchasedSkillCtrl);
				bool isPurchased = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSkillPurchased(item.Key);
				singlePurchasedSkillCtrl.LoadSkill(parentPanel.parentPanel, playerOccupation, item.Value, isPurchased);
			}
			List<CanvasGroup> list = new List<CanvasGroup>();
			foreach (SinglePurchasedSkillCtrl allShowingPurchasedSkillCtrl in parentPanel.allShowingPurchasedSkillCtrls)
			{
				list.Add(allShowingPurchasedSkillCtrl.CanvasGroup);
			}
			parentPanel.anim.SetSlotsAnim(list);
			SetBtnInteractive(isActive: false);
			parentPanel.SetCurentHandler(this);
			parentPanel.parentPanel.SetScrolllbar();
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

	private Transform skillItemRoot;

	private SkillPurchasedHandler currentHandler;

	private SkillPurchasedHandler archerHandler;

	private SkillPurchasedHandler knightHandler;

	private PurchasedItemUI parentPanel;

	private UIAnim_Common anim;

	private Queue<SinglePurchasedSkillCtrl> allPurchasedSkillCtrlPool = new Queue<SinglePurchasedSkillCtrl>();

	private List<SinglePurchasedSkillCtrl> allShowingPurchasedSkillCtrls = new List<SinglePurchasedSkillCtrl>();

	private void Awake()
	{
		skillItemRoot = base.transform.Find("SkillShowPanel/Mask/Content");
		archerHandler = new SkillPurchasedHandler(this, base.transform.Find("Archer").GetComponent<Button>(), PlayerOccupation.Archer);
		knightHandler = new SkillPurchasedHandler(this, base.transform.Find("Knight").GetComponent<Button>(), PlayerOccupation.Knight);
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(PurchasedItemUI parentPanel)
	{
		this.parentPanel = parentPanel;
		base.gameObject.SetActive(value: true);
		archerHandler.OnSetThisPanel();
		anim.StartAnim();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
		currentHandler.Hide();
		currentHandler = null;
	}

	private void SetCurentHandler(SkillPurchasedHandler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	public void AddShowingCtrl(SinglePurchasedSkillCtrl ctrl)
	{
		allShowingPurchasedSkillCtrls.Add(ctrl);
	}

	public SinglePurchasedSkillCtrl GetSinglePurchasedSkillCtrl()
	{
		if (allPurchasedSkillCtrlPool.Count > 0)
		{
			SinglePurchasedSkillCtrl singlePurchasedSkillCtrl = allPurchasedSkillCtrlPool.Dequeue();
			singlePurchasedSkillCtrl.gameObject.SetActive(value: true);
			return singlePurchasedSkillCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SinglePurchasedSkillCtrl", "Prefabs", skillItemRoot).GetComponent<SinglePurchasedSkillCtrl>();
	}

	public void RecycleAllShowingPurchasedCtrls()
	{
		if (allShowingPurchasedSkillCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingPurchasedSkillCtrls.Count; i++)
			{
				allShowingPurchasedSkillCtrls[i].gameObject.SetActive(value: false);
				allPurchasedSkillCtrlPool.Enqueue(allShowingPurchasedSkillCtrls[i]);
			}
			allShowingPurchasedSkillCtrls.Clear();
		}
	}
}
