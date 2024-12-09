using UnityEngine;
using UnityEngine.UI;

public class GameReportUI : UIView
{
	private Text contentText;

	private Transform contentPanel;

	public override string UIViewName => "GameReportUI";

	public override string UILayerName => "OutGameLayer";

	public override bool AutoHideBySwitchScene => false;

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
		Debug.Log("Destory Game Report UI...");
	}

	public override void OnSpawnUI()
	{
		contentText = base.transform.Find("Bg/ContentBg/Mask/Content").GetComponent<Text>();
		base.transform.Find("Bg/OpenBtn").GetComponent<Button>().onClick.AddListener(OnClickOpenBtn);
		base.transform.Find("Bg/ContentBg/CloseBtn").GetComponent<Button>().onClick.AddListener(OnClickCloseBtn);
		contentPanel = base.transform.Find("Bg/ContentBg");
		contentText.text = string.Empty;
		base.transform.Find("Bg/ContentBg/ClearBtn").GetComponent<Button>().onClick.AddListener(ClearContent);
	}

	public void AddGameReportContent(string content)
	{
		if (contentText.text.Length >= 8000)
		{
			ClearContent();
		}
		Text text = contentText;
		text.text = text.text + "---" + content;
		contentText.text += "\n";
	}

	public void AddSystemReportContent(string content)
	{
		if (contentText.text.Length >= 8000)
		{
			ClearContent();
		}
		contentText.text += content;
		contentText.text += "\n";
	}

	public void AddGameReportContent(string tile, string content)
	{
		if (contentText.text.Length >= 8000)
		{
			ClearContent();
		}
		Text text = contentText;
		text.text = text.text + tile + " : " + content + " \n";
	}

	public void ClearContent()
	{
		contentText.text = string.Empty;
	}

	private void OnClickOpenBtn()
	{
		contentPanel.gameObject.SetActive(value: true);
	}

	private void OnClickCloseBtn()
	{
		contentPanel.gameObject.SetActive(value: false);
	}
}
