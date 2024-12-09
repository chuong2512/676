using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEffectDescriptionUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private Text contentText;

	private Text tmpContentText;

	private Image iconImg;

	private Text nameText;

	private Action callback;

	public override string UIViewName => "LevelEffectDescriptionUI";

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
	}

	public override void OnSpawnUI()
	{
		Transform transform = base.transform.Find("Root/EffectDesPanel");
		contentText = transform.Find("Content/Bg/Content").GetComponent<Text>();
		tmpContentText = transform.Find("Content").GetComponent<Text>();
		iconImg = transform.Find("Content/Bg/Icon").GetComponent<Image>();
		nameText = transform.Find("Content/Bg/NameText").GetComponent<Text>();
		transform.Find("Content/FunctionBtn").GetComponent<Button>().onClick.AddListener(OnClickChosenBtn);
	}

	public void ShowLevelUpEffect(string name, Sprite icon, string content, Action callback)
	{
		nameText.text = name;
		iconImg.sprite = icon;
		string text3 = (contentText.text = (tmpContentText.text = content));
		this.callback = callback;
	}

	private void OnClickChosenBtn()
	{
		callback?.Invoke();
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
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
