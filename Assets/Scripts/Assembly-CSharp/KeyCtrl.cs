using UnityEngine;
using UnityEngine.UI;

public class KeyCtrl : MonoBehaviour
{
	private Text tmpContentText;

	private RectTransform textRect;

	private Text contentText;

	public float HalfSizeX => textRect.sizeDelta.x / 2f + 14f;

	public float HalfSizeY => textRect.sizeDelta.y / 2f + 12.5f;

	private void Awake()
	{
		tmpContentText = GetComponent<Text>();
		contentText = base.transform.Find("Bg/Content").GetComponent<Text>();
		textRect = GetComponent<RectTransform>();
	}

	public void LoadKey(string _name, string _des)
	{
		string text3 = (tmpContentText.text = (contentText.text = "<color=#f19b00ff><size=24>" + _name + "</size></color>：" + _des));
	}

	public void LoadKey(string _name, string _des, string colorStr)
	{
		string text3 = (tmpContentText.text = (contentText.text = "<" + colorStr + "><size=24>" + _name + "</size></color>：" + _des));
	}
}
