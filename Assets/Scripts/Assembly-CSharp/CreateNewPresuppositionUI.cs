using System;
using UnityEngine.UI;

public class CreateNewPresuppositionUI : UIView
{
	private InputField inputField;

	private Text warningText;

	private Func<string, string> presuppositionCheckFunc;

	private Action<string> confirmAction;

	public override string UIViewName => "CreateNewPresuppositionUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		inputField.text = (string)objs[0];
		inputField.ActivateInputField();
		warningText.text = string.Empty;
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
		inputField = base.transform.Find("Mask/InputField").GetComponent<InputField>();
		warningText = base.transform.Find("Mask/WarningHint").GetComponent<Text>();
		base.transform.Find("Mask/ConfirmBtn").GetComponent<Button>().onClick.AddListener(OnClickConfirmBtn);
		base.transform.Find("Mask/CancelBtn").GetComponent<Button>().onClick.AddListener(OnClickCancelBtn);
	}

	public void ShowCreateNewPresuppositionPanel(Func<string, string> checkFunc, Action<string> confirmAction)
	{
		presuppositionCheckFunc = checkFunc;
		this.confirmAction = confirmAction;
	}

	private void OnClickCancelBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void OnClickConfirmBtn()
	{
		string text = presuppositionCheckFunc(inputField.text);
		if (!text.IsNullOrEmpty())
		{
			warningText.text = text;
			return;
		}
		confirmAction(inputField.text);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
