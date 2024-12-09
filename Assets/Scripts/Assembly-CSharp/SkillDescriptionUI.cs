using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescriptionUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private SkillDesPanelCtrl _skillDesPanelCtrl;

	private Transform centerRoot;

	private Image maskBg;

	public override string UIViewName => "SkillDescriptionUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Skill Description UI...");
	}

	public override void OnSpawnUI()
	{
		InitSkillDescription();
	}

	private void InitSkillDescription()
	{
		_skillDesPanelCtrl = base.transform.Find("Root/SkillDesPanel").GetComponent<SkillDesPanelCtrl>();
		centerRoot = base.transform.Find("Root/CenterRoot");
		maskBg = GetComponent<Image>();
	}

	public void ShowSkillDescription(string skillCode, Vector3 pos, bool isOnBattle)
	{
		maskBg.enabled = false;
		_skillDesPanelCtrl.transform.position = pos;
		_skillDesPanelCtrl.ShowDescription(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode, isOnBattle);
	}

	public void ShowSkillDescription(PlayerOccupation playerOccupation, string skillCode, bool isOnBattle, string btnName, Action callback, bool isInteractive)
	{
		_skillDesPanelCtrl.ShowDescription(playerOccupation, skillCode, isOnBattle, btnName, callback, isInteractive);
		maskBg.enabled = true;
		_skillDesPanelCtrl.transform.position = centerRoot.position;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用_关闭按钮");
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}
}
