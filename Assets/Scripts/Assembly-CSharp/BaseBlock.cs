using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseBlock : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	protected const float WaitBlockActivenTime = 0.8f;

	public const string RoomSpriteAssetPath = "Sprites/RoomUI";

	public Vector2Int BlockPosition { get; protected set; }

	public int RoomSeed { get; protected set; }

	public abstract string HandleLoadActionName { get; }

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			OnClick();
		}
	}

	protected abstract void OnClick();

	public abstract void ResetBlock();
}
