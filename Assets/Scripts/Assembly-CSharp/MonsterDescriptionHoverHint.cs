using UnityEngine.UI;

public class MonsterDescriptionHoverHint : BaseHoverHint
{
	private Text nameText;

	private Text descriptionContentText;

	private Text tmpVisualTxt;

	protected override void OnAwake()
	{
		nameText = base.transform.Find("NameBottom/TmpTxtBg/TitleBg/TitleTxt").GetComponent<Text>();
		tmpVisualTxt = base.transform.Find("NameBottom/TmpTxtBg").GetComponent<Text>();
		descriptionContentText = base.transform.Find("Description").GetComponent<Text>();
	}

	public void LoadMonsterDescription(string nameStr, string contentStr)
	{
		string text3 = (tmpVisualTxt.text = (nameText.text = nameStr));
		descriptionContentText.text = contentStr;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.m_RectTransform);
	}
}
