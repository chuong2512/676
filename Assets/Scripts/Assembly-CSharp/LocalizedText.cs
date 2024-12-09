using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	public string key;

	private Text text;

	private void Start()
	{
		text = GetComponent<Text>();
		Initalization();
	}

	public void Initalization()
	{
		text.text = key.LocalizeText();
	}

	public void changeText()
	{
		text.text = key.LocalizeText();
	}
}
