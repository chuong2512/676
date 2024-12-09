using UnityEngine;

public class InputManaeger : Singleton<InputManaeger>
{
	public bool IsClick(out Vector3 position)
	{
		if (Input.GetMouseButtonUp(0))
		{
			position = Input.mousePosition;
			return true;
		}
		position = Vector3.zero;
		return false;
	}
}
