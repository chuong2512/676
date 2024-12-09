using UnityEngine;
using UnityEngine.UI;

public class OccupationSingleSign : MonoBehaviour
{
	public PlayerOccupation PlayerOccupation;

	public bool isParent;

	private Image occupationImg;

	private Text nameText;

	private Button btn;

	private void Awake()
	{
		occupationImg = GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		btn = GetComponent<Button>();
		if (!isParent)
		{
			btn.onClick.AddListener(OnClick);
		}
	}

	public void SetOccupation(Sprite sprite, string nameStr)
	{
		occupationImg.sprite = sprite;
		nameText.text = nameStr;
	}

	public void SetChosen()
	{
		btn.interactable = false;
	}

	public void SetNotChosen()
	{
		btn.interactable = true;
	}

	private void OnClick()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("切换角色母按钮");
		(SingletonDontDestroy<UIManager>.Instance.GetView("CardPresuppositionUI") as CardPresuppositionUI).SwitchToOtherOccupation(PlayerOccupation);
	}
}
