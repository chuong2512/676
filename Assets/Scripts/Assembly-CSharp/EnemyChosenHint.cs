using UnityEngine;

public class EnemyChosenHint : MonoBehaviour
{
	public Sprite PossibleSprite;

	public Sprite ChosenSprite;

	public Sprite RandomSprite;

	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_spriteRenderer = base.transform.Find("Sprite").GetComponent<SpriteRenderer>();
	}

	public void ShowHint(Transform target, Vector3 offset)
	{
		base.transform.SetParent(target);
		base.transform.position = target.position + offset;
	}

	public void SetPossible()
	{
		_spriteRenderer.sprite = PossibleSprite;
	}

	public void SetChosen()
	{
		_spriteRenderer.sprite = ChosenSprite;
	}

	public void SetRandom()
	{
		_spriteRenderer.sprite = RandomSprite;
	}
}
