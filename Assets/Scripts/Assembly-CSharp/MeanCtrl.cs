using UnityEngine;
using UnityEngine.UI;

public class MeanCtrl : MonoBehaviour
{
	private Image meanIconImg;

	private Text tmpContentText;

	private Text contentText;

	private void Awake()
	{
		meanIconImg = base.transform.Find("Bg/MeanIcon").GetComponent<Image>();
		tmpContentText = GetComponent<Text>();
		contentText = base.transform.Find("Bg/Content").GetComponent<Text>();
	}

	public void LoadMean(Sprite meanIcon, string discription)
	{
		meanIconImg.sprite = meanIcon;
		string text3 = (tmpContentText.text = (contentText.text = discription));
	}
}
