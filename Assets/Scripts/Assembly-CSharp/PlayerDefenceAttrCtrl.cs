using UnityEngine;

public abstract class PlayerDefenceAttrCtrl : MonoBehaviour
{
	public abstract PlayerOccupation PlayerOccupation { get; }

	public abstract void LoadPlayerInfo();
}
