public class CardHoverHint : HoverWithKeysHint
{
	private UsualWithDesCardInfo _cardInfo;

	protected override void OnAwake()
	{
		base.OnAwake();
		_cardInfo = base.transform.Find("UsualWithDesCard").GetComponent<UsualWithDesCardInfo>();
	}

	protected override void InitKey()
	{
		keyRoot = base.transform.Find("KeyRoot");
	}

	public void SetCard(UsualCardAttr cardAttr)
	{
		_cardInfo.LoadCard(cardAttr.CardCode);
		AddKeys(cardAttr.AllKeys);
	}
}
