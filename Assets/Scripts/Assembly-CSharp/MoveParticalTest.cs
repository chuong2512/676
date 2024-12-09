using UnityEngine;

public class MoveParticalTest : MonoBehaviour
{
	public GameObject moveParticalPrefab;

	public Transform startTrans;

	public Transform endTrans;

	public float duration;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MovePartical();
		}
	}

	private void MovePartical()
	{
		GameObject newParitcalObject = Object.Instantiate(moveParticalPrefab);
		Vector3 position = endTrans.position;
		Vector3 position2 = startTrans.position;
		position2.z = -1f;
		position.z = -1f;
		float x = Mathf.Lerp(position2.x, position.x, Random.Range(0f, 0.5f));
		float y = Mathf.Lerp(position2.y, position.y, Random.Range(0f, 1f));
		Extension.TransformMoveByBezierWithoutGame(middlePoint: new Vector3(x, y, position2.z), mono: this, target: newParitcalObject.transform, startPoint: position2, endPoint: position, moveTime: duration, endAction: delegate
		{
			Object.Destroy(newParitcalObject);
		});
	}
}
