using UnityEngine;
using UnityEngine.UI;

public class SingleHelpContentCtrl : MonoBehaviour
{
	private Text titleText;

	private Image illuImg;

	private Text contentText;

	private void Awake()
	{
		titleText = base.transform.Find("Title").GetComponent<Text>();
		illuImg = base.transform.Find("IlluBottom/IlluImg").GetComponent<Image>();
		contentText = base.transform.Find("Content").GetComponent<Text>();
	}

	public void LoadContent(string guideCode)
	{
		GuideTipData guideTipData = DataManager.Instance.GetGuideTipData(guideCode);
		titleText.text = guideTipData.TitleKey.LocalizeText();
		contentText.text = guideTipData.ContentKey.LocalizeText();
		illuImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? guideTipData.IllustrationName : guideTipData.IllustrationENName, "Sprites/GuideTips");
	}
}
