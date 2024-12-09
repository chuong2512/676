using UnityEngine;
using UnityEngine.UI;

public class HelpContentLineCtrl : MonoBehaviour
{
	private Text titleText;

	private void Awake()
	{
		titleText = base.transform.Find("Title").GetComponent<Text>();
	}

	public void SetTitle(string title)
	{
		titleText.text = title;
	}
}
