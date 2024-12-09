using UnityEngine;

public class GamblingEventBlock : SpecialEventBlock
{
	public override void ActiveEventBlock(Vector2Int pos, int roomSeed, BaseGameEvent gameEvent, bool isAutoActive)
	{
		base.roomSeed = roomSeed;
		base.GameEvent = gameEvent;
		base.BlockPosition = pos;
	}
}
