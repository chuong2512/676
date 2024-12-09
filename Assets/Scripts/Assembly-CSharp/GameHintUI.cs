using UnityEngine;
using UnityEngine.UI;

public class GameHintUI : UIView
{
	private Text contentText;

	private CanvasGroup hintCanvasGroup;

	private float counter;

	private bool isShowing;

	public override string UIViewName => "GameHintUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnSpawnUI()
	{
		contentText = base.transform.Find("Root/Mask/Content").GetComponent<Text>();
		hintCanvasGroup = GetComponent<CanvasGroup>();
		hintCanvasGroup.alpha = 0f;
		isShowing = false;
		counter = 0f;
	}

	private void Update()
	{
		if (isShowing)
		{
			counter += Time.deltaTime;
			if (counter >= 5f)
			{
				hintCanvasGroup.alpha = 0f;
				isShowing = false;
			}
		}
	}

	public void AddHint(string content)
	{
		isShowing = true;
		hintCanvasGroup.alpha = 1f;
		counter = 0f;
		contentText.text += "\n";
		contentText.text += content;
	}

	public override void OnDestroyUI()
	{
	}
}
