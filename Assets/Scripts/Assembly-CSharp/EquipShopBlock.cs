using UnityEngine;

public class EquipShopBlock : BaseBlock
{
	public override string HandleLoadActionName => "HandleLoad_EquipShopBlock";

	public void ActiveEquipShopBlock(Vector2Int pos, int roomSeed)
	{
		base.BlockPosition = pos;
		base.RoomSeed = roomSeed;
	}

	protected override void OnClick()
	{
		if (!RoomUI.IsAnyBlockInteractiong)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("ShopUI") as ShopUI).ShowShop(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.CurrentMapLayer, base.RoomSeed);
		}
	}

	public override void ResetBlock()
	{
	}
}
