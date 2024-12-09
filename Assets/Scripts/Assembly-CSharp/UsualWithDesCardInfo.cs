using UnityEngine;
using UnityEngine.UI;

public class UsualWithDesCardInfo : MonoBehaviour
{
	private const string AssetPath = "Sprites/Cards";

	private Text nameText;

	private Text desText;

	private Text apCostText;

	private ImageExtent cardImg;

	private int nameFontSize;

	private int fontSize;

	private float lineSpace;

	private void Awake()
	{
		cardImg = GetComponent<ImageExtent>();
		cardImg.material = Object.Instantiate(cardImg.material);
		nameText = base.transform.Find("Name").GetComponent<Text>();
		desText = base.transform.Find("Description").GetComponent<Text>();
		fontSize = desText.fontSize;
		lineSpace = desText.lineSpacing;
		if (SingletonDontDestroy<SettingManager>.Instance.Language == 0)
		{
			nameText.fontSize = Mathf.RoundToInt((float)nameFontSize * 1.1f);
			desText.alignment = TextAnchor.UpperLeft;
			desText.lineSpacing = lineSpace * 1.2f;
			desText.fontSize = Mathf.RoundToInt((float)fontSize * 1.1f);
		}
		else
		{
			desText.alignment = TextAnchor.UpperCenter;
			desText.lineSpacing = lineSpace;
			desText.fontSize = fontSize;
		}
		apCostText = base.transform.Find("ApCost").GetComponent<Text>();
	}

	public void LoadCard(string cardCode)
	{
		UsualCard usualCard = FactoryManager.GetUsualCard(cardCode);
		nameText.text = usualCard.CardName;
		desText.text = usualCard.CardNormalDes;
		apCostText.text = usualCard.ApCost.ToString();
		cardImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(usualCard.IllustrationName, "Sprites/Cards");
	}

	public void SetImageColorTint(bool isTrue)
	{
		cardImg.toggleTint = isTrue;
		cardImg.SetMaterialDirty();
	}
}
