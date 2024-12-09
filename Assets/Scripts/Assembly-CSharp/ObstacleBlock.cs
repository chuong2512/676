using UnityEngine;
using UnityEngine.UI;

public class ObstacleBlock : BaseBlock
{
	private Image blockImg;

	public override string HandleLoadActionName => "HandleLoad_ObstacleBlock";

	protected override void OnAwake()
	{
		base.OnAwake();
		blockImg = base.transform.Find("Block").GetComponent<Image>();
	}

	public void AcitveObstacleBlock(Vector2Int pos, int roomSeed)
	{
		base.RoomSeed = roomSeed;
		base.BlockPosition = pos;
		blockImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite($"障碍格{Random.Range(1, 5)}", "Sprites/RoomUI");
	}

	protected override void OnClick()
	{
	}

	public override void ResetBlock()
	{
	}
}
