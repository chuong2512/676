using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoomBlock : BaseBlock
{
	private class HandlerArgs
	{
		public bool isClick;
	}

	private const int MoveDownDistance = -25;

	private const string RoomBlockImageFormat = "地块样图{0}";

	public const string EmptyBlockName = "空白格2";

	private static Dictionary<string, MethodInfo> allBlockProcessMethodInfos = new Dictionary<string, MethodInfo>();

	private Image roomImg;

	private bool isActive;

	[HideInInspector]
	public bool IsEverInteracted;

	private RoomUI _roomUi;

	private const string RoomBlockLoadActionName = "";

	private const string EmptyBlockLoadActionName = "HandleLoad_EmptyBlock";

	private string RoomBlockHandleLoadActionName;

	public int RoomIndex { get; private set; }

	public override string HandleLoadActionName => RoomBlockHandleLoadActionName;

	protected override void OnAwake()
	{
		base.OnAwake();
		roomImg = GetComponent<Image>();
	}

	protected override void OnClick()
	{
		if (!isActive || IsEverInteracted || RoomUI.IsAnyBlockInteractiong)
		{
			return;
		}
		isActive = false;
		Vector3 vfxPos = base.transform.position;
		RoomUI.IsAnyBlockInteractiong = true;
		base.transform.DOLocalMoveY(-25f, 0.1f).OnComplete(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound(SoundStatic.ClickRoomBlockSoundNames[Random.Range(0, SoundStatic.ClickRoomBlockSoundNames.Length)]);
			roomImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("空白格2", "Sprites/RoomUI");
			_roomUi.PlayRoomBlockBrokenAnim(vfxPos, delegate
			{
				StartBlock(isClick: true);
			});
		}).SetEase(Ease.OutQuint);
	}

	public void LoadRoomBlock(RoomUI roomUi, int roomIndex, int roomSeed, Vector2Int pos)
	{
		isActive = false;
		IsEverInteracted = false;
		base.RoomSeed = roomSeed;
		_roomUi = roomUi;
		RoomIndex = roomIndex;
		base.BlockPosition = pos;
		SetRoomBlock();
		SetSprite(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite($"地块样图{Random.Range(1, 11)}", "Sprites/RoomUI"));
	}

	private void SetRoomBlock()
	{
		RoomBlockHandleLoadActionName = "";
		roomImg.material = null;
	}

	public void SetEmptyBlock()
	{
		IsEverInteracted = true;
		RoomBlockHandleLoadActionName = "HandleLoad_EmptyBlock";
		roomImg.material = null;
	}

	public void SetSprite(Sprite sprite)
	{
		roomImg.sprite = sprite;
	}

	public override void ResetBlock()
	{
	}

	public void StartBlock(bool isClick)
	{
		string key = $"RoomBlockHandler_{RoomIndex}";
		if (!allBlockProcessMethodInfos.TryGetValue(key, out var value))
		{
			value = typeof(RoomBlock).GetMethod(key, BindingFlags.Static | BindingFlags.NonPublic);
			allBlockProcessMethodInfos.Add(key, value);
		}
		value.Invoke(null, new object[2]
		{
			this,
			new HandlerArgs
			{
				isClick = isClick
			}
		});
	}

	public void ActiveBlock()
	{
		if (!IsEverInteracted)
		{
			isActive = true;
			roomImg.material = SingletonDontDestroy<ResourceManager>.Instance.LoadMaterial("RoomBlock_Mat", "Materials");
		}
	}

	public void ActiveRoundBlock()
	{
		_roomUi.ActiveRoundRoomBlock(base.BlockPosition);
	}

	private static void RoomBlockHandler_0(RoomBlock block, HandlerArgs args)
	{
		block.SetSprite(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("空白格2", "Sprites/RoomUI"));
		block.IsEverInteracted = true;
		block.transform.localPosition = new Vector3(block.BlockPosition.x * 175, 0f, 0f);
		block._roomUi.AddEmptyBlock(block.BlockPosition, isSetSprite: false);
	}

	private static void RoomBlockHandler_1(RoomBlock block, HandlerArgs args)
	{
		block._roomUi.AddObstacleBlockToMap(block.BlockPosition);
		block.IsEverInteracted = true;
	}

	private static void RoomBlockHandler_2(RoomBlock block, HandlerArgs args)
	{
		block.SetSprite(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("空白格2", "Sprites/RoomUI"));
		block.IsEverInteracted = true;
		block.ActiveRoundBlock();
		block.transform.localPosition = new Vector3(block.BlockPosition.x * 175, 0f, 0f);
		block._roomUi.AddEmptyBlock(block.BlockPosition, isSetSprite: false);
	}

	private static void RoomBlockHandler_3(RoomBlock block, HandlerArgs args)
	{
		block.IsEverInteracted = true;
		if (Singleton<GameManager>.Instance.CurrentMapLevel != 4 && Singleton<GameManager>.Instance.CurrentMapLayer == 1)
		{
			block._roomUi.AddNormalNextRoomBlock(block.BlockPosition);
		}
		else
		{
			block._roomUi.AddBossNextRoomBlock(block.BlockPosition);
		}
	}

	private static void RoomBlockHandler_4(RoomBlock block, HandlerArgs args)
	{
		bool isClick = args.isClick;
		block.IsEverInteracted = true;
		block._roomUi.AddEliteMonsterBlock(block.BlockPosition, isClick);
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).HideEnemyHint(block.BlockPosition);
	}

	private static void RoomBlockHandler_5(RoomBlock block, HandlerArgs args)
	{
		bool isClick = args.isClick;
		block.IsEverInteracted = true;
		block._roomUi.AddNormalMonsterBlock(block.BlockPosition, isClick);
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).HideEnemyHint(block.BlockPosition);
	}

	private static void RoomBlockHandler_6(RoomBlock block, HandlerArgs args)
	{
		block._roomUi.AddEventBlock(block.BlockPosition, args.isClick);
	}

	private static void RoomBlockHandler_7(RoomBlock block, HandlerArgs args)
	{
		if (Singleton<GameManager>.Instance.CurrentMapLayer % 2 == 1)
		{
			block._roomUi.AddEquipShopBlock(block.BlockPosition);
		}
		else
		{
			block._roomUi.AddCardShopBlock(block.BlockPosition);
		}
	}

	private static void RoomBlockHandler_8(RoomBlock block, HandlerArgs args)
	{
	}

	private static void RoomBlockHandler_9(RoomBlock block, HandlerArgs args)
	{
		block._roomUi.AddBossBlock(block.BlockPosition);
	}

	private static void RoomBlockHandler_10(RoomBlock block, HandlerArgs args)
	{
		block._roomUi.AddRoomPlotBlock(block.BlockPosition);
	}

	private static void RoomBlockHandler_11(RoomBlock block, HandlerArgs args)
	{
		block._roomUi.AddHiddenBossBlock(block.BlockPosition);
	}
}
