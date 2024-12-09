using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideTipsBtn : MonoBehaviour
{
	private List<string> allGuideTips;

	private Image outlineImg;

	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
		outlineImg = base.transform.Find("Outline").GetComponent<Image>();
	}

	public void ActiveGuideTips()
	{
		base.gameObject.SetActive(value: true);
	}

	public void AddGuideTips(List<string> guideTips)
	{
		allGuideTips = guideTips;
		outlineImg.gameObject.SetActive(value: true);
	}

	private void OnClick()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("提示按钮_点击");
		outlineImg.gameObject.SetActive(value: false);
		SingletonDontDestroy<UIManager>.Instance.ShowView("GuideTipsUI", base.transform, allGuideTips, null);
	}
}
