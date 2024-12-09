using UnityEngine.UI;

public class CardDescriptionHoverHint : HoverWithKeysHint
{
	private Text nameText;

	private Text tmpVisualTxt;

	private Text descriptionText;

	protected override void OnAwake()
	{
		base.OnAwake();
		nameText = base.transform.Find("NameBottom/TmpTxtBg/TitleBg/TitleTxt").GetComponent<Text>();
		tmpVisualTxt = base.transform.Find("NameBottom/TmpTxtBg").GetComponent<Text>();
		descriptionText = base.transform.Find("Description").GetComponent<Text>();
	}

	protected override void InitKey()
	{
		keyRoot = base.transform.Find("NameBottom/KeyRoot");
	}

	public void SetCardBaseInfo(string nameStr, string descriptionStr)
	{
		string text3 = (tmpVisualTxt.text = (nameText.text = nameStr));
		descriptionText.text = descriptionStr;
	}

	public void ForceRebuildLayoutImmediate()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}
}
