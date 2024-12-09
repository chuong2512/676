using System;
using UnityEngine;
using UnityEngine.UI;

public class SystemHintUI : UIView
{
	private Text content;

	private Action oneChosenConfirmCallback;

	private Transform currentShowPanel;

	private Transform oneChosenPanel;

	private Transform twoChosenPanel;

	private Action twoChosenConfirmCallback;

	private Action twoChosenCancleCallback;

	private Text btn1Text;

	private Text btn2Text;

	public override string UIViewName => "SystemHintUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		if (currentShowPanel != null)
		{
			currentShowPanel.gameObject.SetActive(value: false);
			currentShowPanel = null;
		}
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy System Hint UI...");
	}

	public override void OnSpawnUI()
	{
		Transform transform = base.transform.Find("Mask");
		content = transform.Find("Content").GetComponent<Text>();
		transform.Find("OneChosenPanel/ConfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickOneChosenConfirmBtn);
		transform.Find("TwoChosenPanel/ConfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickTwoChosenConfirmBtn);
		transform.Find("TwoChosenPanel/CancelBtn").GetComponent<Button>().onClick.AddListener(OnClickTwoChosenCancelBtn);
		oneChosenPanel = transform.Find("OneChosenPanel");
		twoChosenPanel = transform.Find("TwoChosenPanel");
		btn1Text = transform.Find("TwoChosenPanel/ConfirmBtn/Text").GetComponent<Text>();
		btn2Text = transform.Find("TwoChosenPanel/CancelBtn/Text").GetComponent<Text>();
	}

	public void ShowOneChosenSystemHint(string systemhint, Action callback)
	{
		content.text = systemhint;
		oneChosenConfirmCallback = callback;
		currentShowPanel = oneChosenPanel;
		oneChosenPanel.gameObject.SetActive(value: true);
	}

	private void OnClickOneChosenConfirmBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		oneChosenConfirmCallback?.Invoke();
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public void ShowTwoChosenSystemHint(string systemhint, Action confirmCallback, Action cancelCallback = null)
	{
		btn1Text.text = "Confirm".LocalizeText();
		btn2Text.text = "Cancel".LocalizeText();
		content.text = systemhint;
		twoChosenConfirmCallback = confirmCallback;
		twoChosenCancleCallback = cancelCallback;
		currentShowPanel = twoChosenPanel;
		twoChosenPanel.gameObject.SetActive(value: true);
	}

	public void ShowTwoChosenSystemHint(string systemhint, string btn1Str, string btn2Str, Action confirmCallback, Action cancelCallback = null)
	{
		btn1Text.text = btn1Str;
		btn2Text.text = btn2Str;
		content.text = systemhint;
		twoChosenConfirmCallback = confirmCallback;
		twoChosenCancleCallback = cancelCallback;
		currentShowPanel = twoChosenPanel;
		twoChosenPanel.gameObject.SetActive(value: true);
	}

	private void OnClickTwoChosenConfirmBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		twoChosenConfirmCallback?.Invoke();
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void OnClickTwoChosenCancelBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		twoChosenCancleCallback?.Invoke();
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
