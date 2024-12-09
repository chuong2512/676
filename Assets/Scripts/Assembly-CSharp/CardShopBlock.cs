using UnityEngine;

public class CardShopBlock : BaseBlock
{
	public override string HandleLoadActionName => "HandleLoad_CardShopBlock";

	public void ActiveCardShopBlock(Vector2Int pos, int roomSeed)
	{
		base.BlockPosition = pos;
		base.RoomSeed = roomSeed;
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("CardShopUI") as CardShopUI).ShowShop(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer, base.RoomSeed);
		}
	}

	public override void ResetBlock()
	{
	}
}
