using UnityEngine;
using UnityEngine.UI;

public class KnightBlockCtrl : PlayerDefenceAttrCtrl
{
	private static readonly Color DeactiveColor = "A1A098FF".HexColorToColor();

	private static readonly Color DeactiveOutlineColor = "818479FF".HexColorToColor();

	private static readonly Color ActiveColor = Color.white;

	private static readonly Color ActiveOutlineColor = Color.black;

	public Sprite NotDefenceSprite;

	public Sprite DefenceSprite;

	public Sprite DefenceStableSprite;

	private Text blockAmountText;

	private Outline blockAmountTextOutline;

	private Image iconImg;

	private int baseBlockValue;

	private int stableBlockValue;

	public override PlayerOccupation PlayerOccupation => PlayerOccupation.Knight;

	private void Awake()
	{
		blockAmountText = base.transform.Find("Amount").GetComponent<Text>();
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		blockAmountTextOutline = blockAmountText.GetComponent<Outline>();
	}

	private void OnDisable()
	{
		EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
		EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnRemoveBuff);
	}

	public override void LoadPlayerInfo()
	{
		stableBlockValue = 0;
		baseBlockValue = Singleton<GameManager>.Instance.Player.PlayerAttr.DefenceAttr;
		blockAmountText.text = baseBlockValue.ToString();
		blockAmountText.color = DeactiveColor;
		blockAmountTextOutline.effectColor = DeactiveOutlineColor;
		EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
		EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnRemoveBuff);
	}

	private void OnGetNewBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null)
		{
			if (buffEventData.buffType == BuffType.Buff_Defence)
			{
				blockAmountText.color = ActiveColor;
				blockAmountTextOutline.effectColor = ActiveOutlineColor;
				iconImg.sprite = DefenceSprite;
			}
			else if (buffEventData.buffType == BuffType.Buff_Stable)
			{
				iconImg.sprite = DefenceStableSprite;
			}
		}
	}

	private void OnRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null)
		{
			if (buffEventData.buffType == BuffType.Buff_Defence)
			{
				iconImg.sprite = NotDefenceSprite;
				blockAmountText.color = DeactiveColor;
				blockAmountTextOutline.effectColor = DeactiveOutlineColor;
			}
			else if (buffEventData.buffType == BuffType.Buff_Stable)
			{
				iconImg.sprite = DefenceSprite;
			}
		}
	}

	public void UpdatePlayerBaseBlock(int blockValue)
	{
		baseBlockValue = blockValue;
		UpdateBlockValue();
	}

	public void UpdatePlayerStableBlock(int stableBlockValue)
	{
		this.stableBlockValue = stableBlockValue;
		UpdateBlockValue();
	}

	private void UpdateBlockValue()
	{
		if (stableBlockValue > 0)
		{
			blockAmountText.text = baseBlockValue + "+<size=20>" + stableBlockValue + "</size>";
		}
		else
		{
			blockAmountText.text = baseBlockValue.ToString();
		}
	}
}
