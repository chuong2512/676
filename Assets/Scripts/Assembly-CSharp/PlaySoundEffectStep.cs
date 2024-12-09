public class PlaySoundEffectStep : BaseEffectStep
{
	public string soundName;

	public bool isLoop;

	public override EffectConfigType EffectConfigType => EffectConfigType.PlaySound;
}
