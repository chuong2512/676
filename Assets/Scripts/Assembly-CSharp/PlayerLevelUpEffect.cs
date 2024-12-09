using UnityEngine;

public abstract class PlayerLevelUpEffect
{
	private const string IconPath = "Sprites/LevelUpEffectIcon";

	public Sprite IconSprite => SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(IconName, "Sprites/LevelUpEffectIcon");

	protected abstract string IconName { get; }

	public abstract string NameKey { get; }

	public abstract string DesKey { get; }

	public abstract void Effect();
}
