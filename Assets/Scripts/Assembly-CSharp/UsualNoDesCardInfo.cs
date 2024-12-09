using UnityEngine;
using UnityEngine.UI;

public class UsualNoDesCardInfo : MonoBehaviour
{
	private Text nameText;

	private Text apCostText;

	private ImageExtent cardImg;

	private void Awake()
	{
		cardImg = GetComponent<ImageExtent>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		apCostText = base.transform.Find("ApCost").GetComponent<Text>();
		cardImg.material = Object.Instantiate(cardImg.material);
	}

	public void LoadCard(string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		nameText.text = usualCardAttr.NameKey.LocalizeText();
		apCostText.text = usualCardAttr.ApCost.ToString();
		cardImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(usualCardAttr.IllustrationName, "Sprites/Cards");
	}

	public void LoadCard(Sprite cardSprite, string nameStr, string apStr)
	{
		cardImg.sprite = cardSprite;
		nameText.text = nameStr;
		apCostText.text = apStr;
	}

	public void SetCardUnusable()
	{
		cardImg.toggleTint = true;
		cardImg.SetMaterialDirty();
	}

	public void SetCardUsable()
	{
		cardImg.toggleTint = false;
		cardImg.SetMaterialDirty();
	}

	public void FreshCard()
	{
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: true);
	}
}
