using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BagUI_SkillPanel : MonoBehaviour
{
	private Transform skillPanel;

	private Transform skillInventoryRoot;

	private Transform equipedSkillRoot;

	private Queue<SkillSlotCtrl> skillSlotPool = new Queue<SkillSlotCtrl>();

	private Dictionary<string, SkillSlotCtrl> allShowingInventorySkillSlots = new Dictionary<string, SkillSlotCtrl>();

	private Dictionary<int, SkillSlotCtrl> allShowingEquipedSkillSlots = new Dictionary<int, SkillSlotCtrl>();

	private Scrollbar skillInventoryScrollbar;

	private Tween skillInventoryScrollbarTween;

	private SkillSlotCtrl[] allEquipedSkillSlots;

	private Button skillLeftMoveBtn;

	private Button skillRightMoveBtn;

	private SkillSlotCtrl currentChoosenSkillSlot;

	private int currentChoosenSkillIndex;

	private ScrollRect skillInventoryScrollRect;

	private bool isMouseOnSkillInventoryPanel;

	private BagUI parentUI;

	private const string SkillIconAssetPath = "Sprites/SkillIcon";

	private GameObject dragSkillObj;

	private SkillSlotCtrl currentPointInSlot;

	private Image dragSkillIconImg;

	public SkillSlotCtrl CurrentPointInSlot => currentPointInSlot;

	public bool IsDragingSkillItem { get; private set; }

	public void InitSkillPanel(BagUI parentUI, GameObject dragSkillObj, Image dragSkillIconImg)
	{
		InitDragSkill(dragSkillObj, dragSkillIconImg);
		this.parentUI = parentUI;
		skillPanel = base.transform;
		skillInventoryRoot = skillPanel.Find("SkillInventoryRoot/Mask/Content");
		equipedSkillRoot = skillPanel.Find("SkillEquipedRoot");
		allEquipedSkillSlots = new SkillSlotCtrl[6];
		Transform transform = skillPanel.Find("SkillEquipedRoot");
		for (int i = 0; i < allEquipedSkillSlots.Length; i++)
		{
			allEquipedSkillSlots[i] = transform.GetChild(i).GetComponent<SkillSlotCtrl>();
		}
		skillLeftMoveBtn = skillPanel.Find("SkillInventoryRoot/LeftBtn").GetComponent<Button>();
		skillLeftMoveBtn.onClick.AddListener(OnClickSkillInventoryMoveRight);
		skillRightMoveBtn = skillPanel.Find("SkillInventoryRoot/RightBtn").GetComponent<Button>();
		skillRightMoveBtn.onClick.AddListener(OnClickSkillInventoryMoveLeft);
		OnMouseEventCallback component = skillPanel.Find("SkillInventoryRoot/Mask").GetComponent<OnMouseEventCallback>();
		component.EnterEventTrigger.Event.AddListener(OnMouseEnterSkillInventoryPanel);
		component.ExitEventTrigger.Event.AddListener(OnMouseExitSkillInventoryPanel);
		skillInventoryScrollbar = skillPanel.Find("SkillInventoryRoot/Mask/Scrollbar").GetComponent<Scrollbar>();
		skillInventoryScrollbar.onValueChanged.AddListener(OnSkillInventoryScrollbarValueChanged);
		skillInventoryScrollRect = skillPanel.Find("SkillInventoryRoot").GetComponent<ScrollRect>();
	}

	public void Show()
	{
		skillPanel.gameObject.SetActive(value: true);
		parentUI.AddGuideTips(new List<string>(1) { "Code_9_4" });
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("打开技能面板");
		LoadSkillInventory();
		LoadSkillEquiped();
		if (allShowingInventorySkillSlots.Count <= 8)
		{
			skillLeftMoveBtn.gameObject.SetActive(value: false);
			skillRightMoveBtn.gameObject.SetActive(value: false);
		}
		else
		{
			skillLeftMoveBtn.gameObject.SetActive(value: false);
			skillRightMoveBtn.gameObject.SetActive(value: true);
		}
		StartCoroutine(OpenSkillPanel_IE());
		parentUI.uiAnim.StartSkillPanel(allShowingInventorySkillSlots);
		EventManager.BroadcastEvent(EventEnum.E_OnOpenBagSkillPanel, null);
	}

	public void Hide()
	{
		RecycleAllSkillSlots();
		dragSkillObj.SetActive(value: false);
		if (currentChoosenSkillSlot != null)
		{
			currentChoosenSkillSlot = null;
		}
		skillPanel.gameObject.SetActive(value: false);
	}

	private IEnumerator OpenSkillPanel_IE()
	{
		yield return null;
		skillInventoryScrollbar.value = 0f;
	}

	private void OnMouseEnterSkillInventoryPanel()
	{
		isMouseOnSkillInventoryPanel = true;
	}

	private void OnMouseExitSkillInventoryPanel()
	{
		isMouseOnSkillInventoryPanel = false;
	}

	private void OnSkillInventoryScrollbarValueChanged(float value)
	{
		if (!1f.Equals(value))
		{
			if (0f.Equals(value))
			{
				skillLeftMoveBtn.gameObject.SetActive(value: false);
				return;
			}
			skillRightMoveBtn.gameObject.SetActive(value: true);
			skillLeftMoveBtn.gameObject.SetActive(value: true);
		}
		else
		{
			skillRightMoveBtn.gameObject.SetActive(value: false);
		}
	}

	private void OnClickSkillInventoryMoveLeft()
	{
		if (skillInventoryScrollbar.value != 1f)
		{
			if (skillInventoryScrollbarTween != null && skillInventoryScrollbarTween.IsActive())
			{
				skillInventoryScrollbarTween.Complete();
			}
			float endValue = Mathf.Clamp01(skillInventoryScrollbar.value + 0.3f);
			float startValue = skillInventoryScrollbar.value;
			skillInventoryScrollbarTween = DOTween.To(() => startValue, delegate(float x)
			{
				startValue = x;
			}, endValue, 0.3f).OnUpdate(delegate
			{
				skillInventoryScrollbar.value = startValue;
			});
		}
	}

	private void OnClickSkillInventoryMoveRight()
	{
		if (skillInventoryScrollbar.value != 0f)
		{
			if (skillInventoryScrollbarTween != null && skillInventoryScrollbarTween.IsActive())
			{
				skillInventoryScrollbarTween.Complete();
			}
			float endValue = Mathf.Clamp01(skillInventoryScrollbar.value - 0.3f);
			float startValue = skillInventoryScrollbar.value;
			skillInventoryScrollbarTween = DOTween.To(() => startValue, delegate(float x)
			{
				startValue = x;
			}, endValue, 0.3f).OnUpdate(delegate
			{
				skillInventoryScrollbar.value = startValue;
			});
		}
	}

	private void LoadSkillInventory()
	{
		List<string> allSkills = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSkills;
		HashSet<string> allNewSkills = Singleton<GameManager>.Instance.Player.PlayerInventory.AllNewSkills;
		for (int i = 0; i < allSkills.Count; i++)
		{
			SkillSlotCtrl skillSlot = GetSkillSlot();
			skillSlot.transform.SetParent(skillInventoryRoot);
			skillSlot.LoadSkill(allSkills[i], this, isEquiped: false, allNewSkills.Contains(allSkills[i]), skillInventoryScrollRect);
			allShowingInventorySkillSlots.Add(allSkills[i], skillSlot);
		}
	}

	private void LoadSkillEquiped()
	{
		List<string> currentSkillList = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.CurrentSkillList;
		int count = currentSkillList.Count;
		for (int i = 0; i < currentSkillList.Count; i++)
		{
			SkillSlotCtrl skillSlotCtrl = allEquipedSkillSlots[i];
			skillSlotCtrl.LoadSkill(currentSkillList[i], this, isEquiped: true, isNew: false, null);
			allShowingEquipedSkillSlots.Add(i, skillSlotCtrl);
		}
		for (int j = count; j < allEquipedSkillSlots.Length; j++)
		{
			allEquipedSkillSlots[j].LockSkillSlot(this);
		}
	}

	private SkillSlotCtrl GetSkillSlot()
	{
		if (skillSlotPool.Count > 0)
		{
			SkillSlotCtrl skillSlotCtrl = skillSlotPool.Dequeue();
			skillSlotCtrl.gameObject.SetActive(value: true);
			return skillSlotCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SkillSlot", "Prefabs", equipedSkillRoot).GetComponent<SkillSlotCtrl>();
	}

	private void RecycleAllSkillSlots()
	{
		foreach (KeyValuePair<string, SkillSlotCtrl> allShowingInventorySkillSlot in allShowingInventorySkillSlots)
		{
			allShowingInventorySkillSlot.Value.gameObject.SetActive(value: false);
			skillSlotPool.Enqueue(allShowingInventorySkillSlot.Value);
		}
		allShowingInventorySkillSlots.Clear();
		allShowingEquipedSkillSlots.Clear();
	}

	public bool TryReleaseSkill(int index)
	{
		if (!isMouseOnSkillInventoryPanel)
		{
			return false;
		}
		string text = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.CurrentSkillList[index];
		if (text.IsNullOrEmpty())
		{
			return false;
		}
		SkillSlotCtrl skillSlotCtrl = allShowingEquipedSkillSlots[index];
		if (currentChoosenSkillIndex >= 0)
		{
			currentChoosenSkillIndex = -1;
		}
		skillSlotCtrl.ResetSkillSlot();
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseSkill(text);
		SkillSlotCtrl skillSlot = GetSkillSlot();
		skillSlot.transform.SetParent(skillInventoryRoot);
		skillSlot.transform.SetAsLastSibling();
		skillSlot.LoadSkill(text, this, isEquiped: false, isNew: false, skillInventoryScrollRect);
		allShowingInventorySkillSlots.Add(text, skillSlot);
		Singleton<GameManager>.Instance.Player.PlayerInventory.ReleaseSkill(text);
		parentUI.SetChanged(isChanged: true);
		return true;
	}

	public bool TrySwapSkill(SkillSlotCtrl slotDrag, SkillSlotCtrl slotNowIn)
	{
		if (slotDrag.IsEquiped == slotNowIn.IsEquiped)
		{
			string currentSkillCode = slotDrag.CurrentSkillCode;
			string currentSkillCode2 = slotNowIn.CurrentSkillCode;
			slotDrag.LoadSkill(currentSkillCode2, this, slotNowIn.IsEquiped, isNew: false, slotDrag.IsEquiped ? null : skillInventoryScrollRect);
			slotNowIn.LoadSkill(currentSkillCode, this, slotDrag.IsEquiped, isNew: false, slotDrag.IsEquiped ? null : skillInventoryScrollRect);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SwapSkill(slotDrag.Index, slotNowIn.Index);
			if (!slotDrag.IsEquiped)
			{
				allShowingInventorySkillSlots[currentSkillCode] = slotNowIn;
				allShowingInventorySkillSlots[currentSkillCode2] = slotDrag;
			}
			return true;
		}
		string currentSkillCode3 = slotDrag.CurrentSkillCode;
		string currentSkillCode4 = slotNowIn.CurrentSkillCode;
		if (slotDrag.IsEquiped)
		{
			slotNowIn.LoadSkill(currentSkillCode3, this, isEquiped: false, isNew: false, skillInventoryScrollRect);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseSkill(currentSkillCode3);
			Singleton<GameManager>.Instance.Player.PlayerInventory.ReleaseSkill(currentSkillCode3);
			allShowingInventorySkillSlots.Add(currentSkillCode3, slotNowIn);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipSkill(currentSkillCode4, slotDrag.Index);
			Singleton<GameManager>.Instance.Player.PlayerInventory.EquipSkill(currentSkillCode4);
			allShowingInventorySkillSlots.Remove(currentSkillCode4);
			slotDrag.LoadSkill(currentSkillCode4, this, isEquiped: true, isNew: false, null);
		}
		else
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipSkill(currentSkillCode3, slotNowIn.Index);
			Singleton<GameManager>.Instance.Player.PlayerInventory.EquipSkill(currentSkillCode3);
			slotNowIn.LoadSkill(currentSkillCode3, this, isEquiped: true, isNew: false, null);
			allShowingInventorySkillSlots.Remove(currentSkillCode3);
			if (currentSkillCode4.IsNullOrEmpty())
			{
				slotDrag.gameObject.SetActive(value: false);
				skillSlotPool.Enqueue(slotDrag);
			}
			else
			{
				slotDrag.LoadSkill(currentSkillCode4, this, isEquiped: false, isNew: false, skillInventoryScrollRect);
				allShowingInventorySkillSlots.Add(currentSkillCode4, slotDrag);
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseSkill(currentSkillCode4);
				Singleton<GameManager>.Instance.Player.PlayerInventory.ReleaseSkill(currentSkillCode4);
			}
		}
		CancelNewSkillSlot(slotDrag);
		CancelNewSkillSlot(slotNowIn);
		parentUI.SetChanged(isChanged: true);
		return true;
	}

	public void CancelNewSkillSlot(SkillSlotCtrl slotCtrl)
	{
		if (!Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewSkill(slotCtrl.CurrentSkillCode))
		{
			parentUI.CancelBagNewSkillImg();
		}
	}

	private void InitDragSkill(GameObject dragSkillObj, Image dragSkillIconImg)
	{
		this.dragSkillObj = dragSkillObj;
		this.dragSkillIconImg = dragSkillIconImg;
	}

	public GameObject StartDragSkill(string cardCode)
	{
		IsDragingSkillItem = true;
		dragSkillObj.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("魔法拖动");
		SkillCard skillCard = FactoryManager.GetSkillCard(Singleton<GameManager>.Instance.Player.PlayerOccupation, cardCode);
		dragSkillIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCard.SkillCardAttr.IllustrationName, "Sprites/SkillIcon");
		return dragSkillObj;
	}

	public void SetCurrentPointInSlot(SkillSlotCtrl slot)
	{
		currentPointInSlot = slot;
	}

	public void EndDragSkill()
	{
		IsDragingSkillItem = false;
		dragSkillObj.SetActive(value: false);
	}
}
