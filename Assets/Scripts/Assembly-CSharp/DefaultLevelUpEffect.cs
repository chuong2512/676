public class DefaultLevelUpEffect : PlayerLevelUpEffect
{
	protected override string IconName => "生命恢复20";

	public override string NameKey => "DefaultLevelUpEffectNameKey";

	public override string DesKey => "DefaultLevelUpEffectDesKey";

	public override void Effect()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("特效_治疗增益");
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(20);
	}
}
