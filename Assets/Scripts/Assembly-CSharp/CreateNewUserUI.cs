using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewUserUI : UIView
{
	private InputField inputField;

	private Button cancelBtn;

	private Button confirmBtn;

	private Text warningText;

	private int userIndex;

	private Action<int, string> callback;

	public override string UIViewName => "CreateNewUserUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowCreateNewUserUI((int)objs[0], (bool)objs[1], (Action<int, string>)objs[2], (string)objs[3]);
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
		confirmBtn = base.transform.Find("Mask/ConfirmBtn").GetComponent<Button>();
		confirmBtn.onClick.AddListener(OnClickConfirmBtn);
		cancelBtn = base.transform.Find("Mask/CancelBtn").GetComponent<Button>();
		cancelBtn.onClick.AddListener(OnClickCloseBtn);
		warningText = base.transform.Find("Mask/WarningHint").GetComponent<Text>();
	}

	private void ShowCreateNewUserUI(int index, bool isMustCreate, Action<int, string> callback, string initName)
	{
		if (callback == null)
		{
			throw new NullReferenceException("callback cannot be null");
		}
		this.callback = callback;
		warningText.text = string.Empty;
		userIndex = index;
		inputField.text = initName;
		if (isMustCreate)
		{
			cancelBtn.gameObject.SetActive(value: false);
			Vector3 localPosition = confirmBtn.transform.localPosition;
			localPosition.x = 0f;
			confirmBtn.transform.localPosition = localPosition;
		}
		else
		{
			cancelBtn.gameObject.SetActive(value: true);
			Vector3 localPosition2 = confirmBtn.transform.localPosition;
			localPosition2.x = 0f - cancelBtn.transform.localPosition.x;
			confirmBtn.transform.localPosition = localPosition2;
		}
	}

	private void OnClickConfirmBtn()
	{
		string text = inputField.text;
		string text2 = AppData.CheckNewName(text);
		if (!text2.IsNullOrEmpty())
		{
			warningText.text = text2;
			return;
		}
		callback?.Invoke(userIndex, text);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void OnClickCloseBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
